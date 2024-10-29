using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectGrabbable : MonoBehaviour
{
    private Rigidbody objectRigidBody;
    private Transform objectGrabPointTransform;
    [SerializeField] private float lerpSpeed = 10.0f;

    private void Awake()
    {
        objectRigidBody = GetComponent<Rigidbody>();
    }
    public void Grab(Transform objectGrabPointTransform)
    {
        this.objectGrabPointTransform = objectGrabPointTransform;
        //objectRigidBody.useGravity = false;
        objectRigidBody.isKinematic = true;
    }
    
    public void Drop()
    {
        this.objectGrabPointTransform = null;
        //objectRigidBody.useGravity = true;
        objectRigidBody.isKinematic = false;
    }

    private void FixedUpdate()
    {
        if (objectGrabPointTransform != null)
        {
            Vector3 newPosition = Vector3.Lerp(transform.position, objectGrabPointTransform.position, Time.deltaTime * lerpSpeed);
            objectRigidBody.MovePosition(newPosition);
        }
    }
}
