using DG.Tweening;
using UnityEngine;

public class PileInteract : ObjectGrabbable
{
    [SerializeField] private float playerGrowDuration = 2.0f;
    
    private float playerLocalScaleStart;
    private PlayerManager player; 
    
    protected override void Start()
    {
        player = MainManager.instance.Player;
        playerLocalScaleStart = player.baseScale;
        base.Start();
    }

    [ContextMenu("Interact")]
    public override void Interact()
    {
        MainManager.instance.Player.SetHasPile(true);
        GrowBackPlayer();
        base.Interact();
    }

    [ContextMenu("GrowBackPlayer")]
    public void GrowBackPlayer()
    {
        GrowBackPlayer(playerGrowDuration);
    }
    
    private void GrowBackPlayer(float duration)
    {
        Sequence seq = DOTween.Sequence();
        CharacterController characterController = player.GetComponent<CharacterController>();
        float initialScale = player.transform.localScale.y;
        float initialBottom = player.transform.position.y - (characterController.height * initialScale * 0.5f);

        seq.Join(player.transform.DOScale(playerLocalScaleStart, duration).SetEase(Ease.InOutSine))
            .OnStart(() =>
            {
                player.SetGravityScale(0.0f);
            })
            .OnUpdate(() =>
            {
                float currentScale = player.transform.localScale.y;
                float currentBottom = player.transform.position.y - (characterController.height * 0.75f * currentScale * 0.5f);
                float correction = initialBottom - currentBottom;

                player.transform.position += Vector3.up * correction;
            })
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
            .OnComplete(() =>
            {
                float valGravityScale = 0.0f;
                DOTween.To(() => valGravityScale, x => valGravityScale = x, player.baseGravityScale, 10.0f)
                    .OnUpdate(() =>
                    {
                        player.SetGravityScale(valGravityScale);
                    });
            });
    }
}
