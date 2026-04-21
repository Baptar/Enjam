using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class LigthsManager : MonoBehaviour
{
    [SerializeField] private List<Light> lights = new List<Light>();
    private Sequence blinkSequence;

    
    public void SwitchLights(bool enable)
    {
        foreach (Light light in lights)
        {
            var value = light.intensity;
            var endValue = enable ? 40000.0f : 0.0f;
            DOTween.To(() => value, x => value = x, endValue, 1f).SetEase(Ease.InOutFlash)
                .OnUpdate(() => light.intensity = value);
        }
    }

    [ContextMenu("BlinkLight")]
    public void BlinkLight()
    {
        BlinkLights(3, 0.0f, .05f);
    }
    
    public void BlinkLights(int blinkCount, float targetIntensity = 20000f, float blinkDuration = 2.0f)
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

        for (int b = 0; b < blinkCount; b++)
        {
            for (int f = 0; f < 4; f++)
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

            // OFF
            blinkSequence.Append(lights[0].DOIntensity(targetIntensity, blinkDuration * 0.15f).SetEase(Ease.InExpo));
            for (int i = 1; i < lights.Count; i++)
                blinkSequence.Join(lights[i].DOIntensity(targetIntensity, blinkDuration * 0.15f).SetEase(Ease.InExpo));

            // Silence
            blinkSequence.AppendInterval(blinkDuration * Random.Range(0.8f, 1.2f));

            // ON
            float hesitationIntensity = originalIntensities[0] * 0.4f;
            blinkSequence.Append(lights[0].DOIntensity(hesitationIntensity, blinkDuration * 0.2f).SetEase(Ease.OutCubic));
            for (int i = 1; i < lights.Count; i++)
                blinkSequence.Join(lights[i].DOIntensity(originalIntensities[i] * 0.4f, blinkDuration * 0.2f)
                    .SetEase(Ease.OutCubic));

            blinkSequence.AppendInterval(0.05f);

            blinkSequence.Append(lights[0].DOIntensity(targetIntensity, blinkDuration * 0.05f));
            for (int i = 1; i < lights.Count; i++)
                blinkSequence.Join(lights[i].DOIntensity(targetIntensity, blinkDuration * 0.05f));

            blinkSequence.AppendInterval(0.04f);

            //  Finak stabilisation
            blinkSequence.Append(lights[0].DOIntensity(originalIntensities[0], blinkDuration * 0.4f)
                .SetEase(Ease.OutCubic));
            blinkSequence.Join(lights[0].DOColor(originalColors[0], blinkDuration * 0.4f));
            for (int i = 1; i < lights.Count; i++)
            {
                blinkSequence.Join(lights[i].DOIntensity(originalIntensities[i], blinkDuration * 0.4f)
                    .SetEase(Ease.OutCubic));
                blinkSequence.Join(lights[i].DOColor(originalColors[i], blinkDuration * 0.4f));
            }

            // Tint
            blinkSequence.InsertCallback(
                blinkSequence.Duration() - blinkDuration * 0.6f,
                () =>
                {
                    foreach (var light in lights)
                        light.color = new Color(1f, 0.6f, 0.2f);
                }
            );

            blinkSequence.AppendInterval(blinkDuration * Random.Range(0.5f, 1.0f));
        }
    }

    public void StopBlink()
    {
        blinkSequence?.Kill(complete: true);
    }
}
