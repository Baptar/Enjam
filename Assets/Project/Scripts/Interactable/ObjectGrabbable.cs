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

        Vector3 newPosition = Vector3.Lerp(transform.position, objectGrabPointTransform.position, Time.deltaTime * lerpSpeed);
        objectRigidBody.MovePosition(newPosition);
        
        Vector3 targetRotationEuler = objectGrabPointTransform.rotation.eulerAngles;
        targetRotationEuler.x = 0.0f;
        targetRotationEuler.z = 0.0f;
        Quaternion targetRotation = blockYOnGrabbed ? Quaternion.Euler(targetRotationEuler) : objectGrabPointTransform.rotation;
        Quaternion newRotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * lerpSpeed);
        objectRigidBody.MoveRotation(newRotation);
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

    public virtual void Drop()
    {
        OnDropEvent?.Invoke();
    }

    public override bool GetInteractable() => !MainManager.instance.Player.GetGrabbedObject() && bInteractable;

    protected virtual void SetObjectGrabPointTransform(Transform newObjectGrabPoint)
    {
        objectGrabPointTransform = newObjectGrabPoint;
    }
}
