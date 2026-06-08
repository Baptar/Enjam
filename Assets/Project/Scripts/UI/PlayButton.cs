using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class PlayButton : MonoBehaviour
{
    [SerializeField] private CanvasGroup mainMenuCanvasGroup;

    public void Play()
    {
        mainMenuCanvasGroup.interactable = false;
        mainMenuCanvasGroup.blocksRaycasts = false;
        
        Sequence seq = DOTween.Sequence();
        seq.Append(mainMenuCanvasGroup.DOFade(0f, 1f));
        seq.AppendCallback(() => { SceneManager.LoadScene("MainScene"); });
        
        Cursor.visible = false;
    }
}
