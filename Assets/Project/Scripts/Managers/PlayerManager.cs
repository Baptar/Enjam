using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    private bool bIsGrabbingObject = false;
    
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
    private bool bIsReading = false;
    
    
    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        #region Handles Movement
        Vector3 moveDirection = new Vector3(walkSpeed * Input.GetAxis("Horizontal"), -gravityScale, walkSpeed * Input.GetAxis("Vertical"));
        bIsWalking = !(Input.GetAxis("Vertical") == 0 && Input.GetAxis("Horizontal") == 0);
        if (!canMove) return; 
        characterController.Move(transform.rotation * moveDirection * Time.deltaTime);
        #endregion
        
        #region Handles Rotation
        rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
        rotationX = Mathf.Clamp(rotationX, -lookXBotLimit, lookXTopLimit);
        playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
        transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X")* lookSpeed, 0);
        #endregion

        #region Handles ReadPaper
        if (bIsReading)
        {
            // TODO CHANGE LATER FOR NEW INPUT SYSTEM
            if (!Input.GetKeyDown(KeyCode.E)) return;
            
            bIsReading = false;
            MainManager.instance.PaperManager.RemovePaper();
            return;
        }
        #endregion
        
        #region Raycast
        // Raycast Elements
        if (!Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward,
                out RaycastHit raycastHit, raycastDistance, raycastLayerMask))
        {
            MainManager.instance.UIManager.SetCanvaTextInteract("");
            return;
        }
        
        // Check if it's an interactable element
        if (!raycastHit.collider.TryGetComponent(out ObjectInteractable objectInteractable))
        {
            MainManager.instance.UIManager.SetCanvaTextInteract("");
            return;
        }
        
        // Get Text to display
        string textToDisplay = objectInteractable.GetInteractable() ? objectInteractable.GetTextInteract() : objectInteractable.GetTextCantInteract();
        MainManager.instance.UIManager.SetCanvaTextInteract(textToDisplay);
        
        // Manager Input to interact (TODO Change later for new input system)
        if (Input.GetKeyDown(KeyCode.E)) objectInteractable.Interact();
        #endregion
    }

    public void EnableCollision(bool value) => GetComponent<Collider>().enabled = value;

    #region Getter
    // Component
    public Camera GetPlayerCamera() => playerCamera;
    public Transform GetObjectGrabPointTransform() => objectGrabPointTransform;
    
    // Movement
    public bool GetCanMove() => canMove;
    public bool GetIsWalking() => bIsWalking;
    
    // Inventory
    public bool GetHasBeer() => bHasBeer;
    public bool GetIsGrabbingObject() => bIsGrabbingObject;
    
    //Zone
    public bool GetInBenchZone() => bIsInBenchZone;
    #endregion
    
    #region Setter
    // Movement
    public void SetCanMove(bool value) => canMove = value;
    
    // Inventory
    public void SetHasBeer(bool value) => bHasBeer = value;
    public void SetIsGrabbingObject(bool value) => bIsGrabbingObject = value;
    
    // Zone
    public void SetInBenchZone(bool value) => bIsInBenchZone = value;
    #endregion
}
