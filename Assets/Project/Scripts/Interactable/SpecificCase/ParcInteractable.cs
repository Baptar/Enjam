using UnityEngine;

public class ParcInteractable : ZoneInteractable
{
    [Space(10)]
    [Header("DEBUG")]
    [SerializeField] private bool bGaveJuda;
    
    private void Start()
    {
        SetTextInteract("Talk to hole");
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
        Debug.Log("TalkToParc");

        bGaveJuda = true;
        SetTextInteract("Throw radio");
        MainManager.instance.JudasManager.GetJudasObjectGrabbable().gameObject.SetActive(true);
        MainManager.instance.JudasManager.GetJudasObjectGrabbable().Interact();
    }

    private void ThrowRadio()
    {
        Debug.Log("Throw Radio");
        MainManager.instance.Player.Drop();
        bInInteractionZone = false;
        enabled = false;
    }

    private bool GetHasJuda() => MainManager.instance.Player.GetHasJuda();
}
