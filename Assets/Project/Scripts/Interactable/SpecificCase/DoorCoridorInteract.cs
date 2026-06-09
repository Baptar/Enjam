using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Localization;

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
    [SerializeField] private Transform cameraJudaTarget;
    [SerializeField] private float judasCamFOV = 1;
    
    [Space(5)]
    [Header("Shake")]
    [SerializeField] private GameObject goShakeDoor;
    [SerializeField] private float shakeDuration = 0.5f;
    [SerializeField] private float shakeStrength = 1f;
    [SerializeField] private int shakeVibrato = 10;
    [SerializeField] private float shakeRandomness = 90f;
    
    [Space(5)]
    [Header("Localization")]
    [SerializeField] private LocalizedString start;
    [SerializeField] private LocalizedString candy;
    [SerializeField] private LocalizedString candyCantInteract;
    [SerializeField] private LocalizedString beforeJudas;
    [SerializeField] private LocalizedString judas;
    [SerializeField] private LocalizedString parkFell;
    [SerializeField] private LocalizedString parkFellCantInteract;
    [SerializeField] private LocalizedString chillBeer;
    [SerializeField] private LocalizedString chillBeerCantInteract;
    [SerializeField] private LocalizedString understandPark;
    
    
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

    public override bool GetInteractable()
    {
        if (currentDoorEvent != EDoorEvent.Judas) return bInteractable;
        return bInteractable && MainManager.instance.Player.GetHasJuda();
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
                newTextInteract = start.GetLocalizedString();
                newTextCantInteract = "";
                newIsInteractable = true;
                break;
            
            case EDoorEvent.Candy:
                newTextInteract = candy.GetLocalizedString();
                newTextCantInteract = candyCantInteract.GetLocalizedString();
                newIsInteractable = false;
                break;
            
            case EDoorEvent.BeforeJudas:
                MainManager.instance.AudioManager.PlayerSoundTocLittle(transform);
                newTextInteract = beforeJudas.GetLocalizedString();
                newTextCantInteract = "";
                newIsInteractable = true;
                break;
            
            case EDoorEvent.Judas:
                newTextInteract = judas.GetLocalizedString();
                newTextCantInteract = "";
                newIsInteractable = true;
                break;
            
            case EDoorEvent.ParcFell:
                newTextInteract = parkFell.GetLocalizedString();
                newTextCantInteract = parkFellCantInteract.GetLocalizedString();
                newIsInteractable = false;
                break;
            
            case EDoorEvent.ChillBeer:
                newTextInteract = chillBeer.GetLocalizedString();
                newTextCantInteract = chillBeerCantInteract.GetLocalizedString();
                newIsInteractable = false;
                break;
            
            case EDoorEvent.UnderstandParc:
                newTextInteract = understandPark.GetLocalizedString();
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
        MainManager.instance.JudasManager.OnInteractJudas(
            this, 
            camJudaTarget : cameraJudaTarget, 
            judasTransformTarget : judasWorldPosition, 
            judasSceneName : judasSceneName, 
            fovCam : judasCamFOV);
    }

    [ContextMenu("Shake Door")]
    public void ShakeDoor()
    {
        goShakeDoor.transform.DOShakePosition(
            shakeDuration, 
            shakeStrength,
            shakeVibrato,
            shakeRandomness);
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
