using System.Numerics;
using DG.Tweening;
using TMPro;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class PaperInteract : ObjectGrabbable
{
    [Header("References")]
    [SerializeField] private string paperText;
    [SerializeField] private TMP_Text textPaperRef;
    
    [Space(5)]
    [Header("Paper Animation Settings")]
    [SerializeField] private Transform paperEndTransform; 
    [SerializeField] private Ease paperEase = Ease.InOutFlash; 
    [SerializeField] private float moveDuration = 0.25f; 
    
    [Space(5)]
    [Header("Paper Grab Settings")]
    [SerializeField] private float swayPositionAmount = 0.02f;
    [SerializeField] private float swayRotationAmount = 1.5f;
    [SerializeField] private float swaySpeed = 6.0f;
    private Vector3 swayPositionOffset;
    private Quaternion swayRotationOffset;
    private Vector3 lastCameraEuler;
    private bool bUpdateTransform;
    
    private void OnValidate()
    {
        if (!textPaperRef) return;
        textPaperRef.text = paperText;
    }
    
    protected void Start()
    {
        lastCameraEuler = MainManager.instance.Player.GetPlayerCamera().transform.eulerAngles;
    }
    
    protected override void Update()
    {
        if (objectGrabPointTransform == null) return;

        Vector3 cameraDelta = MainManager.instance.Player.GetPlayerCamera().transform.eulerAngles - lastCameraEuler;
    
        if (cameraDelta.x > 180) cameraDelta.x -= 360;
        if (cameraDelta.y > 180) cameraDelta.y -= 360;
    
        cameraDelta.x = Mathf.Clamp(cameraDelta.x, -10f, 10f);
        cameraDelta.y = Mathf.Clamp(cameraDelta.y, -10f, 10f);
    
        lastCameraEuler = MainManager.instance.Player.GetPlayerCamera().transform.eulerAngles;

        Vector3 targetSwayPosition = new Vector3(-cameraDelta.y, -cameraDelta.x, 0) * swayPositionAmount;
        Quaternion targetSwayRotation = Quaternion.Euler(-cameraDelta.y * swayRotationAmount, 0, cameraDelta.x * swayRotationAmount);

        swayPositionOffset = Vector3.Lerp(swayPositionOffset, targetSwayPosition, Time.deltaTime * swaySpeed);
        swayRotationOffset = Quaternion.Lerp(swayRotationOffset, targetSwayRotation, Time.deltaTime * swaySpeed);

        if (!bUpdateTransform) return;
            
        transform.localPosition = swayPositionOffset;
        transform.localRotation = swayRotationOffset;
    }

    protected override void Grab()
    {
        MainManager.instance.PaperManager.SetCurrentPaperRead(this);
        PlaySound("event:/Hall/PaperGrab");
        
        SetObjectGrabPointTransform(MainManager.instance.Player.GetObjectGrabPointPaperTransform());
        GetComponent<Collider>().enabled = false;
        MainManager.instance.Player.SetGrabbedObject(this);
    }

    public void MakePaperAppear()
    {
        gameObject.SetActive(true);
        MovePaper();
    }

    private void MovePaper()
    {
        PlaySound("event:/Hall/PaperAppear");
        Sequence seq = DOTween.Sequence();
        seq.Append(transform.DOMove(paperEndTransform.position, moveDuration).SetEase(paperEase))
            .Insert(0.0f, transform.DORotateQuaternion(paperEndTransform.rotation, moveDuration).SetEase(paperEase))
            .OnComplete(()=> SetInteractable(true));
    }

    public void MakePaperDisappear()
    {
        Drop();
        //Sequence seq = DOTween.Sequence();
        //seq.Append(transform.DOShakePosition(1.0f, 0.1f).SetEase(Ease.InOutFlash))
        //    .Insert(0f, transform.DOScale(Vector3.zero, 1.0f).SetEase(Ease.InOutFlash))
        //    .OnComplete(() => gameObject.SetActive(false));
        gameObject.SetActive(false);
    }
    
    protected override void SetObjectGrabPointTransform(Transform newObjectGrabPoint)
    {
        gameObject.transform.parent = newObjectGrabPoint;
        objectGrabPointTransform = newObjectGrabPoint;
    
        lastCameraEuler = MainManager.instance.Player.GetPlayerCamera().transform.eulerAngles;
    
        Sequence sequence = DOTween.Sequence();
        sequence.Append(transform.DOLocalMove(Vector3.zero, lerpSpeed).SetEase(Ease.InOutFlash))
            .Insert(0.0f, transform.DOLocalRotate(Vector3.zero, lerpSpeed).SetEase(Ease.InOutFlash))
            .OnComplete(() =>
            {
                swayPositionOffset = Vector3.zero;
                swayRotationOffset = Quaternion.identity;
                bUpdateTransform = true;
                MainManager.instance.Player.SetIsReading(true);
            });
    }
}
