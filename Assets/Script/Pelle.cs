using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pelle : ObjectGrabbable
{
    public int cpt = 0;
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
            transform.rotation = playerPickUp.transform.rotation;
        }
    }

    public void PelleAccessible()
    {
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        startRotation = true;
    }
}
