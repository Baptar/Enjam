using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class PaperBase : MonoBehaviour, IInteractable
{
    [SerializeField] private NeighboorDoor1 door1;
    [SerializeField] private NeighboorDoor2 door2;
    [SerializeField] private bool isDoor1;
    [SerializeField] private string paperText;
    [SerializeField] public string textInteraction;
    [SerializeField] public string textCantInteract;
    
    [SerializeField] public string papertextInteraction;
    [SerializeField] public string papertextCantInteract;
    public bool canTake = true;
    
    public void Interact(PlayerPickUp interactor)
    {
        if (isDoor1)
        {
            door1.textCantInteract = papertextCantInteract;
            door1.textInteraction = papertextInteraction;
        }
        else
        {
            door2.textCantInteract = papertextCantInteract;
            door2.textInteraction = papertextInteraction;
        }
        interactor.bIsReading = true;
        interactor.SetPaperText(paperText);
        interactor.paperCanvas.gameObject.SetActive(true);
        Destroy(gameObject);
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
}
