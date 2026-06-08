using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParcInteractable : ZoneInteractable
{
    [SerializeField] private ObjectGrabbable judaGrabbable;
    
    private void Start()
    {
        SetTextInteract("Talk to hole");
    }
    
    public override bool GetInteractable()
    {
        return GetHasJuda() ? 
            bInteractable && MainManager.instance.Player.GetHasRadio() && bInInteractionZone : 
            bInteractable;
    }

    public override void Interact()
    {
        if (GetInteractable())
        {
            SetInteractable(false);
            MainManager.instance.Player.SetIsInInteractionZone(false);
            
            if (!eventSoundOnInteract.IsNull) PlaySound(eventSoundOnInteract);
            eventOnInteract?.Invoke();

            if (!GetHasJuda()) TalkToParc();
            else ThrowRadio();
        }
        else eventOnInteractButNotInteractable?.Invoke();
    }

    private void TalkToParc()
    {
        Debug.Log("TalkToParc");
        SetTextInteract("Throw radio");
        // TODO : dialog with parc and then call function MainManager.instance.Player.SetHasJuda(true)
        
        judaGrabbable.gameObject.SetActive(true);
        judaGrabbable.Interact();
    }

    private void ThrowRadio()
    {
        Debug.Log("Throw Radio");
    }

    private bool GetHasJuda()
    {
        return MainManager.instance.Player.GetHasJuda();
    }
}
