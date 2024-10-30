using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandyTrigger : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other != null);
        {
            if (other.gameObject.TryGetComponent(out Candy candy))
            {
               candy.OnCandyGive();
            }
        }
    }
}
