using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    [Header("Set in inspector")]
    [SerializeField] GameObject m_defaultSelected;
    
    private bool m_isGamepad;
    private GameState gameState;
    private static InputManager m_instance;
    public static InputManager Instance => m_instance;
    
    public GameState GameState { get => gameState; set => gameState = value; }

    private void Awake()
    {
        if (m_instance != null && m_instance != this)
        {
            Debug.LogWarning("Multiple InputManager instances in scene!");
            Destroy(gameObject);
            return;
        }

        m_instance = this;
    }
    
    private void Start()
    {
        m_isGamepad = false;
        gameState = GameState.Menus;
    }

    public void ShowCursor(bool show)
    {
        Cursor.visible = show;
        Cursor.lockState = show ? CursorLockMode.None : CursorLockMode.Confined;
    }
    
    public void SetSelected(GameObject selected)
    {
        if (EventSystem.current.currentSelectedGameObject != selected)
            EventSystem.current.SetSelectedGameObject(selected);
    }
    
    void Update()
    {
        if (gameState != GameState.Menus) return;
        
        // Mouse check
        if (Mouse.current != null &&
            (Mouse.current.delta.ReadValue() != Vector2.zero ||
             Mouse.current.leftButton.wasPressedThisFrame))
        {
            ShowCursor(true);
            m_isGamepad = false;
        }
        
        // Gamepad and keyboard check
        if (Gamepad.current != null && (
                Gamepad.current.leftStick.ReadValue() != Vector2.zero ||
                Gamepad.current.dpad.ReadValue() != Vector2.zero ||
                Gamepad.current.buttonSouth.wasPressedThisFrame) 
            || Keyboard.current != null &&
                Keyboard.current.anyKey.wasPressedThisFrame)
        {
            ShowCursor(false);
            m_isGamepad = true;
        }
    }
}