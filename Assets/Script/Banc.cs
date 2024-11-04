using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Banc : MonoBehaviour, IInteractable
{
    public PlayerPickUp player;
    public bool canTake = true;
    public string textInteraction;
    public string textCantInteract;
    [SerializeField] private Transform playerNewTransform;
    [SerializeField] private float timeBetweenChangeVision = 1f;
    private void OnTriggerEnter(Collider other)
    {
        if (TryGetComponent(out PlayerPickUp playerPickUp)) playerPickUp.bInBancZone = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (TryGetComponent(out PlayerPickUp playerPickUp)) playerPickUp.bInBancZone = false;
    }
    
    
    public void Interact(PlayerPickUp interactor)
    {
        canTake = false;
        Debug.Log("Interact with banc");
        interactor.MoveVision(playerNewTransform, timeBetweenChangeVision);
    }
    
    public void SetCanTake(bool canTake)
    {
        this.canTake = canTake;
    }
    
    public bool GetCanTake()
    {
        return player.bInBancZone && player.bHasBeer && canTake;
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
