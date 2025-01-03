using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NeighboorDoor2 : MonoBehaviour, IInteractable
{
    public string textInteraction;
    [SerializeField] private PlayerPickUp playerPickUp;
    public string textCantInteract;
    public bool canTake = false;
    private int ActualNumber = 0;
    [SerializeField] private PaperIndice paperIndice;
    [SerializeField] private PaperLookInfo paperLookInfo;
    
    private FMOD.Studio.EventInstance event_fmod_littleToc;
    private FMOD.Studio.EventInstance event_fmod_hardToc;
    
    private void Start()
    {
        event_fmod_littleToc = FMODUnity.RuntimeManager.CreateInstance("event:/Hall/DoorToc");
        event_fmod_hardToc = FMODUnity.RuntimeManager.CreateInstance("event:/Hall/BigDoorToc");
        paperIndice.GetComponent<Renderer>().enabled = false;
        paperLookInfo.GetComponent<Renderer>().enabled = false;
        
        paperIndice.GetComponent<MeshCollider>().enabled = false;
        paperLookInfo.GetComponent<MeshCollider>().enabled = false;
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
                playerPickUp.door2Number = 1;
                StopTocLittle();
                paperLookInfo.GetComponent<Renderer>().enabled = true;
                paperLookInfo.GetComponent<MeshCollider>().enabled = true;
                paperLookInfo.PaperAnimation();
                //paperLookInfo.PlayAnimation
                PaperSound();
                this.canTake = false;

                textCantInteract = "";
                textInteraction = "";
                break;
            case 1:
                ActualNumber++;
                playerPickUp.door2Number = 2;
                StopTocLittle();
                canTake = false;
                paperIndice.GetComponent<Renderer>().enabled = true;
                paperIndice.GetComponent<MeshCollider>().enabled = true;
                paperLookInfo.GetComponent<Renderer>().enabled = false;
                paperLookInfo.GetComponent<MeshCollider>().enabled = false;
                paperIndice.PaperAnimation();
                //paperIndice.PlayAnimation
                PaperSound();
                textCantInteract = "";
                textInteraction = "";
                break;
            case 2:
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
    
    private void PaperSound()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/Hall/PaperAppear");
    }
    
    public void TocLittle()
    {
        Debug.Log("Toc little Start");
        FMODUnity.RuntimeManager.PlayOneShot("event:/Hall/DoorToc2ActiveTrig");
    }
    
    public void TocHard()
    {
        event_fmod_hardToc.start();
    }

    public void StopTocLittle()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/Hall/DoorToc2NoneTrig");
    }
    
    public void StopTocHard()
    {
        event_fmod_hardToc.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }
}
