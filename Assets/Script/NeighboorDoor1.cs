using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeighboorDoor1 : MonoBehaviour, IInteractable
{
    public string textInteraction;
    public string textCantInteract;
    public bool canTake = true;
    private int ActualNumber = 0;
    [SerializeField] private Candy candy;
    [SerializeField] private PlayerPickUp playerPickUp;
    [SerializeField] private PaperCandy textCandy;
    [SerializeField] private PaperParcFell textFall;
    [SerializeField] private PaperChillBeer textChill;
    
    private FMOD.Studio.EventInstance event_fmod_littleToc;
    private FMOD.Studio.EventInstance event_fmod_hardToc;

    private void Start()
    {
        event_fmod_littleToc = FMODUnity.RuntimeManager.CreateInstance("event:/Hall/DoorToc");
        event_fmod_hardToc = FMODUnity.RuntimeManager.CreateInstance("event:/Hall/BigDoorToc");
        textCandy.GetComponent<Renderer>().enabled = false;
        textFall.GetComponent<Renderer>().enabled = false;
        textChill.GetComponent<Renderer>().enabled = false;
        
        textCandy.GetComponent<MeshCollider>().enabled = false;
        textFall.GetComponent<MeshCollider>().enabled = false;
        textChill.GetComponent<MeshCollider>().enabled = false;
    }
    
    private void Update()
    {
        event_fmod_littleToc.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject)); 
        event_fmod_hardToc.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject)); 
    }

    public void Interact(PlayerPickUp interactor)
    {
        Debug.Log("NeighboorDoor1 interacted");
        switch (ActualNumber)
        {
            case 0:
                ActualNumber++;
                playerPickUp.door1number = 1;
                StopTocLittle();
                textCandy.GetComponent<Renderer>().enabled = true;
                textCandy.GetComponent<MeshCollider>().enabled = true;
                //textCandy.PlayAnimation
                candy.canTake = true;
                this.canTake = false;

                textCantInteract = "";
                textInteraction = "";
                break;
            case 1:
                ActualNumber++;
                playerPickUp.door1number = 2;
                canTake = false;
                candy.OnCandyGive();
                textFall.GetComponent<Renderer>().enabled = true;
                textFall.GetComponent<MeshCollider>().enabled = true;
                //textFall.PlayAnimation
                textCantInteract = "";
                textInteraction = "";
                break;
            case 2:
                ActualNumber++;
                playerPickUp.door1number = 3;
                canTake = false;
                StopTocHard();
                textChill.GetComponent<Renderer>().enabled = true;
                textChill.GetComponent<MeshCollider>().enabled = true;
                //textChill.PlayAnimation
                textCantInteract = "";
                textInteraction = "";
                break;
            case 3 :
                break;
        }
    }
    
    public void SetCanTake(bool canTake)
    {
        this.canTake = canTake;
    }
    
    public bool GetCanTake()
    {
        //return canTake;
        switch (ActualNumber)
        {
            case 0:
                return true;
            case 1:
                return playerPickUp.bHasCandy;
            case 2:
                return canTake;
            case 3:
                return playerPickUp.bHasKey;
            default:
                return true;
        }
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

    public void TocLittle()
    {
        Debug.Log("Toc little Start");
        event_fmod_littleToc.start(); 
    }
    
    public void TocHard()
    {
        event_fmod_hardToc.start();
    }

    public void StopTocLittle()
    {
        event_fmod_littleToc.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }
    
    public void StopTocHard()
    {
        event_fmod_hardToc.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }
}
