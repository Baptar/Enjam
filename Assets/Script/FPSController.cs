using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class FPSController : MonoBehaviour
{
    public Camera playerCamera;
    

    [HideInInspector] public bool isWalking;
    
    // Rotation Settings
    public float lookSpeed = 2f;
    public float lookXTopLimit = 45f;
    public float lookXBotLimit = 55f;
    float rotationX = 0f;
    
    // Movement Settings
    public float gravityScale = 9.81f;
    public bool canMove = true;
    public float walkSpeed = 3f;
    public float gravity = 10f;
    public bool useGravity = true;

    CharacterController characterController;
    
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
        Vector3 forward = transform.forward;
        
    	Vector3 moveDirection = new Vector3(walkSpeed * Input.GetAxis("Horizontal"), -gravityScale, walkSpeed * Input.GetAxis("Vertical"));
        
        #endregion
        
        #region Handles Rotation
        if (canMove)
            characterController.Move(transform.rotation * moveDirection * Time.deltaTime);

        if (canMove)
        {
            rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXBotLimit, lookXTopLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X")* lookSpeed, 0);
        }
		isWalking = !(Input.GetAxis("Vertical") == 0 && Input.GetAxis("Horizontal") == 0);
		
        
        #endregion
    }
}
