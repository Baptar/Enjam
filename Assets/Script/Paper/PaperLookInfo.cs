using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaperLookInfo : MonoBehaviour, IInteractable
{
    [SerializeField] public string paperText;
    [SerializeField] public string textInteraction;
    [SerializeField] public string textCantInteract;
    [SerializeField] public string doorTextInteraction;
    [SerializeField] public string doorTextCantInteract;
    [SerializeField] private Animator paperAnimation;
    [SerializeField] private NeighboorDoor2 door2;
    public bool canTake = true;
    
    public void Interact(PlayerPickUp interactor)
    {
        //FMODUnity.RuntimeManager.PlayOneShot("event:/Hall/PaperGrab");
        door2.textCantInteract = doorTextCantInteract;
        door2.textInteraction = doorTextInteraction;
            
        gameObject.SetActive(false);
        interactor.bIsReading = true;
        interactor.isDoor1 = false;
        interactor.SetPaperText(paperText);
        interactor.paperCanvas.gameObject.SetActive(true);
    }
    
    public string GetTextCantInteract()
    {
        return textCantInteract;
    }
    
    public void PaperAnimation()
    {
        paperAnimation.Play("papierPorte2", 0, 0.0f);
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
