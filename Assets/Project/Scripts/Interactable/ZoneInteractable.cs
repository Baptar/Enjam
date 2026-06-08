using UnityEngine;

public class ZoneInteractable : ObjectInteractable
{
    protected bool bInInteractionZone = false;
    
    private void OnTriggerEnter(Collider other)
    {
        if (!GetInteractable()) return;
        if (!other.CompareTag("Player")) return;

        bInInteractionZone = true;
        MainManager.instance.Player.SetIsInInteractionZone(true);
        MainManager.instance.Player.SetObjectInteractable(this);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!GetInteractable()) return;
        if (!other.CompareTag("Player")) return;
        
        bInInteractionZone = false;
        MainManager.instance.Player.SetIsInInteractionZone(false);
        MainManager.instance.Player.SetObjectInteractable(null);
    }
    
    public override void Interact()
    {
        if (GetInteractable())
        {
            SetInteractable(false);
            MainManager.instance.Player.SetIsInInteractionZone(false);
            
            if (!eventSoundOnInteract.IsNull) PlaySound(eventSoundOnInteract);
            eventOnInteract?.Invoke();
        }
        else eventOnInteractButNotInteractable?.Invoke();
    }
}
