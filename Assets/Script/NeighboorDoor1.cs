using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeighboorDoor1 : MonoBehaviour, IInteractable
{
    public string textInteraction;
    public string textCantInteract;
    public bool canTake = true;
    private int ActualNumber = 0;
    [SerializeField] private GameObject gameObject;
    public void Interact(PlayerPickUp interactor)
    {
        Debug.Log("NeighboorDoor1 interacted");
        switch (ActualNumber)
        {
            case 0:
                // Stop Toquement de porte
                // afficher ligne de dialogue "va chercher bonbon"
                ActualNumber++;
                // can now take candy
                if (gameObject.TryGetComponent(out ObjectGrabbable objectGrabbable)) objectGrabbable.canTake = true;
                this.canTake = false;
                break;
            case 1:
                // Stop Toquement de porte
                break;
            case 2:
                // Stop Toquement de porte
                ActualNumber++;
                break;
            case 3:
                // Stop Toquement de porte
                ActualNumber++;
                break;
            
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
