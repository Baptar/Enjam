using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerCouloirCollision : MonoBehaviour
{
    [SerializeField] private GameObject[] couloirWall;
    
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnTriggerEnter");
        if (!other.CompareTag("Player")) return;
        Debug.Log("AFTER TRIGGER ENTER");
        
        foreach (GameObject wall in couloirWall)
        {
            wall.GetComponent<Collider>().enabled = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        
        foreach (GameObject wall in couloirWall)
        {
            wall.GetComponent<Collider>().enabled = false;
        }
    }
}
