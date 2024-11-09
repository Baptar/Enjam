using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaperChillBeer : MonoBehaviour, IInteractable
{
    [SerializeField] public string paperText;
    [SerializeField] public string textInteraction;
    [SerializeField] public string textCantInteract;
    [SerializeField] public string doorTextInteraction;
    [SerializeField] public string doorTextCantInteract;
    [SerializeField] private NeighboorDoor1 door1;
    [SerializeField] public NeighboorDoor2 door2;
    [SerializeField] public ObjectGrabbable beerToGrab;
    [SerializeField] private Animator paperAnimation;
    public bool canTake = true;
   
    public void Interact(PlayerPickUp interactor)
    {
        //FMODUnity.RuntimeManager.PlayOneShot("event:/Hall/PaperGrab");
        beerToGrab.gameObject.SetActive(true);
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

    public void OnStopRead(PlayerPickUp playerPickUp)
    {
        Debug.Log("OnStopRead from base");
        door2.TocLittle();
        door2.canTake = true;

        playerPickUp.bHasBeer = true;
        playerPickUp.bHasGrabbleObject = true;
        beerToGrab.Grab(playerPickUp.objectGrabPointTransform);
        beerToGrab.OnTook(playerPickUp);
        //FMODUnity.RuntimeManager.PlayOneShot("event:/Hall/BereGrab");
    }
}
