using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class DoorCoridorInteract : ObjectInteractable
{
    [Serializable]
    public enum EDoorEvent
    {
        None,           
        Start,          // paper :                |  interact : interact       |  cantInteract : 
        Candy,          // paper : buy me candy   |  interact : give candies   |  cantInteract : search candies
        ParcFell,       // paper : parc fell      |  interact :                |  cantInteract : go see hole
        BeforeJudas,    // paper : outside        |  interact :                |  cantInteract : 
        Judas,          // paper :                |  interact : regarder juda  |  cantInteract : 
        ChillBeer,      // paper : buy me candy   |  interact : give candies   |  cantInteract : search candies
        UnderstandParc  // paper : buy me candy   |  interact : give candies   |  cantInteract : search candies
    }
    
    [Header("Judas")]
    [SerializeField] private string judasSceneName;
    [SerializeField] private Transform judasWorldPosition;
    [SerializeField] private Transform cameraJudaTarget;
    [SerializeField] private float judasCamFOV = 1;
    public PaperInteract paperAfterJuda;
    public PaperInteract paperUnderstandParc;
    
    [Space(5)]
    [Header("Toc")]
    [SerializeField] private GameObject goShakeDoor;
    [SerializeField] private Vector3 knockLocalAxis = Vector3.forward;
    [SerializeField] private int vibrato = 6;
    [SerializeField] private float elasticity = 0.4f;
    [Header("Little")]
    [SerializeField] private float littleStrength = 0.01f;
    [SerializeField] private float littleDuration = 0.18f;
    [Header("Big")]
    [SerializeField] private float bigStrength = 0.04f;
    [SerializeField] private float bigDuration = 0.35f;
    [Header("Random")]
    [SerializeField, Range(0f, 0.5f)] private float strengthVariation = 0.25f;
    [SerializeField, Range(0f, 0.5f)] private float durationVariation = 0.15f;
    [SerializeField] private float axisAngleJitter = 6f;
    [SerializeField] private float rotationStrength = 0.6f;
    private Vector3 restLocalPos;
    private Quaternion restLocalRot;
    private float lastFactor = 1f;
    
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
    [SerializeField] private LocalizedString throwRadioCantInteract;
    
    
    [Space(10)]
    [Header("Debug")]
    public EDoorEvent currentDoorEvent = EDoorEvent.None;
    
    private FMOD.Studio.EventInstance event_fmod_littleToc;
    private FMOD.Studio.EventInstance event_fmod_hardToc;
    
    private Sequence sequenceToc;
    private float durationToc = 0.0f;
    
    private void Awake()
    {
        var t = goShakeDoor.transform;
        restLocalPos = t.localPosition;
        restLocalRot = t.localRotation;
    }
    
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
    
    void OnEnable()  => KnockMarkerRelay.OnMarker += HandleMarker;
    void OnDisable() => KnockMarkerRelay.OnMarker -= HandleMarker;

    void HandleMarker(Transform source, string marker)
    {
        if (source != gameObject.transform) return;

        if (marker == "Knock")
        {
            Debug.Log("Knock : " + source);
            ShakeDoor();
        }
        else if (marker == "BigKnock")
        {
            Debug.Log("BigKnock : " + source);
            ShakeBigDoor();
        }
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
                    break;
            
                
                case EDoorEvent.ChillBeer:
                    MainManager.instance.AudioManager.StopSoundTocHard(transform);
                    MainManager.instance.PaperManager.AppearPaperChillBeer();
                    break;
                
                case EDoorEvent.UnderstandParc:
                    MainManager.instance.AudioManager.StopSoundTocLittle(transform);
                    MakePaperUnderstandParcAppear();
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
                MainManager.instance.AudioManager.PlayerSoundTocLittleDoor(gameObject.transform);
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
                MainManager.instance.AudioManager.PlayerSoundTocLittleDoor(gameObject.transform);
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
                MainManager.instance.AudioManager.PlayerSoundTocHard(gameObject.transform);
                newTextInteract = chillBeer.GetLocalizedString();
                newTextCantInteract = chillBeerCantInteract.GetLocalizedString();
                newIsInteractable = true;
                break;
            
            case EDoorEvent.UnderstandParc:
                MainManager.instance.AudioManager.PlayerSoundTocLittleDoor(gameObject.transform);
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
        MainManager.instance.JudasesManager.OnInteractJudas(
            this, 
            camJudaTarget : cameraJudaTarget, 
            judasTransformTarget : judasWorldPosition, 
            fovCam : judasCamFOV);
    }

    [ContextMenu("Shake Little Door")]
    public void ShakeDoor() => Knock(littleStrength, littleDuration);
    
    [ContextMenu("Shake Big Door")]
    public void ShakeBigDoor() => Knock(bigStrength, bigDuration);
    
    private void Knock(float strength, float duration)
    {
        var t = goShakeDoor.transform;

        t.DOKill();
        t.localPosition = restLocalPos;
        t.localRotation = restLocalRot;

        float factor;
        int tries = 0;
        do
        {
            factor = 1f + Random.Range(-strengthVariation, strengthVariation);
            tries++;
        } while (Mathf.Abs(factor - lastFactor) < strengthVariation * 0.4f && tries < 5);
        lastFactor = factor;

        float s = strength * factor;
        float d = duration * (1f + Random.Range(-durationVariation, durationVariation));

        Vector3 dir = Quaternion.Euler(
            Random.Range(-axisAngleJitter, axisAngleJitter),
            Random.Range(-axisAngleJitter, axisAngleJitter),
            0f) * knockLocalAxis.normalized;

        Vector3 target = restLocalPos + dir * s;

        Vector3 rot = new Vector3(
            Random.Range(-1f, 1f),
            Random.Range(-1f, 1f),
            Random.Range(-1f, 1f)) * rotationStrength * factor;
        Quaternion rotTarget = restLocalRot * Quaternion.Euler(rot);

        DOTween.Sequence()
            .Append(t.DOLocalMove(target, d * 0.25f).SetEase(Ease.OutQuad))
            .Join(t.DOLocalRotateQuaternion(rotTarget, d * 0.25f).SetEase(Ease.OutQuad))
            .Append(t.DOLocalMove(restLocalPos, d * 0.75f).SetEase(Ease.OutElastic, 1.2f, 0.4f))
            .Join(t.DOLocalRotateQuaternion(restLocalRot, d * 0.75f).SetEase(Ease.OutElastic, 1.2f, 0.4f))
            .SetLink(goShakeDoor);
    }

    public void SetJudasSceneName(string text) => judasSceneName = text; 
    public string GetJudasSceneName() => judasSceneName; 

    public void SetRadioDoor() => SetJudasSceneName("JudasRadio");

    public void MakePaperThrowRadioAppear()
    {
        paperAfterJuda.MakePaperAppear();
    }

    public void SetTextThrowRadio()
    {
        SetTextCantInteract(throwRadioCantInteract.GetLocalizedString());  
    }
    public void MakePaperUnderstandParcAppear() => paperUnderstandParc.MakePaperAppear();

    public void CancelDoor()
    {
        MainManager.instance.AudioManager.StopSoundTocLittle(transform);
        SetNoneEvent();
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
