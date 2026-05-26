using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TMP_Text interactionText;
    [SerializeField] private Image fadeImage;
    [SerializeField] private Image crosshairImage;

    [Space(10)]
    [Header("Parameters")]
    [SerializeField] private float delayShowCrosshair = 1.5f;

    private IEnumerator Start()
    {
        FadeScreen(false, 0.0f);

        if (delayShowCrosshair <= 0)
        {
            EnableCrosshair(true);
            yield break;
        }
        
        EnableCrosshair(false);
        yield return new WaitForSeconds(delayShowCrosshair);
        EnableCrosshair(true);
        
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
