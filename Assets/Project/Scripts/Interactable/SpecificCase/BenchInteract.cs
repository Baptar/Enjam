using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class BenchInteract : ObjectInteractable
{
    // Bench
    [SerializeField] private Transform playerSitTransform;
    [SerializeField] private Ease easeSitOnBench = Ease.InOutElastic;
    [SerializeField] private float timeBetweenChangeVision = 1f;
    
    // Beer
    [SerializeField] private Animator beerAnimation;
    [SerializeField] private float drinkDuration = 3.0f;
    
    private PlayerManager playerManager;

    private void Start() => playerManager = MainManager.instance.Player;
    
    public override bool GetInteractable() => playerManager.GetInBenchZone() && playerManager.GetHasBeer() && bInteractable;

    public override void Interact() => SitOnBench(playerSitTransform);
    
    private void SitOnBench(Transform sitTransform)
    {
        playerManager.EnableCollision(false);
        playerManager.SetCanMove(false);
         
         Sequence sequence = DOTween.Sequence();
         sequence.Append(transform.DOMove(sitTransform.position, timeBetweenChangeVision).SetEase(easeSitOnBench))
             .Insert(0.0f,
                 transform.DORotateQuaternion(sitTransform.rotation, timeBetweenChangeVision).SetEase(easeSitOnBench))
             .Insert(0.0f,
                 playerManager.GetPlayerCamera().transform
                     .DOLocalRotateQuaternion(Quaternion.Euler(30, 0, 0), timeBetweenChangeVision)
                     .SetEase(easeSitOnBench))
             .OnComplete(()=> StartCoroutine(DrinkBeer()));
    }

    // TODO
    IEnumerator DrinkBeer()
    {
        beerAnimation.Play("glouglouMieux", 0, 0.0f);
        PlaySound("event:/Park/BeerDrunk");
        yield return new WaitForSeconds(drinkDuration);
        /*
        Drop();
        canTake = false;
        playerPickUp.OnBeerDrunken();
         */
    }
    
    /*
      public void OnBeerDrunken()
    {
        parcExtCollider.SetActive(true);
        fpsController.canMove = true;
        StartCoroutine(Shrink());
    }

    IEnumerator Shrink()
    {
        float timeElapsed = 0;
        Vector3 startScale = transform.localScale;
        float walkSpeedStart = fpsController.walkSpeed;
        float gravityStart = fpsController.gravityScale;
        FMODUnity.RuntimeManager.PlayOneShot("event:/Park/GoLittle");
        FMODUnity.RuntimeManager.PlayOneShot("event:/MX");

        while (timeElapsed < timeToShrink)
        {
            transform.localScale = Vector3.Lerp(startScale, shrinkScale, timeElapsed / timeToShrink);
            fpsController.walkSpeed = Mathf.Lerp(walkSpeedStart, shrinkSpeed, timeElapsed / timeToShrink);
            fpsController.gravityScale = Mathf.Lerp(gravityStart, gravityShrink, timeElapsed / timeToShrink);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        pile.gameObject.SetActive(true);
    }
     */
}
