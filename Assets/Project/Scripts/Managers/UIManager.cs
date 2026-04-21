using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TMP_Text interactionText;
    [SerializeField] private Image fadeImage;
    [SerializeField] private Image crosshairImage;


    private void Start()
    {
        FadeScreen(false, 0.0f);
    }
    
    public void EnableCrosshair(bool value) => crosshairImage.DOColor(value ? Color.white : Color.clear, 0.2f).SetEase(Ease.InOutFlash);
    public void EnableInteractionText(bool value) => interactionText.DOColor(value ? Color.white : Color.clear, 0.2f).SetEase(Ease.InOutFlash);

    public void SetCanvaTextInteract(string newText)
    {
        interactionText.text = newText;
        
    }

    public void FadeScreen(bool fadeOut, float duration)
    {
        fadeImage.DOColor(fadeOut ? Color.black : Color.clear, duration).SetEase(Ease.InOutFlash);
    }
}
