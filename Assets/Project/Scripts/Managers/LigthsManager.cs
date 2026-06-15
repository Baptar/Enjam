using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class LigthsManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private List<Light> lights = new List<Light>();
    
    [Space(10)]
    [Header("Parameters")]
    [SerializeField] private int blinkCount = 3;
    [SerializeField] private int lightChangePerBlink = 3;
    [SerializeField] private float blinkDuration = 0.05f;
    
    [Space(10)]
    [Header("Start")]
    [SerializeField] private bool startLightsOff = true;
    [SerializeField] private float startDelay = 1.0f;
    [SerializeField] private float turnOnStartDuration = 1.0f;
    [SerializeField] private Ease startEase = Ease.InOutFlash;
    [SerializeField] private float delayBetweenEachLight = 1.0f;
    
    private Sequence blinkSequence;
    private Sequence switchOnStartSequence;


    private IEnumerator Start()
    {
        if (!startLightsOff) yield break;
        
        MainManager.instance.Player.SetCanMove(false);
        MainManager.instance.Player.SetLookMode(PlayerManager.ELookMode.CantLook);
        
        foreach (var light in lights)
        {
            light.DOIntensity(0.0f, 0.1f);
            //light.gameObject.SetActive(false);
        }
        
        yield return new WaitForSeconds(startDelay);
        
        MainManager.instance.Player.SetCanMove(true);
        MainManager.instance.Player.SetLookMode(PlayerManager.ELookMode.Normal);

        SwitchLightsSequenceIntro(true);
    }

    #region Switch Lights
    [ContextMenu("SwitchOnLight")]
    public void SwitchOnLights()
    {
        SwitchLights(true);
    }
    
    [ContextMenu("SwitchOffLight")]
    public void SwitchOffLights()
    {
        SwitchLights(false);
    }
    
    private void SwitchLights(bool enable)
    {
        foreach (Light light in lights)
        {
            var value = light.intensity;
            var endValue = enable ? 40000.0f : 0.0f;
            DOTween.To(() => value, x => value = x, endValue, turnOnStartDuration).SetEase(startEase)
                .OnUpdate(() => light.intensity = value);
        }
    }
    #endregion
    
    #region Blink Lights
    [ContextMenu("BlinkLight")]
    public void BlinkLight()
    {
        BlinkLights(blinkCount, 0.0f, blinkDuration);
    }
    
    private void BlinkLights(int _blinkCount, float _targetIntensity = 20000f, float _blinkDuration = 2.0f)
    {
        blinkSequence?.Kill(complete: false);

        float[] originalIntensities = new float[lights.Count];
        Color[] originalColors = new Color[lights.Count];
        for (int i = 0; i < lights.Count; i++)
        {
            originalIntensities[i] = lights[i].intensity;
            originalColors[i] = lights[i].color;
        }

        blinkSequence = DOTween.Sequence();

        for (int b = 0; b < _blinkCount; b++)
        {
            for (int f = 0; f < lightChangePerBlink; f++)
            {
                float flickerIntensity = originalIntensities[0] * Random.Range(0.2f, 0.85f);
                float flickerDuration = Random.Range(0.03f, 0.07f);

                blinkSequence.Append(lights[0].DOIntensity(flickerIntensity, flickerDuration));
                for (int i = 1; i < lights.Count; i++)
                {
                    float fi = originalIntensities[i] * Random.Range(0.2f, 0.85f);
                    blinkSequence.Join(lights[i].DOIntensity(fi, flickerDuration));
                }
            }

            // off
            blinkSequence.Append(lights[0].DOIntensity(_targetIntensity, _blinkDuration * 0.15f).SetEase(Ease.InExpo));
            for (int i = 1; i < lights.Count; i++)
                blinkSequence.Join(lights[i].DOIntensity(_targetIntensity, _blinkDuration * 0.15f).SetEase(Ease.InExpo));

            // silence
            blinkSequence.AppendInterval(_blinkDuration * Random.Range(0.8f, 1.2f));

            // on
            float hesitationIntensity = originalIntensities[0] * 0.4f;
            blinkSequence.Append(lights[0].DOIntensity(hesitationIntensity, _blinkDuration * 0.2f).SetEase(Ease.OutCubic));
            for (int i = 1; i < lights.Count; i++)
                blinkSequence.Join(lights[i].DOIntensity(originalIntensities[i] * 0.4f, _blinkDuration * 0.2f)
                    .SetEase(Ease.OutCubic));

            blinkSequence.AppendInterval(0.05f);

            blinkSequence.Append(lights[0].DOIntensity(_targetIntensity, _blinkDuration * 0.05f));
            for (int i = 1; i < lights.Count; i++)
                blinkSequence.Join(lights[i].DOIntensity(_targetIntensity, _blinkDuration * 0.05f));

            blinkSequence.AppendInterval(0.04f);

            //  final stabilisation
            blinkSequence.Append(lights[0].DOIntensity(originalIntensities[0], _blinkDuration * 0.4f)
                .SetEase(Ease.OutCubic));
            blinkSequence.Join(lights[0].DOColor(originalColors[0], _blinkDuration * 0.4f));
            for (int i = 1; i < lights.Count; i++)
            {
                blinkSequence.Join(lights[i].DOIntensity(originalIntensities[i], _blinkDuration * 0.4f)
                    .SetEase(Ease.OutCubic));
                blinkSequence.Join(lights[i].DOColor(originalColors[i], _blinkDuration * 0.4f));
            }

            // tint
            blinkSequence.InsertCallback(
                blinkSequence.Duration() - _blinkDuration * 0.6f,
                () =>
                {
                    foreach (var light in lights)
                        light.color = new Color(1f, 0.6f, 0.2f);
                }
            );

            blinkSequence.AppendInterval(_blinkDuration * Random.Range(0.5f, 1.0f));
        }
    }
    #endregion

    private void SwitchLightsSequenceIntro(bool enable)
    {
        switchOnStartSequence?.Kill(complete: true);
        switchOnStartSequence = DOTween.Sequence();

        for (var i = 0; i < lights.Count; i++)
            switchOnStartSequence.Insert(i * delayBetweenEachLight, lights[i].DOIntensity(40000.0f, turnOnStartDuration).SetEase(startEase));
    }

    public void StopBlink()
    {
        blinkSequence?.Kill(complete: true);
    }
}
