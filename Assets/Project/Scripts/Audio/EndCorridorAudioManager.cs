using UnityEngine;

public class EndCorridorAudioManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Transform pointA;
    [SerializeField] Transform pointB;

    const float MaxValue = 20f;

    Vector3 a;
    Vector3 scaledAB;
    Transform playerTr;

    [Space(10)]
    [Header("Debug")]
    public float parameterValue;
    public bool isActivate = false;

    void Awake()
    {
        a = pointA.position;
        Vector3 ab = pointB.position - a;
        float sqr = ab.sqrMagnitude;
        scaledAB = sqr > 1e-6f ? ab * (MaxValue / sqr) : Vector3.zero;
    }

    void Start()
    {
        FMODUnity.RuntimeManager.StudioSystem.setParameterByName("PL_CrushEnd", 0);
    }

    void Update()
    {
        if (!isActivate) return;

        parameterValue = Mathf.Clamp(Vector3.Dot(playerTr.position - a, scaledAB), 0f, MaxValue);
        FMODUnity.RuntimeManager.StudioSystem.setParameterByName("PL_CrushEnd", parameterValue);
    }

    [ContextMenu("Activate")]
    public void Activate()
    {
        playerTr = MainManager.instance.Player.transform;
        isActivate = true;
    }
}