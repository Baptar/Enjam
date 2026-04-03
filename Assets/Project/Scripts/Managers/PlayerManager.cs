using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerManager : MonoBehaviour
{
    [Header("Rotation Settings")]
    [SerializeField] private Camera playerCamera;
    [SerializeField] private float lookSpeed = 2f;
    [SerializeField] private float lookXTopLimit = 45f;
    [SerializeField] private float lookXBotLimit = 55f;
    private float rotationX = 0f;
    
    [Space(5)]
    [Header("Movement Settings")]
    [SerializeField] private float gravityScale = 9.81f;
    [SerializeField] private bool canMove = true;
    [SerializeField] private float walkSpeed = 3f;
    
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
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        //MainManager.instance.PlayerInputManager.OnInteractPressed += OnInteract;
    }

    // Update is called once per frame
    void Update()
    {
        #region Handles Movement

        Vector2 playerMovementInput = playerInputController.Move;
        Vector3 moveDirection = new Vector3(walkSpeed * playerMovementInput.x, -gravityScale, walkSpeed * playerMovementInput.y);
        bIsWalking = playerMovementInput is not { x: 0, y: 0 };
        if (!canMove) return; 
        characterController.Move(transform.rotation * moveDirection * Time.deltaTime);
        #endregion
        
        #region Handles Rotation
        //Vector2 playerRotationInput = playerInputController.Look;
        rotationX += -/*playerRotationInput.y*/Input.GetAxis("Mouse Y") * lookSpeed;
        rotationX = Mathf.Clamp(rotationX, -lookXBotLimit, lookXTopLimit);
        playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
        transform.rotation *= Quaternion.Euler(0, /*playerRotationInput.x*/Input.GetAxis("Mouse X")* lookSpeed, 0);
        #endregion
        
        #region Raycast
        // Raycast Elements
        if (!Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward,
                out RaycastHit raycastHit, raycastDistance, raycastLayerMask))
        {
            MainManager.instance.UIManager.SetCanvaTextInteract("");
            objectInteractable = null;
            return;
        }
        
        // Check if it's an interactable element
        if (!raycastHit.collider.TryGetComponent(out objectInteractable))
        {
            MainManager.instance.UIManager.SetCanvaTextInteract("");
            objectInteractable = null;
            return;
        }
        
        // Get Text to display
        string textToDisplay = objectInteractable.GetInteractable() ? objectInteractable.GetTextInteract() : objectInteractable.GetTextCantInteract();
        MainManager.instance.UIManager.SetCanvaTextInteract(textToDisplay);
        #endregion
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
    public bool GetIsWalking() => bIsWalking;
    
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
    
    // Inventory
    public void SetHasBeer(bool value) => bHasBeer = value;
    public void SetGrabbedObject(ObjectGrabbable value) => grabbedObject = value;
    
    // Zone
    public void SetInBenchZone(bool value) => bIsInBenchZone = value;
    public void SetIsReading(bool value) => bIsReading = value;
    #endregion
}
