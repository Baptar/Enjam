using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;


public class PlayerManager : MonoBehaviour
{
    [Serializable]
    public enum ELookMode
    {
        Normal, 
        Peephole,
        CantLook
    }
    
    [Header("Rotation Normal Settings")]
    [SerializeField] private Camera playerCamera;
    [SerializeField] private Transform cameraRoot;
    [SerializeField] private float lookSpeed     = 2f;
    [SerializeField] private float lookXTopLimit = 45f;
    [SerializeField] private float lookXBotLimit = 55f;
    private float rotationY = 0f;

    [Space(2)] [Header("Rotation Peephole Settings")]
    [SerializeField] private float peepholeSensitivity = 0.5f;
    [SerializeField] private float peepholeMaxOffset   = 0.3f;
    [SerializeField] private float peepholeSmoothing   = 8f;
    [SerializeField] private float peepholeMaxAngle    = 40f;
    private PeepholeSceneRoot peepholeRoot;
    private Vector3           peepholeBasePosition;
    private Quaternion        peepholeBaseRotation;
    private Vector2           peepholeOffset;        
    private Vector2           peepholeCurrentOffset;
    private bool              isInJudasMode;
    
    [Space(5)]
    [Header("Head Bob")]
    [SerializeField] private bool enableHeadBob  = true;
    [SerializeField] private float bobFrequency  = 2.0f;
    [SerializeField] private float bobAmplitudeY = 0.035f;
    [SerializeField] private float bobAmplitudeX = 0.018f;
    [SerializeField] private float bobSmoothing  = 10f;
    private float   _bobTimer;
    private Vector3 _bobPosCurrent;

    [Space(5)]
    [Header("Breath")]
    [SerializeField] private bool enableBreath       = true;
    [SerializeField] private float breathFrequency   = 0.5f;
    [SerializeField] private float breathPitchAmount = 0.4f;
    [SerializeField] private float breathRollAmount  = 0.15f;
    [SerializeField] private float breathWalkMult    = 1.8f;
    [SerializeField] private float breathSmoothing   = 3f;
    private Quaternion _breathCurrent = Quaternion.identity;

    [Space(5)]
    [Header("Sway")]
    [SerializeField] private bool enableSway       = true;
    [SerializeField] private float swayRollAmount  = 2.5f;
    [SerializeField] private float swayPitchAmount = 1.2f;
    [SerializeField] private float swaySmoothing   = 5f;
    private Quaternion _swayCurrent = Quaternion.identity;
    private Vector3    _cameraInitialLocalPos;
    
    [Space(10)]
    [Header("Movement Settings")]
    [SerializeField] private float gravityScale = 9.81f;
    [SerializeField] private float walkSpeed    = 3f;
    [SerializeField] private bool canMove       = true;
    [SerializeField] private float acceleration = 8f;
    [SerializeField] private float deceleration = 12f;
    private float _currentSpeed = 0f;
    
    [Space(5)]
    [Header("Raycast Settings")]
    [SerializeField] private float raycastDistance = 2f;
    [SerializeField] private LayerMask raycastLayerMask;
    
    [Space(5)]
    [Header("Grabbable Settings")]
    [SerializeField] private Transform objectGrabPointTransform;
    [SerializeField] private Transform objectGrabPointPaperTransform;
    private ObjectGrabbable grabbedObject = null;
    
    [Space(10)]
    [Header("DEBUG")]
    [SerializeField] private ELookMode lookMode = ELookMode.Normal;
    [Space(2)]
    [Header("Inventory")]
    [SerializeField] private bool bHasBeer = false;
    [SerializeField] private bool bHasRadio = false;
    [SerializeField] private bool bHasJuda = false;
    [Space(2)]
    [Header("Zone")]
    [SerializeField] private bool bInBenchZone = false;

   [HideInInspector] public bool bWalking;
    
    private CharacterController characterController;
    private PlayerInputController playerInputController;
    private ObjectInteractable objectInteractable;
    private bool bReading = false;
    public bool bInInteractionZone = false;
    
    // Start is called before the first frame update
    void Start()
    {
        playerInputController = MainManager.instance.PlayerInputManager;
        characterController   = GetComponent<CharacterController>();
        Cursor.lockState      = CursorLockMode.Locked;
        Cursor.visible        = false;
        FisheyePostProcess.GlobalStrengthOverride = false;
        _cameraInitialLocalPos = playerCamera.transform.localPosition;
        _swayCurrent           = Quaternion.identity;
        
        MainManager.instance.PlayerInputManager.OnInteractPressed += OnInteract;
    }

    // Update is called once per frame
    void Update()
    {
        #region Raycast
        // Raycast Elements
        if (lookMode != ELookMode.Normal)
        {
            MainManager.instance.UIManager.SetCanvaTextInteract("");
            objectInteractable = null;
        }

        else if (!GetIsInInteractionZone())
        {
            if (!Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward,
                    out RaycastHit raycastHit, raycastDistance, raycastLayerMask))
            {
                MainManager.instance.UIManager.SetCanvaTextInteract("");
                objectInteractable = null;
            }
        
            // Check if it's an interactable element
            else if (!raycastHit.collider.TryGetComponent(out objectInteractable))
            {
                MainManager.instance.UIManager.SetCanvaTextInteract("");
                objectInteractable = null;
            }

            else
            {
                // Get Text to display
                string textToDisplay = objectInteractable.GetInteractable() ? objectInteractable.GetTextInteract() : objectInteractable.GetTextCantInteract();
                MainManager.instance.UIManager.SetCanvaTextInteract(textToDisplay);
            }
        }
        #endregion
        
        #region Handles Movement
        Vector2 playerMovementInput = playerInputController.Move;
        Vector3 moveDirection = new Vector3(walkSpeed * playerMovementInput.x, -gravityScale, walkSpeed * playerMovementInput.y);
        bWalking = playerMovementInput is not { x: 0, y: 0 };
        if (canMove)
        {
            characterController.Move(transform.rotation * moveDirection * Time.deltaTime);
        }
        #endregion
        
        #region Handles Rotation
        switch (lookMode)
        {
            case ELookMode.CantLook: break;
            case ELookMode.Normal: HandleNormalRotation(); break;
            case ELookMode.Peephole: HandlePeepholeRotation(); break;
        }
        #endregion
        
        #region Camera Feel
        if (enableHeadBob) HandleBob();
        if (enableBreath) HandleBreath();
        if (enableSway) HandleSway();
        ApplyCameraFeel();
        #endregion
    }

    #region Rotation
    private void HandleNormalRotation()
    {
        Vector2 delta = playerInputController.Look;

        rotationY -= delta.y * lookSpeed;
        rotationY = Mathf.Clamp(rotationY, -lookXBotLimit, lookXTopLimit);

        transform.rotation *= Quaternion.Euler(0, delta.x * lookSpeed, 0);
    }

    private void HandlePeepholeRotation()
    {
        if (peepholeRoot == null) return;

        Vector2 look = playerInputController.Look;

        // get target rotation
        peepholeOffset += look * peepholeSensitivity;
        peepholeOffset.x = Mathf.Clamp(peepholeOffset.x, -peepholeMaxAngle, peepholeMaxAngle);
        peepholeOffset.y = Mathf.Clamp(peepholeOffset.y, -peepholeMaxAngle, peepholeMaxAngle);

        // smooth
        peepholeCurrentOffset = Vector2.Lerp(
            peepholeCurrentOffset,
            peepholeOffset,
            Time.deltaTime * peepholeSmoothing
        );

        // local rotation
        peepholeRoot.PeepholeCamera.transform.rotation = 
            peepholeBaseRotation * Quaternion.Euler(-peepholeCurrentOffset.y, peepholeCurrentOffset.x, 0f);
    }
    #endregion
    
    #region Camera Feel
    private void HandleBob()
    {
        Vector3 targetPos = Vector3.zero;

        if (bWalking && canMove)
        {
            _bobTimer += Time.deltaTime * bobFrequency;
            targetPos.y = Mathf.Sin(_bobTimer) * bobAmplitudeY;
            targetPos.x = Mathf.Sin(_bobTimer * 2f) * bobAmplitudeX;
        }

        _bobPosCurrent = Vector3.Lerp(_bobPosCurrent, targetPos, Time.deltaTime * bobSmoothing);
    }

    private void HandleBreath()
    {
        float pitch = Mathf.Sin(Time.time * breathFrequency * Mathf.PI * 2f) * breathPitchAmount;
        float roll  = Mathf.Sin(Time.time * breathFrequency * Mathf.PI * 2f * 0.5f) * breathRollAmount;

        Quaternion breathTarget = Quaternion.Euler(pitch, 0f, roll);
        _breathCurrent = Quaternion.Slerp(_breathCurrent, breathTarget, Time.deltaTime * breathSmoothing);
    }

    private void HandleSway()
    {
        Vector2 look = playerInputController.Look;

        Quaternion targetRoll  = Quaternion.AngleAxis(-look.x * swayRollAmount,  Vector3.forward);
        Quaternion targetPitch = Quaternion.AngleAxis(-look.y * swayPitchAmount, Vector3.right);
        _swayCurrent = Quaternion.Slerp(_swayCurrent, targetRoll * targetPitch, Time.deltaTime * swaySmoothing);
    }
    
    private void ApplyCameraFeel()
    {
        if (enableHeadBob) playerCamera.transform.localPosition = _cameraInitialLocalPos + _bobPosCurrent;

        Quaternion verticalRotation = Quaternion.Euler(rotationY, 0, 0);
        Quaternion breathValue      = enableBreath ? _breathCurrent : Quaternion.identity;
        Quaternion swayValue        = enableSway   ? _swayCurrent   : Quaternion.identity;

        playerCamera.transform.localRotation = verticalRotation * swayValue * breathValue;
    }
    #endregion

    private void OnInteract()
    {
        if (bReading)
        {
            SetIsReading(false);
            SetGrabbedObject(null);
            MainManager.instance.PaperManager.RemovePaper();
            return;
        }

        if (lookMode == ELookMode.Peephole && isInJudasMode)
        {
            StartCoroutine(MainManager.instance.JudasManager.ExitJudas());
        }
        
        if (objectInteractable) 
        {
            objectInteractable.Interact(); 
        }
    }

    public void Drop()
    {
        if (!GetGrabbedObject()) return;
        
        GetGrabbedObject().Drop();
        SetGrabbedObject(null);
    }

    public void LookPoint(Vector3 targetPoint, float duration = 0.5f, Ease ease = Ease.InOutFlash)
    {
        SetCanMove(false);
        ELookMode previousLookMode = GetLookMode();
        SetLookMode(ELookMode.CantLook);
        
        Vector3 direction = (targetPoint - playerCamera.transform.position).normalized;
        float targetBodyY = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        float targetRotationY = Mathf.Clamp(-Mathf.Asin(direction.y) * Mathf.Rad2Deg, -lookXBotLimit, lookXTopLimit);
        
        Vector3 targetRotation = new Vector3(0f, targetBodyY, 0f);
        
        Sequence sequence = DOTween.Sequence();
        sequence.Append(transform.DORotate(targetRotation, duration).SetEase(ease))
            .Join( DOTween.To(() => rotationY, x => rotationY = x, targetRotationY, duration).SetEase(ease))
            .OnComplete(() =>
            {
                SetCanMove(true);
                SetLookMode(previousLookMode);
            });
    }
    
    #region Getter
    // Component
    public Camera GetPlayerCamera() => playerCamera;
    public Transform GetObjectGrabPointTransform() => objectGrabPointTransform;
    public Transform GetObjectGrabPointPaperTransform() => objectGrabPointPaperTransform;
    
    // Movement
    public bool GetCanMove() => canMove;
    public float GetWalkSpeed() => walkSpeed;
    public ELookMode GetLookMode() => lookMode;

    public bool GetIsWalking() => bWalking;
    public float GetGravityScale() => gravityScale;
    public float GetPlayerRotationY() => rotationY;
    
    // Inventory
    public bool GetHasBeer() => bHasBeer;
    public bool GetHasRadio() => bHasRadio;
    public bool GetHasJuda() => bHasJuda;
    public ObjectGrabbable GetGrabbedObject() => grabbedObject;
    
    //Zone
    public bool GetInBenchZone() => bInBenchZone;
    public bool GetIsReading() => bReading;
    public bool GetIsInInteractionZone() => bInInteractionZone;
    #endregion
    
    #region Setter
    public void EnableCollision(bool value) => GetComponent<Collider>().enabled = value;
    
    // Movement
    public void SetCanMove(bool value) => canMove = value;
    public void SetWalkSpeed(float value) => walkSpeed = value;
    public void SetGravityScale(float value) => gravityScale = value;
    public void SetPlayerRotationY(float value) => rotationY = value;
    public void SetLookMode(ELookMode value) => lookMode = value;

    public void SetPeepholeRoot(PeepholeSceneRoot value)
    {
        peepholeRoot = value;
        peepholeBasePosition = value.PeepholeCamera.transform.position;
        peepholeBaseRotation = value.PeepholeCamera.transform.rotation;
        peepholeOffset = Vector2.zero;
        peepholeCurrentOffset = Vector2.zero;    
    }
    
    public void SetIsInJudasMode(bool value) => isInJudasMode = value;
    
    // Inventory
    public void SetHasBeer(bool value) => bHasBeer = value;
    public void SetHasRadio(bool value) => bHasRadio = value;
    public void SetHasJuda(bool value) => bHasJuda = value;
    public void SetGrabbedObject(ObjectGrabbable value) => grabbedObject = value;
    public void SetObjectInteractable(ObjectInteractable value) => objectInteractable = value;
    
    // Zone
    public void SetInBenchZone(bool value) => bInBenchZone = value;
    public void SetIsReading(bool value) => bReading = value;

    public void SetIsInInteractionZone(bool value)
    {
        bInInteractionZone = value;   
    }
    #endregion
}