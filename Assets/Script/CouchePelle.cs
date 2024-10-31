using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CouchePelle : MonoBehaviour, IInteractable
{
    [SerializeField] private PlayerPickUp playerPickUp;
    [SerializeField] private Pelle pelle;
    public bool canTake = true;
    public string textInteraction;
    public string textCantInteract;
    public void Interact(PlayerPickUp interactor)
    {
        pelle.cpt++;
        if (pelle.cpt == 3)
        {
            pelle.Detruit();
        }
        
        Destroy(gameObject);
    }
    
    public void SetCanTake(bool canTake)
    {
        this.canTake = canTake;
    }
    
    public bool GetCanTake()
    {
        return canTake && playerPickUp.bHasPelle;
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
