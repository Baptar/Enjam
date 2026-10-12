using UnityEngine;
using UnityEngine.Localization;

public class RadioThrowZone : ZoneInteractable
{
    public override bool GetInteractable()
    {
        return bInteractable && bInInteractionZone;
    }

    public override void Interact()
    {
        if (GetInteractable())
        {
            SetInteractable(false);
            MainManager.instance.Player.SetIsInInteractionZone(false);
            
            if (!eventSoundOnInteract.IsNull) PlaySound(eventSoundOnInteract);
            eventOnInteract?.Invoke();
            
            ThrowRadio();
        }
        else eventOnInteractButNotInteractable?.Invoke();
    }

    private void ThrowRadio()
    {
        MainManager.instance.Player.Drop();
        bInInteractionZone = false;
        
        enabled = false;
        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        MainManager.instance.Player.bInInteractionZone = false;
    }
}
