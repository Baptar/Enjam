using DG.Tweening;
using UnityEngine;
using UnityEngine.Localization;

public class RemoteInteract : ObjectInteractable
{
    [SerializeField] private CaptureIRLVideo captureVideo;
    [SerializeField] private LocalizedString switchTVText;
    private bool bUsed = false;
    
    public override bool GetInteractable() => bInteractable || MainManager.instance.Player.GetHasPile();

    public override void Interact()
    {
        if (GetInteractable())
        {
            if (!bUsed)
            {
                SetInteractable(false);
                bUsed = true;
                if (!eventSoundOnInteract.IsNull) PlaySound(eventSoundOnInteract);
                eventOnInteract?.Invoke();
                SetTextInteract(switchTVText.GetLocalizedString());
                captureVideo.WatchTv(this);
            }

            else
            {
                captureVideo.SwitchTvChannel();
            }
        }
        else eventOnInteractButNotInteractable?.Invoke();
    }

    public override string GetTextCantInteract()
    {
        return (bUsed ? "" : base.GetTextCantInteract());
    }
}
