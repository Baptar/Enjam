using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Candy : ObjectGrabbable
{
    // Start is called before the first frame update
    void Start()
    {
        canTake = false;
    }

    public void OnCandyGive()
    {
        Destroy(gameObject);
    }
}
