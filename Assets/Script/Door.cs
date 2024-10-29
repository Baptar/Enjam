using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject start;
    [SerializeField] private GameObject end;
    public void Interact(PlayerPickUp interactor)
    {
        Debug.Log("Door interacted");
        if (interactor.bHasKey)
        {
            // Get All Child of Start
            for (int i = 0; i < start.transform.childCount; i++)
            {
                GameObject child = start.transform.GetChild(i).gameObject;
                // remove render
                if (child.GetComponent<Renderer>())
                    child.GetComponent<Renderer>().enabled = false;
                // remove collision
                if (child.GetComponent<BoxCollider>())
                    child.GetComponent<BoxCollider>().enabled = false;
                // remove lights
                if (child.GetComponent<Light>())
                    child.GetComponent<Light>().enabled = false;
                
                // Do Same on this Child (just one level)
                for (int j = 0; j < child.transform.childCount; j++)
                {
                    GameObject childBis = child.transform.GetChild(j).gameObject;
                    // remove render
                    if (childBis.GetComponent<Renderer>())
                        childBis.GetComponent<Renderer>().enabled = false;
                    // remove collision
                    if (childBis.GetComponent<BoxCollider>())
                        childBis.GetComponent<BoxCollider>().enabled = false;
                    // remove lights
                    if (childBis.GetComponent<Light>())
                        childBis.GetComponent<Light>().enabled = false;
                }
            }
            
            // Get All Child of End
            for (int i = 0; i < end.transform.childCount; i++)
            {
                GameObject child = end.transform.GetChild(i).gameObject;
                // add render
                if (child.GetComponent<Renderer>())
                    child.GetComponent<Renderer>().enabled = true;
                // add collision
                if (child.GetComponent<BoxCollider>())
                    child.GetComponent<BoxCollider>().enabled = true;
                // add lights 
                if (child.GetComponent<Light>())
                    child.GetComponent<Light>().enabled = true;
                
                // Do Same on this Child (just one level)
                for (int j = 0; j < child.transform.childCount; j++)
                {
                    GameObject childBis = child.transform.GetChild(j).gameObject;
                    // add render
                    if (childBis.GetComponent<Renderer>())
                        childBis.GetComponent<Renderer>().enabled = true;
                    // add collision
                    if (childBis.GetComponent<BoxCollider>())
                        childBis.GetComponent<BoxCollider>().enabled = true;
                    // add lights
                    if (childBis.GetComponent<Light>())
                        childBis.GetComponent<Light>().enabled = true;
                }
            }
        }
    }
}
