using DG.Tweening;
using UnityEngine;

public class LightZone : MonoBehaviour
{
    [SerializeField] private float enterLightIntensity;
    [SerializeField] private float exitLightIntensity;
    [SerializeField] private float duration = 1.0f;
    [SerializeField] private Ease ease = Ease.InOutFlash;
    
    private Sequence sequenceLight;
    
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }
        
        sequenceLight?.Kill();
        sequenceLight = DOTween.Sequence();

        sequenceLight.Append(MainManager.instance.Player.cameraLight.DOIntensity(enterLightIntensity, duration).SetEase(ease));
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }
        
        sequenceLight?.Kill();
        sequenceLight = DOTween.Sequence();

        sequenceLight.Append(MainManager.instance.Player.cameraLight.DOIntensity(exitLightIntensity, duration).SetEase(ease));
    }
}
