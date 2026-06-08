using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class PlayButton : MonoBehaviour
{
    [Header("Set in inspector")]
    [SerializeField] private CanvasGroup mainMenuCanvasGroup;
    [SerializeField] private InputManager inputManager;

    public void Play()
    {
        mainMenuCanvasGroup.interactable = false;
        mainMenuCanvasGroup.blocksRaycasts = false;
        
        Sequence seq = DOTween.Sequence();
        seq.Append(mainMenuCanvasGroup.DOFade(0f, 1f));
        seq.AppendCallback(() => { SceneManager.LoadScene("MainScene"); });

        inputManager.GameState = GameState.Game;
        inputManager.ShowCursor(false);
    }
}
