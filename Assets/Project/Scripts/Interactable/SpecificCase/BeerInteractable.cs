using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class BeerInteractable : ObjectGrabbable
{
    [SerializeField] private float drinkEffectDelay = 0.5f;
    [SerializeField] private float shrinkScale = 0.3f;
    [SerializeField] private float shrinkGravity = 0.3f;
    [SerializeField] private float shrinkDuration = 2f;
    [SerializeField] private float shrinkWalkSpeed = 2f;
    [SerializeField] private UnityEvent onShrinkEnd;
    private Animator animator;

    protected override void Awake()
    {
        base.Awake();
        animator = GetComponent<Animator>();
    }
    
    [ContextMenu("Start Drink Beer")]
    public void StartDrinkBeer()
    {
        if (!animator) return;
        StartCoroutine(DrinkBeer(drinkEffectDelay));
    }

    private IEnumerator DrinkBeer(float duration)
    {
        animator.Play("glouglouMieux", 0, 0.0f);
        PlaySound("event:/Park/BeerDrunk");
        yield return new WaitForSeconds(duration);
        Drop();
    }

    private void OnBeerDrunken()
    {
        PlayerManager player = MainManager.instance.Player;
        MainManager.instance.AudioManager.PlaySound("event:/Park/GoLittle", transform);
        MainManager.instance.AudioManager.PlaySound("event:/MX", transform);
        
        MainManager.instance.Player.EnableCollision(true);
        float camX = player.GetPlayerCamera().transform.localEulerAngles.x;
        if (camX > 180f) camX -= 360f;
    
        DOTween.To(
            () => player.GetPlayerRotationY(), 
            x => player.SetPlayerRotationY(x), 
            camX, 
            0.3f
        ).OnComplete(() => player.SetCanMove(true));
        
        Sequence seq = DOTween.Sequence();
        seq.Append(player.transform.DOScale(shrinkScale, shrinkDuration)).SetEase(Ease.InOutFlash)
            .InsertCallback(0.0f, () =>
            {
                float valGravityScale = player.GetGravityScale();
                DOTween.To(() => valGravityScale, x => valGravityScale = x, shrinkGravity, shrinkDuration).SetEase(Ease.InOutFlash)
                    .OnUpdate(() =>
                    {
                        player.SetGravityScale(valGravityScale);
                    });
            })
            .InsertCallback(0.0f, () =>
            {
                float valWalkSpeed = player.GetWalkSpeed();
                DOTween.To(() => valWalkSpeed, x => valWalkSpeed = x, shrinkWalkSpeed, shrinkDuration).SetEase(Ease.InOutFlash)
                    .OnUpdate(() =>
                    {
                        player.SetWalkSpeed(valWalkSpeed);
                    });
            })
            .OnComplete(()=> onShrinkEnd?.Invoke());
    }
}
