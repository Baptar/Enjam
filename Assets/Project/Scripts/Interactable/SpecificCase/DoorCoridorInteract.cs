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
        BeforeJudas,
        Judas,
        ParcFell,
        ChillBeer,
        UnderstandParc
    }
    
    [Header("Judas")]
    [SerializeField] private string judasSceneName;
    [SerializeField] private Transform judasWorldPosition;
    [SerializeField] private float judasCamFOV = 1;
    
    
    [Space(10)]
    [Header("Debug")]
    public EDoorEvent currentDoorEvent = EDoorEvent.None;
    
    private FMOD.Studio.EventInstance event_fmod_littleToc;
    private FMOD.Studio.EventInstance event_fmod_hardToc;
    
    
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
            
            bool nextStateInteractable = false;
            EDoorEvent nextDoorEvent = EDoorEvent.None;
            bool setNextDoorEvent = true;
            
            switch (currentDoorEvent)
            {
                case EDoorEvent.Start:
                    MainManager.instance.AudioManager.StopSoundTocLittle(transform);
                    MainManager.instance.PaperManager.AppearPaperCandy();
                    break;
            
                case EDoorEvent.Candy:
                    MainManager.instance.Player.Drop();
                    MainManager.instance.PaperManager.AppearPaperParcFell();
                    break;
                
                case EDoorEvent.BeforeJudas:
                    MainManager.instance.AudioManager.StopSoundTocLittle(transform);
                    MainManager.instance.PaperManager.AppearPaperJudas();
                    break;
                
                case EDoorEvent.Judas:
                    LookJudas();
                    setNextDoorEvent = false;
                    break;
            
                case EDoorEvent.ParcFell:
                    MainManager.instance.AudioManager.StopSoundTocHard(transform);
                    MainManager.instance.PaperManager.AppearPaperChillBeer();
                    break;
            
                // never Called
                case EDoorEvent.ChillBeer:
                    break;
                
                case EDoorEvent.UnderstandParc:
                    MainManager.instance.AudioManager.StopSoundTocLittle(transform);
                    MainManager.instance.PaperManager.AppearPaperUnderstandParc();
                    break;
            }
            
            SetInteractable(nextStateInteractable);
            if (setNextDoorEvent) SetCurrentDoorEvent(nextDoorEvent);
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
            
                case EDoorEvent.BeforeJudas:
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
            
            case EDoorEvent.BeforeJudas:
                MainManager.instance.AudioManager.PlayerSoundTocLittle(transform);
                newTextInteract = "Answer";
                newTextCantInteract = "";
                newIsInteractable = true;
                break;
            
            case EDoorEvent.Judas:
                newTextInteract = "Look through the peephole";
                newTextCantInteract = "";
                newIsInteractable = true;
                break;
            
            case EDoorEvent.ParcFell:
                newTextInteract = "Answer";
                newTextCantInteract = "";
                newIsInteractable = false;
                break;
            
            case EDoorEvent.ChillBeer:
                newTextInteract = "Answer";
                newTextCantInteract = "Chill Dude";
                newIsInteractable = false;
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

    private void LookJudas()
    {
        MainManager.instance.JudasManager.OnInteractJudas(this, judasTransformCam : judasWorldPosition, judasSceneName : judasSceneName, fovCam : judasCamFOV);
    }
    

    [ContextMenu("SetNoneEvent")]
    public void SetNoneEvent() => SetCurrentDoorEvent(EDoorEvent.None);
    [ContextMenu("SetStartEvent")]
    public void SetStartEvent() => SetCurrentDoorEvent(EDoorEvent.Start);
    [ContextMenu("SetCandyEvent")]
    public void SetCandyEvent() => SetCurrentDoorEvent(EDoorEvent.Candy);
    [ContextMenu("SetJudasEvent")]
    public void SetJudasEvent() => SetCurrentDoorEvent(EDoorEvent.Judas);
    [ContextMenu("SetParcFellEvent")]
    public void SetParcFellEvent() => SetCurrentDoorEvent(EDoorEvent.ParcFell);
    public void SetChillBeerEvent() => SetCurrentDoorEvent(EDoorEvent.ChillBeer);
    public void SetBeforeJudasEvent() => SetCurrentDoorEvent(EDoorEvent.BeforeJudas);
    public void SetUnderstandParc() => SetCurrentDoorEvent(EDoorEvent.UnderstandParc);
}
