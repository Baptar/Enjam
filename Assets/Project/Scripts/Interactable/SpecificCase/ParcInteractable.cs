using UnityEngine;
using UnityEngine.Localization;

public class ParcInteractable : ZoneInteractable
{
    [SerializeField] private LocalizedString judasLocalizedString;
    [SerializeField] private LocalizedString radioLocalizedString;
    
    [Space(10)]
    [Header("DEBUG")]
    [SerializeField] private bool bGaveJuda;
    
    private void Start()
    {
        SetTextInteract(judasLocalizedString.GetLocalizedString());
    }

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        
        if (GetInteractable() && !bGaveJuda)
        {
            TalkToParc();
        }
    }
    
    public override bool GetInteractable()
    {
        return bGaveJuda ? 
            bInteractable && MainManager.instance.Player.GetHasRadio() && bInInteractionZone : 
            bInteractable && bInInteractionZone;
    }

    public override void Interact()
    {
        if (GetInteractable())
        {
            SetInteractable(false);
            MainManager.instance.Player.SetIsInInteractionZone(false);
            
            if (!eventSoundOnInteract.IsNull) PlaySound(eventSoundOnInteract);
            eventOnInteract?.Invoke();

            if (!bGaveJuda) TalkToParc();
            else ThrowRadio();
        }
        else eventOnInteractButNotInteractable?.Invoke();
    }

    private void TalkToParc()
    {
        Debug.LogError("TalkToParc");
        
        bGaveJuda = true;
        SetTextInteract(radioLocalizedString.GetLocalizedString());
        MainManager.instance.JudasesManager.GenerateJudas();
        gameObject.SetActive(false);
        //MainManager.instance.JudasesManager.GetJudasObjectGrabbable().gameObject.SetActive(true);
        //MainManager.instance.JudasesManager.GetJudasObjectGrabbable().Interact();
    }

    private void ThrowRadio()
    {
        Debug.Log("Throw Radio");
        MainManager.instance.Player.Drop();
        bInInteractionZone = false;
        
        enabled = false;
        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        MainManager.instance.Player.bInInteractionZone = false;
    }

    private bool GetHasJuda() => MainManager.instance.Player.GetHasJuda();
}
