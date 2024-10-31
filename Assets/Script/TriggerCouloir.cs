using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerCouloir : MonoBehaviour
{
    private bool available = true;
    
    [SerializeField] private NeighboorDoor1 neighboorDoor1;
    void OnTriggerEnter(Collider other)
    {
        if (available)
        {
            if (other != null);
            {
                if (other.gameObject.TryGetComponent(out PlayerPickUp player))
                {
                    available = false;
                    neighboorDoor1.TocLittle();   
                }
            }
        }
    }
}
