using UnityEngine;

public class ZoneInteractable : ObjectInteractable
{
    protected bool bInInteractionZone = false;
    
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        bInInteractionZone = true;
        MainManager.instance.Player.SetIsInInteractionZone(true);
        
        if (!GetInteractable()) return;
        MainManager.instance.Player.SetObjectInteractable(this);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        bInInteractionZone = false;
        MainManager.instance.Player.SetIsInInteractionZone(false);
        
        if (!GetInteractable()) return;
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
