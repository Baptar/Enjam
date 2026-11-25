using UnityEngine;

public class EndCorridorAudioManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Transform pointA;
    [SerializeField] Transform pointB;
    [SerializeField] Renderer eyeRenderer;

    const float MaxValue = 20f;
    
    // Eye shader
    private const float MinIntensityValue = 0.01f;
    private const float MaxIntensityValue = 0.2f;
    private Material eyeMaterial;
    private static readonly int IntensityID = Shader.PropertyToID("_Intensity");

    Vector3 a;
    Vector3 scaledAB;
    Transform playerTr;

    [Space(10)]
    [Header("Debug")]
    public float parameterValue;
    public float intensityValue;
    public bool isActivate = false;

    void Awake()
    {
        a = pointA.position;
        Vector3 ab = pointB.position - a;
        float sqr = ab.sqrMagnitude;
        scaledAB = sqr > 1e-6f ? ab * (MaxValue / sqr) : Vector3.zero;
        
        eyeMaterial = eyeRenderer.material;
    }

    void Start()
    {
        FMODUnity.RuntimeManager.StudioSystem.setParameterByName("PL_CrushEnd", 0);
    }

    void Update()
    {
        if (!isActivate) return;

        parameterValue = Mathf.Clamp(Vector3.Dot(playerTr.position - a, scaledAB), 0f, MaxValue);
        intensityValue = Mathf.Lerp(MinIntensityValue, MaxIntensityValue, parameterValue / MaxValue);
        eyeMaterial.SetFloat("_Intensity", intensityValue);
        FMODUnity.RuntimeManager.StudioSystem.setParameterByName("PL_CrushEnd", parameterValue);
    }

    [ContextMenu("Activate")]
    public void Activate()
    {
        playerTr = MainManager.instance.Player.transform;
        isActivate = true;
    }
}