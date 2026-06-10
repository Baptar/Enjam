using System.Collections;
using DG.Tweening;
using UnityEngine;

public class RadioInteractable : ObjectGrabbable
{
    [Space(10)]
    [Header("Jump Settings")]
    [SerializeField] private Transform jumpEndValue;
    [SerializeField] private float jumpPower;
    [SerializeField] private float jumpDuration;
    [SerializeField] private float durationBeforeParcAppear = 2.0f;
    
    public override void Interact()
    {
        // Interactable
        if (GetInteractable())
        {
            SetInteractable(false);
            if (!eventSoundOnInteract.IsNull) PlaySound(eventSoundOnInteract);
            eventOnInteract?.Invoke();
            Grab();
            
            MainManager.instance.Player.SetHasRadio(true);
        }
        // Not Interactable
        else eventOnInteractButNotInteractable?.Invoke();
    }

    public override void Drop()
    {
        StartCoroutine(OnDrop());
    }

    // ReSharper disable Unity.PerformanceAnalysis
    private IEnumerator OnDrop()
    {
        MainManager.instance.Player.FocusTarget(transform, jumpDuration - 1.0f);
        
        Sequence sequence = DOTween.Sequence();
        sequence.Append(transform.DOJump(jumpEndValue.position, jumpPower, 1, jumpDuration))
            .OnComplete(() =>
            {
                OnDropEvent?.Invoke();
                if (!objectRigidBody) return;
        
                objectCollider.enabled = true;
                objectGrabPointTransform = null;
                objectRigidBody.freezeRotation = false;
                objectRigidBody.isKinematic = false;
                
                MainManager.instance.Player.SetHasRadio(false);
                
            });
        
        yield return new WaitForSeconds(durationBeforeParcAppear);
        ParcObj.instance.MakeParcAppear();
    }
}
