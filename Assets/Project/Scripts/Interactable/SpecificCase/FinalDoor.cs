using DG.Tweening;
using UnityEngine;
using UnityEngine.Localization;

public class FinalDoor : ObjectInteractable
{
    [SerializeField] private Animator animator;
    [SerializeField] private Animator animatorEndDoor;
    [SerializeField] private Transform transformKey;
    
    [Space(10)]
    [Header("Judas")]
    [SerializeField] private string judasSceneName;
    [SerializeField] private Transform judasWorldPosition;
    [SerializeField] private Transform cameraJudaTarget;
    [SerializeField] private float judasCamFOV = 1;
    
    [Space(5)]
    [Header("Localization")]
    [SerializeField] private LocalizedString finalDoorCantOpen;
    [SerializeField] private LocalizedString finalDoorCanOpen;
    [SerializeField] private LocalizedString finalDoorLookJuda;
    
    private Transform playerCameraTransform;
    private Transform targetTransform;

    protected override void Start()
    {
        base.Start();
        SetTextCantInteract(finalDoorCantOpen.GetLocalizedString());
    }
    
    public override void Interact()
    {
        if (GetInteractable())
        {
            // look in juda
            if (MainManager.instance.Player.GetHasJuda())
            {
                MainManager.instance.JudasesManager.OnInteractJudas(
                    camJudaTarget : cameraJudaTarget, 
                    judasTransformTarget : judasWorldPosition, 
                    judasSceneName : judasSceneName, 
                    fovCam : judasCamFOV);
                return;
            }
            
            SetInteractable(false);
            if (!eventSoundOnInteract.IsNull) PlaySound(eventSoundOnInteract);

            Sequence sequence = DOTween.Sequence();
            sequence.AppendInterval(2.0f);
            sequence.AppendCallback(() =>
            {
                animator.Play("Door Opened", 0, 0f);
                animatorEndDoor.Play("Door Opened", 0, 0f);
            });
            
            eventOnInteract?.Invoke();
        }
        else eventOnInteractButNotInteractable?.Invoke();
    }

    public override bool GetInteractable()
    {
        bool playerHasJuda = MainManager.instance.Player.GetHasJuda();
        string textInteraction = playerHasJuda ? finalDoorLookJuda.GetLocalizedString() : finalDoorCanOpen.GetLocalizedString();
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

    public void MoveKey(GameObject keyObj)
    {
        keyObj.GetComponent<Rigidbody>().isKinematic = true;
        keyObj.transform.SetParent(transformKey);
        
        Sequence sequence = DOTween.Sequence();
        sequence.Append(keyObj.transform.DOLocalMove(Vector3.zero, 1.0f))
            .Join(keyObj.transform.DOLocalRotate(Vector3.zero, 1.0f))
            .Join(keyObj.transform.DOScale(Vector3.one, 1.0f))
            .Append(keyObj.transform.DOLocalRotate(new Vector3(-75.0f, .0f, .0f), .5f))
            .Append(keyObj.transform.DOLocalRotate(new Vector3(0.0f, .0f, .0f), .5f));
    }
}
