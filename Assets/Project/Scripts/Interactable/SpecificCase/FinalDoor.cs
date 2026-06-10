using UnityEngine;

public class FinalDoor : ObjectInteractable
{
    [SerializeField] private Animator animator;
    [SerializeField] private Animator animatorEndDoor;
    
    [Space(10)]
    [Header("Judas")]
    [SerializeField] private string judasSceneName;
    [SerializeField] private Transform judasWorldPosition;
    [SerializeField] private Transform cameraJudaTarget;
    [SerializeField] private float judasCamFOV = 1;
    
    private Transform playerCameraTransform;
    private Transform targetTransform;

    public override void Interact()
    {
        if (GetInteractable())
        {
            // look in juda
            if (MainManager.instance.Player.GetHasJuda())
            {
                MainManager.instance.JudasManager.OnInteractJudas(
                    camJudaTarget : cameraJudaTarget, 
                    judasTransformTarget : judasWorldPosition, 
                    judasSceneName : judasSceneName, 
                    fovCam : judasCamFOV);
                return;
            }
            
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

    public override bool GetInteractable()
    {
        bool playerHasJuda = MainManager.instance.Player.GetHasJuda();
        string textInteraction = playerHasJuda ? "Regarder à travers" : "Ouvrir";
        SetTextInteract(textInteraction);
        
        return playerHasJuda || bInteractable;
    }

    private bool CheckIsInFront()
    {
        playerCameraTransform = MainManager.instance.PlayerCamera.transform;
        targetTransform = gameObject.transform;
        
        float angleDiff = Vector3.Angle(playerCameraTransform.forward, targetTransform.forward);
        return angleDiff <= 90.0f;
    }

    public void DoorClosedVibration()
    {
        GamepadVibration.Vibrate(0.5f, 0.5f, 0.15f);
    }
}
