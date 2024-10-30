using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pile : ObjectGrabbable
{
    // Start is called before the first frame update
    void Start()
    {
        //canTake = false;
    }

    public void OnPileTaken()
    {
        playerPickUp.bHasGrabbleObject = false;
        Drop();
        Destroy(gameObject);
    }
}
