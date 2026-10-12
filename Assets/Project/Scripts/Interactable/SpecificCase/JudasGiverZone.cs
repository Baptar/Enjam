using UnityEngine;
using UnityEngine.Localization;

public class JudasGiverZone : ZoneInteractable
{
    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        
        if (GetInteractable())
        {
            Interact();
        }
    }
    
    public override bool GetInteractable()
    {
        return bInteractable && bInInteractionZone;
    }

    public override void Interact()
    {
        if (GetInteractable())
        {
            SetInteractable(false);
            
            if (!eventSoundOnInteract.IsNull) PlaySound(eventSoundOnInteract);
            eventOnInteract?.Invoke();
            GiveJudas();
        }
        else eventOnInteractButNotInteractable?.Invoke();
    }

    private void GiveJudas()
    {
        bInteractable = false;
        MainManager.instance.JudasesManager.GenerateJudas();
        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        MainManager.instance.Player.bInInteractionZone = false;
    }
}
