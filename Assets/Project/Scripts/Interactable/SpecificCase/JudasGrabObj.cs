public class JudasGrabObj : ObjectGrabbable
{
    protected virtual void Start()
    {
        base.Start();
        InitDissolveMaterial(MainManager.instance.JudasesManager.GetJudasGenerator().judasMaterialFade);
    }

    
    public override void Interact()
    {
        // Interactable
        if (GetInteractable())
        {
            SetInteractable(false);
            if (!eventSoundOnInteract.IsNull) PlaySound(eventSoundOnInteract);
            eventOnInteract?.Invoke();
            Grab();
            
            MainManager.instance.JudasesManager.InitJudaObject(this);
            MainManager.instance.Player.SetHasJuda(true);
            MainManager.instance.JudasesManager.GetJudasGenerator().RemoveOtherJudas(this);
            MainManager.instance.JudasesManager.StartJudasEvent();
        }
        // Not Interactable
        else eventOnInteractButNotInteractable?.Invoke();
        
        
        
        // Interactable
        if (GetInteractable())
        {
            SetInteractable(false);
            if (!eventSoundOnInteract.IsNull) PlaySound(eventSoundOnInteract);
            eventOnInteract?.Invoke();
            Grab();
        }
        // Not Interactable
        else eventOnInteractButNotInteractable?.Invoke();
    }
}
