using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Banc : MonoBehaviour
{
    [SerializeField] private bool canTake = true;
    [SerializeField] private string textInteraction;
    [SerializeField] private string textCantInteract;
    
    [SerializeField] private Transform playerNewTransform;
    [SerializeField] private float timeBetweenChangeVision = 1f;
    [SerializeField] private Ease easeMovePlayer =  Ease.InOutElastic;

    
    
   //public void Interact()
   //{
   //    
   //    interactor.MoveVision(playerNewTransform, timeBetweenChangeVision, easeMovePlayer);
   //}
   //
   //public void SetCanTake(bool canTake)
   //{
   //    this.canTake = canTake;
   //}
   //
   //public bool GetCanTake()
   //{
   //    return player.bInBancZone && player.bHasBeer && canTake;
   //    return canTake;
   //}

}
