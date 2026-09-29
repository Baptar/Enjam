using UnityEngine;
using Random = UnityEngine.Random;

public class JudasGenerator : MonoBehaviour
{
    private JudasGrabObj[] judas;

    [Header("References")]
    [SerializeField] private Transform pointSpawn;
    [SerializeField] private Material judasMaterial;
    public Material judasMaterialFade;

    [Header("vertical power")]
    [SerializeField] private float forceVerticaleMin = 8f;
    [SerializeField] private float forceVerticaleMax = 14f;

    [Header("horizontal power")]
    [SerializeField] private float forceHorizontaleMin = 2f;
    [SerializeField] private float forceHorizontaleMax = 6f;
    
    [Space(10)]
    [Header("Debug")]
    [SerializeField] private bool debug = true;

    
    [ContextMenu("Generate")]
    public void Generate()
    {
        foreach (JudasGrabObj rb in judas)
        {
            Vector3 posSpawn = pointSpawn != null ? pointSpawn.position : transform.position;
            Rigidbody rbObj = rb.GetComponent<Rigidbody>();

            rbObj.isKinematic = false;
            rbObj.useGravity = true;
            rbObj.velocity = Vector3.zero;
            rbObj.angularVelocity = Vector3.zero;
            rbObj.constraints = RigidbodyConstraints.None;

            float angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
            Vector3 directionHorizontale = new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle));

            float forceV = Random.Range(forceVerticaleMin, forceVerticaleMax);
            float forceH = Random.Range(forceHorizontaleMin, forceHorizontaleMax);

            Vector3 forceFinale = (Vector3.up * forceV) + (directionHorizontale * forceH);

            UseGravity(true);
            rbObj.AddForce(forceFinale, ForceMode.Impulse);
            rbObj.AddTorque(Random.insideUnitSphere * forceV, ForceMode.Impulse);

            if (debug)
            {
                Debug.DrawRay(posSpawn, forceFinale, Color.red, 20f);
            }
        }
    }
    
    private void Start()
    {
        InitJudasArray();
        UseGravity(false);
    }

    private void InitJudasArray()
    {
        judas = GetComponentsInChildren<JudasGrabObj>();
        if (judas.Length == 0)
        {
            enabled = false;
        }
    }

    private void UseGravity(bool value)
    {
        foreach (JudasGrabObj rb in judas)
        {
            Rigidbody rbObj = rb.GetComponent<Rigidbody>();
            rbObj.useGravity = value;
        }
    }
    
    public void RemoveOtherJudas(JudasGrabObj judasObject)
    {
        judasObject.GetComponent<MeshRenderer>().material = judasMaterial;
        
        
        foreach (JudasGrabObj juda in judas)
        {
            if (juda != judasObject)
            {
                juda.FadeOut(judasMaterialFade);
                Destroy(juda, 3.0f);
            }
        }
    }
    
    /*
     [ContextMenu("Generate")]
    public void Generate()
    {
        generateSequence?.Kill();
        generateSequence = DOTween.Sequence();

        Vector3 posSpawn = pointSpawn != null ? pointSpawn.position : transform.position;

        foreach (JudasGrabObj rb in judas)
        {
            JudasGrabObj current = rb;

            generateSequence.AppendCallback(() => LaunchObject(current, posSpawn));
            generateSequence.AppendInterval(intervalle);
        }
    }
    
    private void LaunchObject(JudasGrabObj rb, Vector3 posSpawn)
    {
        if (rb == null) return;

        Rigidbody rbObj = rb.GetComponent<Rigidbody>();

        rbObj.isKinematic = false;
        rbObj.useGravity = true;
        rbObj.velocity = Vector3.zero;
        rbObj.angularVelocity = Vector3.zero;
        rbObj.constraints = RigidbodyConstraints.None;

        float angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
        Vector3 directionHorizontale = new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle));

        float forceV = Random.Range(forceVerticaleMin, forceVerticaleMax);
        float forceH = Random.Range(forceHorizontaleMin, forceHorizontaleMax);

        Vector3 forceFinale = (Vector3.up * forceV) + (directionHorizontale * forceH);

        UseGravity(true);
        rbObj.AddForce(forceFinale, ForceMode.Impulse);
        rbObj.AddTorque(Random.insideUnitSphere * forceV, ForceMode.Impulse);

        if (debug)
        {
            Debug.DrawRay(posSpawn, forceFinale, Color.red, 20f);
        }
    }
     */
}
