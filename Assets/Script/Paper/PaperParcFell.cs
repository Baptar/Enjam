using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaperParcFell : MonoBehaviour, IInteractable
{
    [SerializeField] public string paperText;
    [SerializeField] public string textInteraction;
    [SerializeField] public string textCantInteract;
    [SerializeField] public string doorTextInteraction;
    [SerializeField] public string doorTextCantInteract;
    [SerializeField] private NeighboorDoor1 door1;
    [SerializeField] public NeighboorDoor2 door2;
    public bool canTake = true;
    
    public void Interact(PlayerPickUp interactor)
    {
        door1.textCantInteract = doorTextCantInteract;
        door1.textInteraction = doorTextInteraction;
            
        gameObject.SetActive(false);
        interactor.bIsReading = true;
        interactor.isDoor1 = true;
        interactor.SetPaperText(paperText);
        interactor.paperCanvas.gameObject.SetActive(true);
    }
    
    public string GetTextCantInteract()
    {
        return textCantInteract;
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
        if (textInteraction == "") return "Press E to Read";
        return textInteraction;
    }

    public void OnStopRead()
    {
        Debug.Log("OnStopRead from base");
        door2.TocLittle();
        door2.canTake = true;
    }
}
