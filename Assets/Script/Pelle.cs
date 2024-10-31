using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pelle : ObjectGrabbable
{
    public int cpt = 0;

    public void Detruit()
    {
        Destroy(this.gameObject);
    }
}
