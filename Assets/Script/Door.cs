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
                DisplayObject(start.transform.GetChild(i).gameObject, false);
            }
            
            // Get All Child of End
            for (int i = 0; i < end.transform.childCount; i++)
            {
                DisplayObject(end.transform.GetChild(i).gameObject, true);
            }
        }
    }

    private void DisplayObject(GameObject gameobject, bool bShow)
    {
        // remove render of gameobject
        if (gameobject.GetComponent<Renderer>())
            gameobject.GetComponent<Renderer>().enabled = bShow;
        // remove collision of gameobject
        if (gameobject.GetComponent<BoxCollider>())
            gameobject.GetComponent<BoxCollider>().enabled = bShow;
        // remove lights of gameobject
        if (gameobject.GetComponent<Light>())
            gameobject.GetComponent<Light>().enabled = bShow;
            
        // do same thing for childs
        for (int i = 0; i < gameobject.transform.childCount; i++)
        {
            GameObject child = gameobject.transform.GetChild(i).gameObject;
            DisplayObject(child, bShow);
        }
    }
}
