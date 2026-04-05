using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class BenchInteract : ObjectInteractable
{
    // Bench
    [SerializeField] private Ease easeSitOnBench = Ease.InOutElastic;
    [SerializeField] private float timeBetweenChangeVision = 1f;
    
    // Beer
    [SerializeField] private BeerInteractable beerObj;
    
    private PlayerManager playerManager;

    private void Start() => playerManager = MainManager.instance.Player;
    
    public override bool GetInteractable() => playerManager.GetInBenchZone() && playerManager.GetHasBeer() && bInteractable;

    private void OnTriggerEnter(Collider other) => playerManager.SetInBenchZone(other.CompareTag("Player"));

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            playerManager.SetInBenchZone(false);
    }
    
    public void SitOnBench(Transform sitTransform)
    {
        playerManager.EnableCollision(false);
        float camX = playerManager.GetPlayerCamera().transform.localEulerAngles.x;
        if (camX > 180f) camX -= 360f;
        playerManager.SetPlayerRotationY(camX);
        playerManager.SetCanMove(false);
         
         Sequence sequence = DOTween.Sequence();
         sequence.Append(playerManager.transform.DOMove(sitTransform.position, timeBetweenChangeVision).SetEase(easeSitOnBench))
             .Insert(0.0f,
                 playerManager.transform.DORotateQuaternion(sitTransform.rotation, timeBetweenChangeVision).SetEase(easeSitOnBench))
             .Insert(0.0f,
                 playerManager.GetPlayerCamera().transform
                     .DOLocalRotateQuaternion(Quaternion.Euler(30, 0, 0), timeBetweenChangeVision)
                     .SetEase(easeSitOnBench))
             .OnComplete(()=> beerObj.StartDrinkBeer());
    }
}
