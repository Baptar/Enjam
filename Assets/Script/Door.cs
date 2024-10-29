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
            for (int i = 0; i < start.transform.childCount; i++)
            {
                GameObject child = start.transform.GetChild(i).gameObject;
                //Do something with child
                child.GetComponent<Renderer>().enabled = false;
            }
            
            for (int i = 0; i < end.transform.childCount; i++)
            {
                GameObject child = end.transform.GetChild(i).gameObject;
                //Do something with child
                child.GetComponent<Renderer>().enabled = true;
            }
            
            //Destroy(gameObject);
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
