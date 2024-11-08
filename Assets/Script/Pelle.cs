using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mail;
using UnityEngine;

public class Pelle : ObjectGrabbable
{
    public int cpt = 0;
    [SerializeField] private Camera playerCamera;
    public bool startRotation;
    public void Detruit()
    {
        playerPickUp.bHasGrabbleObject = false;
        Destroy(this.gameObject);
    }

    private void Update()
    {
        if (startRotation)
        {
            //transform.rotation = Quaternion.Euler(-50, 0, 0);
            //startRotation = false;
        }
    }

    public void PelleAccessible()
    {
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        startRotation = true;
    }
}
