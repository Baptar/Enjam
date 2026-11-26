using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class EndManager : MonoBehaviour
{
    [FormerlySerializedAs("sceneToLoad")] [SerializeField] private string endLevel;

    [Header("Camera Path")]
    [SerializeField] private Transform startPosition;
    [SerializeField] private Transform midPosition;
    [SerializeField] private Transform endPosition;

    [Header("Timings")]
    [SerializeField] private float durationToStart = 2.0f;
    [SerializeField] private float durationStartToMid = 3.0f;
    [SerializeField] private float durationMidToEnd = 4.0f;

    [Header("Event")]
    [SerializeField] private float delayAfterStart = 6.0f;
    [SerializeField] private UnityEvent onDelayAfterStart;
    [SerializeField] private UnityEvent onStartBathroom;

    [Header("Credit")]
    [SerializeField] private Transform creditsPositionEnd;
    [SerializeField] private GameObject creditsGameObject;
    [SerializeField] private float durationCredit = 7.0f;

    [Header("MainMenu")]
    [SerializeField] private string mainMenuScene;
    [SerializeField] private float delayMainMenu = 2.0f;

    // Séquence de la caméra de fin : gardée pour pouvoir la tuer avant un changement de scène
    private Sequence endSeq;

    public void TPEndLevel()
    {
        Debug.Log("Ending Level");
        KillEndSequence();
        SceneManager.LoadScene(endLevel);
    }

    public void EndBatrhroom()
    {
        MainManager.instance.Player.SetCanMove(false);
        MainManager.instance.Player.SetLookMode(PlayerManager.ELookMode.CantLook);
        onStartBathroom?.Invoke();

        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX_BigEND");
        
        Transform cam = MainManager.instance.Player.GetPlayerCamera().transform;

        Vector3 p0 = startPosition.position;
        Vector3 p1 = midPosition.position;
        Vector3 p2 = endPosition.position;
        Quaternion r0 = startPosition.rotation;
        Quaternion r1 = midPosition.rotation;
        Quaternion r2 = endPosition.rotation;

        float d1 = durationStartToMid;
        float d2 = durationMidToEnd;
        float total = d1 + d2;

        Vector3 vMid = new Vector3(
            MidTangent(p1.x - p0.x, d1, p2.x - p1.x, d2),
            MidTangent(p1.y - p0.y, d1, p2.y - p1.y, d2),
            MidTangent(p1.z - p0.z, d1, p2.z - p1.z, d2)
        );
        float sMid = 2f / total;

        // Si EndBatrhroom est appelé deux fois, on repart proprement
        KillEndSequence();
        endSeq = DOTween.Sequence();
        Sequence seq = endSeq;

        seq.Append(cam.DOMove(p0, durationToStart).SetEase(Ease.InOutSine));
        seq.Join(cam.DORotateQuaternion(r0, durationToStart).SetEase(Ease.InOutSine));

        seq.Append(DOVirtual.Float(0f, total, total, t =>
        {
            if (cam == null) return; // garde-fou : la caméra a été détruite

            Vector3 pos;
            float s;

            if (t <= d1)
            {
                float u = t / d1;
                Hermite(u, out float h00, out float h10, out float h01, out float h11);
                pos = h00 * p0 + h01 * p1 + h11 * d1 * vMid;
                s = h01 * 1f + h11 * d1 * sMid;
            }
            else
            {
                float u = (t - d1) / d2;
                Hermite(u, out float h00, out float h10, out float h01, out float h11);
                pos = h00 * p1 + h10 * d2 * vMid + h01 * p2;
                s = h00 * 1f + h10 * d2 * sMid + h01 * 2f;
            }

            cam.position = pos;
            cam.rotation = s <= 1f
                ? Quaternion.Slerp(r0, r1, s)
                : Quaternion.Slerp(r1, r2, s - 1f);
        }).SetEase(Ease.Linear));

        seq.InsertCallback(delayAfterStart, () => onDelayAfterStart?.Invoke());

        // Tue automatiquement la séquence si la caméra (donc le joueur) est détruite
        seq.SetLink(cam.gameObject);
    }

    private void KillEndSequence()
    {
        if (endSeq != null && endSeq.IsActive())
        {
            endSeq.Kill();
        }
        endSeq = null;
    }

    private void OnDestroy()
    {
        KillEndSequence();
    }

    private static float MidTangent(float delta1, float time1, float delta2, float time2)
    {
        float a = delta1 / time1;
        float b = delta2 / time2;
        if (a * b <= 0f) return 0f;
        return 2f * a * b / (a + b);
    }

    private void OnDrawGizmos()
    {
        if (startPosition == null || midPosition == null || endPosition == null) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(startPosition.position, midPosition.position);
        Gizmos.DrawLine(midPosition.position, endPosition.position);
        Gizmos.DrawSphere(startPosition.position, 0.05f);
        Gizmos.DrawSphere(midPosition.position, 0.05f);
        Gizmos.DrawSphere(endPosition.position, 0.05f);
    }

    private static void Hermite(float u, out float h00, out float h10, out float h01, out float h11)
    {
        float u2 = u * u;
        float u3 = u2 * u;
        h00 = 2f * u3 - 3f * u2 + 1f;
        h10 = u3 - 2f * u2 + u;
        h01 = -2f * u3 + 3f * u2;
        h11 = u3 - u2;
    }

    public void FadeAndUnlockEnding()
    {
        MainManager.instance.UIManager.FadeScreen(true, 0.75f);
        if (SteamManager.Initialized) SteamManager.Instance.UnlockAchievement("LAMAPADRIE_SUCCESS_ENDING");
    }

    public void StartCredit()
    {
        creditsGameObject.transform
            .DOLocalMoveY(creditsPositionEnd.localPosition.y, durationCredit)
            .SetLink(creditsGameObject);
    }

    [ContextMenu("MainMenuScene")]
    public void MainMenuScene()
    {
        string sceneToLoad = mainMenuScene;

        // Volontairement pas lié à EndManager : le chargement du menu doit avoir lieu même si l'objet disparaît
        Sequence seq = DOTween.Sequence();
        seq.AppendInterval(delayMainMenu);
        seq.AppendCallback(() =>
        {
            // On coupe la séquence caméra avant de quitter la scène
            KillEndSequence();
            SceneManager.LoadScene(sceneToLoad);
        });
    }
}