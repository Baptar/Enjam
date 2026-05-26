using System;
using UnityEngine;


public class PlayerManager : MonoBehaviour
{
    [Serializable]
    public enum ELookMode
    {
        Normal, 
        Peephole
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
    [Header("Movement Settings")]
    [SerializeField] private float gravityScale = 9.81f;
    [SerializeField] private float walkSpeed    = 3f;
    [SerializeField] private bool canMove       = true;
    
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
    [Space(2)]
    [Header("Zone")]
    [SerializeField] private bool bIsInBenchZone = false;

   [HideInInspector] public bool bIsWalking;
    
    private CharacterController characterController;
    private PlayerInputController playerInputController;
    private ObjectInteractable objectInteractable;
    private bool bIsReading = false;
    
    
    // Start is called before the first frame update
    void Start()
    {
        playerInputController = MainManager.instance.PlayerInputManager;
        characterController   = GetComponent<CharacterController>();
        Cursor.lockState      = CursorLockMode.Locked;
        Cursor.visible        = false;
        FisheyePostProcess.GlobalStrengthOverride = false;
        
        //MainManager.instance.PlayerInputManager.OnInteractPressed += OnInteract;
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
        
        else if (!Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward,
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
        #endregion
        
        #region Handles Movement
        Vector2 playerMovementInput = playerInputController.Move;
        Vector3 moveDirection = new Vector3(walkSpeed * playerMovementInput.x, -gravityScale, walkSpeed * playerMovementInput.y);
        bIsWalking = playerMovementInput is not { x: 0, y: 0 };
        if (!canMove) return; 
        characterController.Move(transform.rotation * moveDirection * Time.deltaTime);
        #endregion
        
        #region Handles Rotation
        switch (lookMode)
        {
          case ELookMode.Normal: HandleNormalRotation(); break;
          case ELookMode.Peephole: HandlePeepholeRotation(); break;
        }
        #endregion
    }

    private void HandleNormalRotation()
    {
        //Vector2 playerRotationInput = playerInputController.Look;
        rotationY += -/*playerRotationInput.y*/Input.GetAxis("Mouse Y") * lookSpeed;
        rotationY = Mathf.Clamp(rotationY, -lookXBotLimit, lookXTopLimit);
        playerCamera.transform.localRotation = Quaternion.Euler(rotationY, 0, 0);
        transform.rotation *= Quaternion.Euler(0, /*playerRotationInput.x*/Input.GetAxis("Mouse X")* lookSpeed, 0);
    }

    private void HandlePeepholeRotation()
    {
        if (peepholeRoot == null) return;

        //Vector2 look = playerInputController.Look;
        Vector2 look = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        // get target rotation
        peepholeOffset += look * peepholeSensitivity;
        peepholeOffset.x = Mathf.Clamp(peepholeOffset.x, -peepholeMaxAngle, peepholeMaxAngle); // left and right
        peepholeOffset.y = Mathf.Clamp(peepholeOffset.y, -peepholeMaxAngle, peepholeMaxAngle); // up and down

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

    private void OnInteract()
    {
        if (bIsReading)
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
        if (GetGrabbedObject())
        {
            GetGrabbedObject().Drop();
            SetGrabbedObject(null);
        }
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

    public bool GetIsWalking() => bIsWalking;
    public float GetGravityScale() => gravityScale;
    public float GetPlayerRotationY() => rotationY;
    
    // Inventory
    public bool GetHasBeer() => bHasBeer;
    public ObjectGrabbable GetGrabbedObject() => grabbedObject;
    
    //Zone
    public bool GetInBenchZone() => bIsInBenchZone;
    public bool GetIsReading() => bIsReading;
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
    public void SetGrabbedObject(ObjectGrabbable value) => grabbedObject = value;
    
    // Zone
    public void SetInBenchZone(bool value) => bIsInBenchZone = value;
    public void SetIsReading(bool value) => bIsReading = value;
    #endregion
}

/*
using System;
using UnityEngine;


public class PlayerManager : MonoBehaviour
{
    [Serializable]
    public enum ELookMode
    {
        Normal, 
        Peephole
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
    [Header("Movement Settings")]
    [SerializeField] private float gravityScale = 9.81f;
    [SerializeField] private float walkSpeed    = 3f;
    [SerializeField] private bool canMove       = true;
    [SerializeField] private float acceleration = 8f;
    [SerializeField] private float deceleration = 12f;
    private float   _currentSpeed = 0f;
    private Vector2 _smoothMoveInput;
    
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
    [SerializeField] private bool enableBreath         = true;
    [SerializeField] private float breathFrequency     = 0.5f;   // cycles par seconde
    [SerializeField] private float breathPitchAmount   = 0.4f;   // degrés de rotation haut/bas
    [SerializeField] private float breathRollAmount    = 0.15f;  // degrés de tilt gauche/droite
    [SerializeField] private float breathWalkMult      = 1.8f;   // multiplicateur en marchant
    [SerializeField] private float breathSmoothing     = 3f;
    private Quaternion _breathCurrent = Quaternion.identity;

    [Space(5)]
    [Header("Sway")]
    [SerializeField] private bool enableSway       = true;
    [SerializeField] private float swayRollAmount  = 2.5f;   // degrés de tilt en degrés
    [SerializeField] private float swayPitchAmount = 1.2f;
    [SerializeField] private float swaySmoothing   = 5f;
    private Quaternion _swayCurrent = Quaternion.identity;
    private Vector3    _cameraInitialLocalPos;
    
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
    [Space(2)]
    [Header("Zone")]
    [SerializeField] private bool bIsInBenchZone = false;

   [HideInInspector] public bool bIsWalking;
    
    private CharacterController characterController;
    private PlayerInputController playerInputController;
    private ObjectInteractable objectInteractable;
    private bool bIsReading = false;
    
    
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
        //MainManager.instance.PlayerInputManager.OnInteractPressed += OnInteract;
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
        
        else if (!Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward,
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
        #endregion
        
        #region Handles Movement
        Vector2 playerMovementInput = playerInputController.Move;

        float targetSpeed  = playerMovementInput.magnitude > 0.01f ? 1f : 0f;
        float accelRate    = targetSpeed > _currentSpeed ? acceleration : deceleration;
        _currentSpeed      = Mathf.MoveTowards(_currentSpeed, targetSpeed, accelRate * Time.deltaTime);
        bIsWalking         = _currentSpeed > 0.05f;

        if (canMove)
        {
            Vector3 moveDirection = new Vector3(
                walkSpeed * playerMovementInput.x,
                -gravityScale,
                walkSpeed * playerMovementInput.y
            );
            characterController.Move(transform.rotation * moveDirection * _currentSpeed * Time.deltaTime);
        }
        #endregion
        
        #region Handles Rotation

        switch (lookMode)
        {
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

    private void HandleNormalRotation()
    {
        //Vector2 playerRotationInput = playerInputController.Look;
        rotationY += -Input.GetAxis("Mouse Y") * lookSpeed;
        rotationY = Mathf.Clamp(rotationY, -lookXBotLimit, lookXTopLimit);
        
        cameraRoot.localRotation = Quaternion.Euler(rotationY, 0, 0);
        transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
    }

    private void HandlePeepholeRotation()
    {
        if (peepholeRoot == null) return;

        //Vector2 look = playerInputController.Look;
        Vector2 look = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

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

        peepholeRoot.PeepholeCamera.transform.rotation = 
            peepholeBaseRotation * Quaternion.Euler(-peepholeCurrentOffset.y, peepholeCurrentOffset.x, 0f);
    }
    
    private void HandleBob()
    {
        Vector3 targetPos = Vector3.zero;

        if (bIsWalking && canMove)
        {
            _bobTimer += Time.deltaTime * bobFrequency * (1f + _currentSpeed * 0.5f);
            targetPos.y = Mathf.Sin(_bobTimer) * bobAmplitudeY * _currentSpeed;
            targetPos.x = Mathf.Sin(_bobTimer * 2f) * bobAmplitudeX * _currentSpeed;
        }
        else
        {
            _bobTimer += Time.deltaTime * bobFrequency;
            float returnStrength = Mathf.Abs(Mathf.Sin(_bobTimer)) < 0.1f ? 0f : Mathf.Sin(_bobTimer) * bobAmplitudeY * _currentSpeed;
            targetPos.y = returnStrength;
        }

        _bobPosCurrent = Vector3.Lerp(_bobPosCurrent, targetPos, Time.deltaTime * bobSmoothing);
    }

    private void HandleBreath()
    {
        float mult = 1f + (_currentSpeed * (breathWalkMult - 1f));

        float pitch = Mathf.Sin(Time.time * breathFrequency * Mathf.PI * 2f) * breathPitchAmount * mult;
        float roll  = Mathf.Sin(Time.time * breathFrequency * Mathf.PI * 2f * 0.5f) * breathRollAmount * mult;

        Quaternion breathTarget = Quaternion.Euler(pitch, 0f, roll);
        _breathCurrent = Quaternion.Slerp(_breathCurrent, breathTarget, Time.deltaTime * breathSmoothing);
    }

    private void HandleSway()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        Quaternion targetRoll  = Quaternion.AngleAxis(-mouseX * swayRollAmount,  Vector3.forward);
        Quaternion targetPitch = Quaternion.AngleAxis(-mouseY * swayPitchAmount, Vector3.right);
        Quaternion swayTarget  = targetRoll * targetPitch;

        _swayCurrent = Quaternion.Slerp(_swayCurrent, swayTarget, Time.deltaTime * swaySmoothing);
    }

    private void ApplyCameraFeel()
    {
        if (enableHeadBob) playerCamera.transform.localPosition = _cameraInitialLocalPos + _bobPosCurrent;
        
        Quaternion breathValue = enableBreath ? _breathCurrent : Quaternion.identity;
        Quaternion swayValue = enableSway ? _swayCurrent : Quaternion.identity;
        playerCamera.transform.localRotation = swayValue * breathValue;
    }

    private void OnInteract()
    {
        if (bIsReading)
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
        if (GetGrabbedObject())
        {
            GetGrabbedObject().Drop();
            SetGrabbedObject(null);
        }
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

    public bool GetIsWalking() => bIsWalking;
    public float GetGravityScale() => gravityScale;
    public float GetPlayerRotationY() => rotationY;
    
    // Inventory
    public bool GetHasBeer() => bHasBeer;
    public ObjectGrabbable GetGrabbedObject() => grabbedObject;
    
    //Zone
    public bool GetInBenchZone() => bIsInBenchZone;
    public bool GetIsReading() => bIsReading;
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
    public void SetGrabbedObject(ObjectGrabbable value) => grabbedObject = value;
    
    // Zone
    public void SetInBenchZone(bool value) => bIsInBenchZone = value;
    public void SetIsReading(bool value) => bIsReading = value;
    #endregion
}
*/
