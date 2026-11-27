using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class MainMenu2Manager : MonoBehaviour
{
    [SerializeField] private HDAdditionalLightData targetLight;
    [SerializeField] private float delayStart = 2.0f;
    [SerializeField] private float durationLight = 1.0f;
    [SerializeField] private float lightIntensity = 40000.0f;

    private void Awake()
    {
        targetLight.intensity = 0.0f;
    }

    private void Start()
    {
        DOVirtual.Float(0f, lightIntensity, durationLight, v =>
            {
                if (targetLight != null) targetLight.intensity = v;
            })
            .SetDelay(delayStart)
            .SetEase(Ease.OutBounce)
            .SetLink(gameObject);
        
        Sequence seq = DOTween.Sequence();
        seq.InsertCallback(delayStart, () => FMODUnity.RuntimeManager.PlayOneShot("event:/introlightswitch2"));
    }
}