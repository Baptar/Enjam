using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeighboorDoor2 : MonoBehaviour, IInteractable
{
    public string textInteraction;
    public string textCantInteract;
    public bool canTake = true;
    private int actualNumber = 0;
    
    public void Interact(PlayerPickUp interactor)
    {
        Debug.Log("NeighboorDoor2 interacted");
        switch (actualNumber)
        {
            
        }
    }

    public void SetCanTake(bool canTake)
    {
        this.canTake = canTake;
    }
    
    public bool GetCanTake()
    {
        return canTake;
    }

    public string GetTextInteract()
    {
        if (textInteraction == "") return "Press E to Interact";
        return textInteraction;
    }
    
    public string GetTextCantInteract()
    {
        return textCantInteract;
    }
}
