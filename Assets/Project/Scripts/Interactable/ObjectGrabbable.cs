using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectGrabbable : ObjectInteractable
{
    [SerializeField] protected float lerpSpeed = 10.0f;
    
    private Rigidbody objectRigidBody;
    private Transform objectGrabPointTransform;

    // Get Rigidbody Component
    private void Awake() => objectRigidBody = GetComponent<Rigidbody>();
    
    // Move Grabbed element
    private void FixedUpdate()
    {
        if (objectGrabPointTransform == null) return;
        
        Vector3 newPosition = Vector3.Lerp(transform.localPosition, objectGrabPointTransform.localPosition, Time.deltaTime * lerpSpeed);
        objectRigidBody.MovePosition(newPosition);
    }

    // Interact with element
    public override void Interact()
    {
        // Interactable
        if (GetInteractable())
        {
            SetInteractable(false);
            if (eventSoundOnInteract != "") PlaySound(eventSoundOnInteract);
            eventOnInteract?.Invoke();
            Grab();
        }
        // Not Interactable
        else eventOnInteractButNotInteractable?.Invoke();
    }
    
    // Grab this Object
    public void Grab()
    {
        objectGrabPointTransform = MainManager.instance.Player.GetObjectGrabPointTransform();
        objectRigidBody.isKinematic = true;
        GetComponent<Collider>().enabled = false;
        MainManager.instance.Player.SetIsGrabbingObject(true);
    }

    public override bool GetInteractable() => !MainManager.instance.Player.GetIsGrabbingObject() && bInteractable;
}
