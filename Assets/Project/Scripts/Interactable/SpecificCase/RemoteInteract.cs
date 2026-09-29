using DG.Tweening;
using UnityEngine;

public class RemoteInteract : ObjectInteractable
{
    [SerializeField] private CaptureIRLVideo captureVideo;
    private bool bUsed = false;
    
    public override bool GetInteractable() => bInteractable || MainManager.instance.Player.GetHasPile();

    public override void Interact()
    {
        if (GetInteractable())
        {
            bUsed = true;
            SetInteractable(false);
            if (!eventSoundOnInteract.IsNull) PlaySound(eventSoundOnInteract);
            eventOnInteract?.Invoke();

            captureVideo.WatchTv();
            
        }
        else eventOnInteractButNotInteractable?.Invoke();
    }

    public override string GetTextCantInteract()
    {
        return (bUsed ? "" : base.GetTextCantInteract());
    }
}
