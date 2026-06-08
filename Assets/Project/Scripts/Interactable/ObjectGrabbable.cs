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
    [SerializeField] private UnityEvent OnDropEvent;

    
    protected Rigidbody objectRigidBody;
    protected Transform objectGrabPointTransform;

    // Get Rigidbody Component
    protected virtual void Awake()
    {
        objectRigidBody = GetComponent<Rigidbody>();
    }
    
    
    // Move Grabbed element
    protected virtual void Update()
    {
        if (objectGrabPointTransform == null) return;
        
        transform.position = Vector3.Lerp(transform.position, objectGrabPointTransform.position, Time.deltaTime * lerpSpeed);
    
        Vector3 targetEuler = objectGrabPointTransform.rotation.eulerAngles;
        if (blockYOnGrabbed) { targetEuler.x = 0f; targetEuler.z = 0f; }
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(targetEuler), Time.deltaTime * lerpSpeed);
    }

    // Interact with element
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
        if (objectRigidBody) objectRigidBody.isKinematic = true;
        GetComponent<Collider>().enabled = false;
        MainManager.instance.Player.SetGrabbedObject(this);
    }

    [ContextMenu("Drop Object")]
    public virtual void Drop()
    {
        OnDropEvent?.Invoke();
        if (!objectRigidBody) return;
        
        GetComponent<Collider>().enabled = true;
        objectGrabPointTransform = null;
        objectRigidBody.isKinematic = false;
    }

    public override bool GetInteractable() => !MainManager.instance.Player.GetGrabbedObject() && bInteractable;

    protected virtual void SetObjectGrabPointTransform(Transform newObjectGrabPoint)
    {
        objectGrabPointTransform = newObjectGrabPoint;
    }
}
