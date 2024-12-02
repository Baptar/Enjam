using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerCouloirCollision : MonoBehaviour
{
    [SerializeField] private GameObject[] couloirWall;
    private bool isColliding;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isColliding = true;
            foreach (GameObject wall in couloirWall)
            {
                wall.GetComponent<MeshCollider>().enabled = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isColliding = false;
            foreach (GameObject wall in couloirWall)
            {
                wall.GetComponent<MeshCollider>().enabled = false;
            }
        }
    }

    private void Update()
    {
        Debug.Log(isColliding);
    }
}
