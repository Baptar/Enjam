using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] private float backgroundAlpha = 0.98f;
    
    [Header("Set in Inspector")] 
    [SerializeField] private CanvasGroup pauseMenu;
    [SerializeField] private InputActionReference pauseAction;
    [SerializeField] private InputActionReference backAction;
    [SerializeField] private PlayerManager playerManager;
    [SerializeField] private SettingsManager settingsManager;
    [SerializeField] private GameObject resumeButton;
    
    private static PauseMenu m_instance;
    public static PauseMenu Instance => m_instance;


    private bool isPaused;
    
    public bool IsPaused => isPaused;


    private void Awake()
    {
        if (m_instance != null && m_instance != this)
        {
            Debug.LogWarning("Multiple PauseMenu instances in scene!");
            Destroy(gameObject);
            return;
        }

        m_instance = this;
    }
    
    public void Show(bool show)
    {
        pauseMenu.alpha = show ? backgroundAlpha : 0;
        pauseMenu.blocksRaycasts = show;
        pauseMenu.interactable = show;
    }

    private void PauseGame(bool pause)
    {
        isPaused = pause;
        Show(pause);
        playerManager.SetPaused(pause);

        if (pause)
        {
            InputManager.Instance.SetSelected(resumeButton);
            InputManager.Instance.GameState = GameState.Menus;
        }
            
        else
        {
            InputManager.Instance.SetSelected(null);
            FMODUnity.RuntimeManager.PlayOneShot("event:/UI/UI_Back");
            settingsManager.ResetAllScrollbarButtons();
            InputManager.Instance.GameState = GameState.Game;
        }
        
        
        
        if (InputManager.Instance.IsGamepad && pause) return; // If gamepad is used, don't show the cursor on opening
        Cursor.visible = pause;
        Cursor.lockState = pause ? CursorLockMode.None : CursorLockMode.Locked;
    }
    
    private void Update()
    {
        if (pauseAction.action.WasPerformedThisFrame())
        {
            PauseGame(!isPaused);
        }
        else if (backAction.action.WasPerformedThisFrame() && isPaused)
        {
            PauseGame(false);
        }
    }


    public void Resume()
    {
        PauseGame(false);
    }
    
}
