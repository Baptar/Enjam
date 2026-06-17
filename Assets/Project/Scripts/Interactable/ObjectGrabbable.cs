using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class ObjectGrabbable : ObjectInteractable
{
    [Space(5)]
    [Header("Grab setting")]
    [SerializeField] protected float lerpSpeed = 20.0f;
    [SerializeField] protected bool blockYOnGrabbed = true;
    [SerializeField] protected UnityEvent OnDropEvent;
    [SerializeField] private float xValue = 0.0f;
    [SerializeField] private float zValue = 0.0f;

    [Space(5)]
    [Header("Drop setting")]
    [SerializeField] private bool bShouldFall = true;
    
    protected Rigidbody objectRigidBody;
    protected Transform objectGrabPointTransform;
    protected Collider objectCollider;
    [HideInInspector] public bool bFollowTargetPoint = true;

    // Get Rigidbody Component
    protected virtual void Awake()
    {
        objectRigidBody = GetComponent<Rigidbody>();
        objectCollider = GetComponent<Collider>();
    }
    
    
    // Move Grabbed element
    protected virtual void Update()
    {
        if (objectGrabPointTransform == null || !bFollowTargetPoint) return;
        
        transform.position = Vector3.Lerp(transform.position, objectGrabPointTransform.position, Time.deltaTime * lerpSpeed);
            
        Vector3 targetEuler = objectGrabPointTransform.rotation.eulerAngles;
        if (blockYOnGrabbed) { targetEuler.x = xValue; targetEuler.z = zValue; }
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(targetEuler), Time.deltaTime * lerpSpeed);
    }

    // Interact with element
    [ContextMenu("Interact")]
    public override void Interact()
    {
        // Interactable
        if (GetInteractable())
        {
            SetInteractable(false);
            if (!eventSoundOnInteract.IsNull) PlaySound(eventSoundOnInteract);
            eventOnInteract?.Invoke();
            Grab();
        }
        // Not Interactable
        else eventOnInteractButNotInteractable?.Invoke();
    }
    
    // Grab this Object
    protected virtual void Grab()
    {
        SetObjectGrabPointTransform(MainManager.instance.Player.GetObjectGrabPointTransform());
        if (objectRigidBody)
        {
            objectRigidBody.isKinematic = true;
            objectRigidBody.freezeRotation = true;
        }
        if (objectCollider) objectCollider.enabled = false;
        MainManager.instance.Player.SetGrabbedObject(this);
    }

    [ContextMenu("Drop Object")]
    public virtual void Drop()
    {
        OnDropEvent?.Invoke();
        
        objectGrabPointTransform = null;
        if (!objectRigidBody) return;
        
        if (!bShouldFall) return;
        objectCollider.enabled = true;
        objectRigidBody.freezeRotation = false;
        objectRigidBody.isKinematic = false;
    }

    public override bool GetInteractable() => !MainManager.instance.Player.GetGrabbedObject() && bInteractable;

    protected virtual void SetObjectGrabPointTransform(Transform newObjectGrabPoint)
    {
        objectGrabPointTransform = newObjectGrabPoint;
    }

    public void FadeOut(Material mat)
    {
        float valFloat = 0.0f;
        
        DOTween.To(() => valFloat, x => valFloat = x, 1.0f, 1f)
            .OnUpdate(() =>
            {
                mat.SetFloat("_Fade", valFloat);
            })
            /*.OnComplete(()=> gameObject.SetActive(false))*/;
    }

    public void InitDissolveMaterial(Material mat)
    {
        mat.SetFloat("_Fade", 0.0f);
    }

    public void MoveToActorAndDiseaper(GameObject actor)
    {
        gameObject.transform.DOMove(actor.transform.position, 0.5f).SetEase(Ease.InOutFlash)
            .OnComplete(()=>gameObject.SetActive(false));
    }
}
