using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaperCandy : MonoBehaviour, IInteractable
{
    [SerializeField] private NeighboorDoor1 door1;
    [SerializeField] public string paperText;
    [SerializeField] public string textInteraction;
    [SerializeField] public string textCantInteract;
    [SerializeField] public string doorTextInteraction;
    [SerializeField] public string doorTextCantInteract;
    public bool canTake = true;
    
    [SerializeField] private Animator paperAnimation;
    
    public void Interact(PlayerPickUp interactor)
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/Hall/PaperGrab");
        door1.textCantInteract = doorTextCantInteract;
        door1.textInteraction = doorTextInteraction;
            
        gameObject.SetActive(false);
        interactor.bIsReading = true;
        interactor.isDoor1 = true;
        interactor.SetPaperText(paperText);
        interactor.paperCanvas.gameObject.SetActive(true);
    }

    public void PaperAnimation()
    {
        paperAnimation.Play("papierPorte2", 0, 0.0f);
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
    }
}
