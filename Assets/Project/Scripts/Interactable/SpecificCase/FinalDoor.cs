using System;
using DG.Tweening;
using UnityEngine;

public class FinalDoor : ObjectInteractable
{
    [SerializeField] private Animator animator;
    [SerializeField] private Animator animatorEndDoor;
    
    private Transform playerCameraTransform;
    private Transform targetTransform;

    public override void Interact()
    {
        if (GetInteractable())
        {
            SetInteractable(false);
            if (!eventSoundOnInteract.IsNull) PlaySound(eventSoundOnInteract);
            
            /*bool res = CheckIsInFront();
            
            animator.Play(!res ? "Door Opened" : "Door Opened Reverse", 0, 0f);
            animatorEndDoor.Play(!res ? "Door Opened" : "Door Opened Reverse", 0, 0f);*/
            animator.Play("Door Opened", 0, 0f);
            animatorEndDoor.Play("Door Opened", 0, 0f);
            
            eventOnInteract?.Invoke();
        }
        else eventOnInteractButNotInteractable?.Invoke();
    }

    private bool CheckIsInFront()
    {
        playerCameraTransform = MainManager.instance.PlayerCamera.transform;
        targetTransform = gameObject.transform;
        
        float angleDiff = Vector3.Angle(playerCameraTransform.forward, targetTransform.forward);
        return angleDiff <= 90.0f;
    }
}
