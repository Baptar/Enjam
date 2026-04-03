using System;
using UnityEngine;
using UnityEngine.InputSystem;

[DefaultExecutionOrder(-10)]
public class PlayerInputController : MonoBehaviour
{
    public Vector2 Move { get; private set; }
    public Vector2 Look { get; private set; }
    public bool Interact { get; private set; }
    
    
    public event Action OnInteractPressed;
    
    private PlayerInput _playerInput;
    private InputAction _moveAction;
    private InputAction _lookAction;
    private InputAction _interactAction;

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        var actions = _playerInput.actions;

        _moveAction = actions["Move"];
        _lookAction = actions["Look"];
        _interactAction = actions["Interact"];
    }

    private void OnEnable()
    {
        if (_moveAction != null)
        {
            _moveAction.performed += OnMove;
            _moveAction.canceled += OnMove;
        }
        if (_lookAction != null)
        {
            _lookAction.performed += OnLook;
            _lookAction.canceled += OnLook;
        }
        

        // One-shot inputs
        if (_interactAction != null)
        {
            _interactAction.performed += OnInteract;
        }
    }

    private void OnDisable()
    {
        if (_moveAction != null)
        {
            _moveAction.performed -= OnMove;
            _moveAction.canceled -= OnMove;
        }
        if (_lookAction != null)
        {
            _lookAction.performed -= OnLook;
            _lookAction.canceled -= OnLook;
        }

        // One-shot inputs
        if (_interactAction != null)
        {
            _interactAction.performed -= OnInteract;
        }
    }

    // === Handlers (aucune allocation, pur event) ===
    private void OnMove(InputAction.CallbackContext ctx)
    {
        Move = ctx.ReadValue<Vector2>();
    }

    private void OnLook(InputAction.CallbackContext ctx)
    {
        Look = ctx.ReadValue<Vector2>();
    }

    private void OnInteract(InputAction.CallbackContext ctx)
    {
        Interact = ctx.ReadValueAsButton();
        if (ctx.performed) OnInteractPressed?.Invoke();
    }
}