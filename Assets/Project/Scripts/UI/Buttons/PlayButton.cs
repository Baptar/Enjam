using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class PlayButton : MonoBehaviour
{
    [Header("Set in inspector")]
    [SerializeField] private CanvasGroup mainMenuCanvasGroup;
    [SerializeField] private InputManager inputManager;
    [SerializeField] private Light lightLampadaire;
    [SerializeField] private float durationLight;

    public void Play()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/UI/UI_Start");
        mainMenuCanvasGroup.interactable = false;
        mainMenuCanvasGroup.blocksRaycasts = false;
        
        Sequence seq = DOTween.Sequence();
        seq.Append(mainMenuCanvasGroup.DOFade(0f, 0f));
        seq.AppendCallback(() => { SceneManager.LoadScene("MainScene"); });

        inputManager.GameState = GameState.Game;
        inputManager.ShowCursor(false);
    }
    
    public void PlayOtherMainMenu()
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append( lightLampadaire.DOIntensity(0.0f, durationLight).SetEase(Ease.InOutQuad));
        sequence.AppendCallback(
            () =>
            {
                FMODUnity.RuntimeManager.PlayOneShot("event:/UI/UI_Start");
                mainMenuCanvasGroup.interactable = false;
                mainMenuCanvasGroup.blocksRaycasts = false;

                Sequence seq = DOTween.Sequence();
                seq.Append(mainMenuCanvasGroup.DOFade(0f, 0f));
                seq.AppendCallback(() => { SceneManager.LoadScene("MainScene"); });

                inputManager.GameState = GameState.Game;
                inputManager.ShowCursor(false);
            });
    }
}
