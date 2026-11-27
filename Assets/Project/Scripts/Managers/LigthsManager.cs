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
    [Tooltip("Nombre de cycles de clignotement")]
    [SerializeField] private int blinkCount = 3;
    [Tooltip("Nombre de variations d'intensité aléatoires (entre 20 % et 85 %) avant chaque extinction")]
    [SerializeField] private int lightChangePerBlink = 3;
    [Tooltip("Durée de base de l'extinction, du noir et du rallumage")]
    [SerializeField] private float blinkDuration = 0.05f;
    
    [Space(10)]
    [Header("Start")]
    [SerializeField] private bool startLightsOff = true;
    [Tooltip("Attente avant l'intro. Le joueur ne peut ni bouger ni regarder pendant ce temps")]
    [SerializeField] private float startDelay = 1.0f;
    [Tooltip("Durée du fondu de la première lumière")]
    [SerializeField] private float turnOnStartDuration = 1.0f;
    [Tooltip("Courbe de ce fondu")]
    [SerializeField] private Ease startEase = Ease.InOutFlash;
    [Tooltip("Écart entre les départs de deux lumières qui se suivent, sans attendre que la précédente ait fini")]
    [SerializeField] private float delayBetweenEachLight = 1.0f;
    
    [Space(10)]
    [Header("Start Blink")]
    [Tooltip("Nombre de tentatives d'allumage avant que la lumière prenne")]
    [SerializeField] private Vector2Int startAttempts = new Vector2Int(2, 5);
    [Tooltip("Pause après chaque tentative")]
    [SerializeField] private Vector2 startPauseDuration = new Vector2(0.05f, 0.35f);
    [Tooltip("Durée de la montée jusqu'à 80–90 % d'intensité, avant la stabilisation à 100 %")]
    [SerializeField] private Vector2 startWarmUpDuration = new Vector2(0.25f, 0.8f);
    [Tooltip("Avance ou retard aléatoire appliqué au départ de chaque lumière")]
    [SerializeField] private float startTimeJitter = 0.25f;
    
    [SerializeField] private string[] startLightSounds = new string[]
    {
        "",
        "event:/introlightswitch1",
        "event:/introlightswitch2",
        "event:/introlightswitch3"
    };
    
    private const float FullIntensity = 40000.0f;
    
    private enum EBlinkStyle { Stutter, Struggle, Snap }
    
    private Sequence blinkSequence;
    private Sequence switchOnStartSequence;

    private void Awake()
    {
        foreach (var light in lights)
        {
            light.intensity = 0.0f;
            //light.gameObject.SetActive(false);
        }
    }
    
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

        SwitchLightsSequenceIntro();
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

    private void SwitchLightsSequenceIntro()
    {
        switchOnStartSequence?.Kill(complete: true);
        switchOnStartSequence = DOTween.Sequence();

        int previousStyle = -1;

        for (var i = 0; i < lights.Count; i++)
        {
            // la première : fondu simple
            if (i == 0)
            {
                switchOnStartSequence.Insert(0.0f,
                    lights[i].DOIntensity(FullIntensity, turnOnStartDuration).SetEase(startEase));
                continue;
            }

            // jamais deux fois le même style à la suite
            int style = Random.Range(0, 3);
            if (style == previousStyle) style = (style + Random.Range(1, 3)) % 3;
            previousStyle = style;

            float startTime = Mathf.Max(0.0f, i * delayBetweenEachLight + Random.Range(-startTimeJitter, startTimeJitter));

            switchOnStartSequence.Insert(startTime, CreateStartBlink(lights[i], (EBlinkStyle)style));

// son au début du clignotement de la lumière i
            if (i < startLightSounds.Length && !string.IsNullOrEmpty(startLightSounds[i]))
            {
                string sound = startLightSounds[i]; // copie locale pour la closure
                switchOnStartSequence.InsertCallback(startTime, () => FMODUnity.RuntimeManager.PlayOneShot(sound));
            }

        }
    }
    
    

    private Sequence CreateStartBlink(Light light, EBlinkStyle style)
    {
        Sequence sequence = DOTween.Sequence();
        int attempts = Random.Range(startAttempts.x, startAttempts.y + 1);

        for (int a = 0; a < attempts; a++)
        {
            switch (style)
            {
                // rafale de micro coupures, comme un néon qui s'amorce
                case EBlinkStyle.Stutter:
                    int sparks = Random.Range(2, 6);
                    for (int s = 0; s < sparks; s++)
                    {
                        AppendFlash(sequence, light, Random.Range(0.15f, 0.75f), Random.Range(0.01f, 0.05f));
                        AppendCut(sequence, light, 0.0f, Random.Range(0.02f, 0.07f));
                    }
                    break;

                // faible lueur qui tremble et n'arrive pas à prendre
                case EBlinkStyle.Struggle:
                    float glow = Random.Range(0.05f, 0.25f);
                    AppendFlash(sequence, light, glow, Random.Range(0.08f, 0.25f));
                    AppendFlash(sequence, light, glow * Random.Range(0.4f, 0.7f), Random.Range(0.02f, 0.08f));
                    AppendFlash(sequence, light, glow * Random.Range(1.5f, 2.5f), Random.Range(0.02f, 0.06f));
                    AppendCut(sequence, light, 0.0f, 0.0f);
                    break;

                // éclair franc suivi d'une retombée
                case EBlinkStyle.Snap:
                    AppendFlash(sequence, light, Random.Range(0.7f, 1.1f), Random.Range(0.02f, 0.1f));
                    AppendCut(sequence, light, Random.Range(0.05f, 0.15f), Random.Range(0.03f, 0.12f));
                    AppendCut(sequence, light, 0.0f, 0.0f);
                    break;
            }

            // pause irrégulière, parfois une longue hésitation
            float pause = Random.Range(startPauseDuration.x, startPauseDuration.y);
            if (Random.value < 0.2f) pause *= 2.0f;
            sequence.AppendInterval(pause);
        }

        // la lumière prend enfin
        AppendFlash(sequence, light, Random.Range(0.8f, 1.0f), Random.Range(0.03f, 0.08f));
        AppendFlash(sequence, light, Random.Range(0.25f, 0.5f), Random.Range(0.03f, 0.1f));

        // chauffe puis stabilisation
        sequence.Append(light.DOIntensity(FullIntensity * Random.Range(0.8f, 0.9f),
            Random.Range(startWarmUpDuration.x, startWarmUpDuration.y)).SetEase(Ease.OutQuad));

        // dernier sursaut occasionnel
        if (Random.value < 0.5f)
            AppendFlash(sequence, light, Random.Range(0.45f, 0.65f), Random.Range(0.02f, 0.05f));

        sequence.Append(light.DOIntensity(FullIntensity, Random.Range(0.1f, 0.3f)).SetEase(Ease.OutSine));

        return sequence;
    }

    // montée quasi instantanée, puis maintien
    private void AppendFlash(Sequence sequence, Light light, float ratio, float holdDuration)
    {
        sequence.Append(light.DOIntensity(FullIntensity * ratio, 0.01f).SetEase(Ease.Linear));
        sequence.AppendInterval(holdDuration);
    }

    // extinction avec une légère rémanence, puis maintien
    private void AppendCut(Sequence sequence, Light light, float ratio, float holdDuration)
    {
        sequence.Append(light.DOIntensity(FullIntensity * ratio, Random.Range(0.02f, 0.08f)).SetEase(Ease.OutExpo));
        sequence.AppendInterval(holdDuration);
    }

    public void StopBlink()
    {
        blinkSequence?.Kill(complete: true);
    }
}