using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndManager : MonoBehaviour
{
    [SerializeField] private string sceneToLoad;
    [SerializeField] private Transform startPosition;
    [SerializeField] private Transform endPosition;
    
    public void TPEndLevel()
    {
        Debug.Log("Ending Level");
        Sequence seq = DOTween.Sequence();
        seq.AppendCallback(()=>
        {
            MainManager.instance.UIManager.FadeScreen(true, 0.75f);
        });
        seq.AppendInterval(0.75f);
        seq.AppendCallback(()=>
        {
            SceneManager.LoadScene(sceneToLoad);
        });
    }

    public void EndBatrhroom()
    {
        MainManager.instance.Player.SetCanMove(false);
        MainManager.instance.Player.SetLookMode(PlayerManager.ELookMode.CantLook);
        
        Sequence seq = DOTween.Sequence();
        seq.Append(MainManager.instance.Player.GetPlayerCamera().transform.DOMove(startPosition.position, 2.0f));
        seq.Join(MainManager.instance.Player.GetPlayerCamera().transform.DORotateQuaternion(startPosition.rotation, 2.0f));
        seq.Append(MainManager.instance.Player.GetPlayerCamera().transform.DOMove(endPosition.position, 4.0f));
        seq.Join(MainManager.instance.Player.GetPlayerCamera().transform.DORotateQuaternion(endPosition.rotation, 4.0f));
        seq.InsertCallback(6.0f, () =>
        {
            MainManager.instance.UIManager.FadeScreen(true, 0.75f);
        });
    }
}
