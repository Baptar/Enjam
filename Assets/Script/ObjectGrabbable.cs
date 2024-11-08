using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectGrabbable : MonoBehaviour
{
    public string eventLocName;
    public PlayerPickUp playerPickUp;
    public bool canTake = true;
	public string textInteraction;
    private Rigidbody objectRigidBody;
    private Transform objectGrabPointTransform;
    [SerializeField] private float lerpSpeed = 10.0f;

    private void Awake()
    {
        objectRigidBody = GetComponent<Rigidbody>();
    }
    public void Grab(Transform objectGrabPointTransform)
    {
        Debug.Log("Grab");
        this.objectGrabPointTransform = objectGrabPointTransform;
        objectRigidBody.isKinematic = true;
    }
    
    public void Drop()
    {
        this.objectGrabPointTransform = null;
        objectRigidBody.isKinematic = false;
        playerPickUp.bHasGrabbleObject = false;
    }

    private void FixedUpdate()
    {
        if (objectGrabPointTransform != null)
        {
            Vector3 newPosition = Vector3.Lerp(transform.position, objectGrabPointTransform.position, Time.deltaTime * lerpSpeed);
            objectRigidBody.MovePosition(newPosition);
        }
    }

	public string GetText()
    {
		if (textInteraction == "") return "Press E to Grab";
        return textInteraction;
    }

    public void OnTook(PlayerPickUp playerPickUp)
    {
        if (eventLocName !="")
            FMODUnity.RuntimeManager.PlayOneShot(eventLocName, transform.position);
        GetComponent<Collider>().enabled = false;
    }
}
