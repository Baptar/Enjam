using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorCoridorInteract : ObjectInteractable
{
    [Serializable]
    public enum EDoorEvent
    {
        None, 
        Start,
        Candy, 
        ParcFell,
        ChillBeer,
        WatchTV,
        UnderstandParc
    }
    
    
    private FMOD.Studio.EventInstance event_fmod_littleToc;
    private FMOD.Studio.EventInstance event_fmod_hardToc;
    
    [Header("Debug")]
    public EDoorEvent currentDoorEvent = EDoorEvent.None;
    
    private void Start()
    {
        event_fmod_littleToc = FMODUnity.RuntimeManager.CreateInstance("event:/Hall/DoorToc");
        event_fmod_hardToc = FMODUnity.RuntimeManager.CreateInstance("event:/Hall/BigDoorToc");
    }
    
    private void Update()
    {
        event_fmod_littleToc.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject)); 
        event_fmod_hardToc.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject)); 
    }

    public override void Interact()
    {
        if (GetInteractable())
        {
            eventOnInteract?.Invoke();
            SetInteractable(false);
            
            switch (currentDoorEvent)
            {
                case EDoorEvent.Start:
                    MainManager.instance.AudioManager.StopSoundTocLittle(transform);
                    MainManager.instance.PaperManager.AppearPaperCandy();
                    break;
            
                case EDoorEvent.Candy:
                    MainManager.instance.Player.Drop();
                    MainManager.instance.PaperManager.AppearPaperYellAtParc();
                    break;
            
                case EDoorEvent.ParcFell:
                    MainManager.instance.AudioManager.StopSoundTocHard(transform);
                    MainManager.instance.PaperManager.AppearPaperChillBeer();
                    break;
            
                // never Called
                case EDoorEvent.ChillBeer:
                    break;
            
                case EDoorEvent.WatchTV:
                    MainManager.instance.AudioManager.StopSoundTocLittle(transform);
                    MainManager.instance.PaperManager.AppearPaperWatchTV();
                    break;
                
                case EDoorEvent.UnderstandParc:
                    MainManager.instance.AudioManager.StopSoundTocLittle(transform);
                    MainManager.instance.PaperManager.AppearPaperUnderstandParc();
                    break;
            }
            
            SetCurrentDoorEvent(EDoorEvent.None);
        }
        else
        {
            eventOnInteractButNotInteractable?.Invoke();
                
            switch (currentDoorEvent)
            {
                case EDoorEvent.None:
                    break;
                
                case EDoorEvent.Start:
                    break;
            
                case EDoorEvent.Candy:
                    break;
            
                case EDoorEvent.ParcFell:
                    break;
            
                case EDoorEvent.ChillBeer:
                    break;
            
                case EDoorEvent.WatchTV:
                    break;
                
                case EDoorEvent.UnderstandParc:
                    break;
            }
        }
    }

    private void SetCurrentDoorEvent(EDoorEvent newDoorEvent)
    {
        currentDoorEvent = newDoorEvent;
        string newTextInteract = "";
        string newTextCantInteract = "";
        bool newIsInteractable = false;
        
        switch (newDoorEvent)
        {
            case EDoorEvent.Start:
                MainManager.instance.AudioManager.PlayerSoundTocLittle(transform);
                newTextInteract = "Answer";
                newTextCantInteract = "";
                newIsInteractable = true;
                break;
            
            case EDoorEvent.Candy:
                newTextInteract = "Give Candies";
                newTextCantInteract = "Search Candies";
                newIsInteractable = false;
                break;
            
            case EDoorEvent.ParcFell:
                newTextInteract = "Answer";
                newTextCantInteract = "Scream in the fall";
                newIsInteractable = false;
                break;
            
            case EDoorEvent.ChillBeer:
                newTextInteract = "Answer";
                newTextCantInteract = "Chill Dude";
                newIsInteractable = false;
                break;
            
            case EDoorEvent.WatchTV:
                MainManager.instance.AudioManager.PlayerSoundTocLittle(transform);
                newTextInteract = "Answer";
                newTextCantInteract = "Find a way to watch TV";
                newIsInteractable = true;
                break;
            
            case EDoorEvent.UnderstandParc:
                newTextInteract = "Answer";
                newTextCantInteract = "";
                newIsInteractable = true;
                break;
        }
        
        SetTextInteract(newTextInteract);
        SetTextCantInteract(newTextCantInteract);
        SetInteractable(newIsInteractable);
    }
    

    public void SetNoneEvent() => SetCurrentDoorEvent(EDoorEvent.None);
    public void SetStartEvent() => SetCurrentDoorEvent(EDoorEvent.Start);
    public void SetCandyEvent() => SetCurrentDoorEvent(EDoorEvent.Candy);
    public void SetParcFellEvent() => SetCurrentDoorEvent(EDoorEvent.ParcFell);
    public void SetChillBeerEvent() => SetCurrentDoorEvent(EDoorEvent.ChillBeer);
    public void SetWatchTVEvent() => SetCurrentDoorEvent(EDoorEvent.WatchTV);
    public void SetUnderstandParc() => SetCurrentDoorEvent(EDoorEvent.UnderstandParc);
}
