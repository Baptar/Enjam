using DG.Tweening;
using UnityEngine;

public class PileInteract : ObjectGrabbable
{
    [SerializeField] private float playerGrowDuration = 2.0f;
    
    private PlayerManager player; 
    
    protected override void Start()
    {
        player = MainManager.instance.Player;
        base.Start();
    }

    [ContextMenu("Interact")]
    public override void Interact()
    {
        MainManager.instance.Player.SetHasPile(true);
        GrowBackPlayer();
        base.Interact();
    }

    public override void Drop()
    {
        SetTextCantInteract("");
        SetLayer(LayerMask.NameToLayer("Default"));
        OnDropEvent?.Invoke();
        
        objectGrabPointTransform = null;
        if (!objectRigidBody) return;

        Destroy(gameObject, 2.0f);
    }

    [ContextMenu("GrowBackPlayer")]
    public void GrowBackPlayer()
    {
        GrowBackPlayer(playerGrowDuration);
    }
    
    private void GrowBackPlayer(float duration)
    {
        Sequence seq = DOTween.Sequence();
        seq.Join(player.transform.DOScale(player.baseScale, duration).SetEase(Ease.InOutSine))
            .OnUpdate(Physics.SyncTransforms)
            .JoinCallback(() =>
            {
                // modif walk speed
                float valWalkSpeed = player.GetWalkSpeed();
                DOTween.To(() => valWalkSpeed, x => valWalkSpeed = x, player.baseWalkSpeed, duration).SetEase(Ease.InOutFlash)
                    .OnUpdate(() =>
                    {
                        player.SetWalkSpeed(valWalkSpeed);
                    });
            })
            .OnComplete(()=> player.SetGravityScale(player.baseGravityScale));
    }
}
