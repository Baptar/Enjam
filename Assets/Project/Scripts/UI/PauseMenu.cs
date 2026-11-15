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
    [SerializeField] private PlayerManager playerManager;
    [SerializeField] private SettingsManager settingsManager;
    [SerializeField] private InputManager inputManager;
    [SerializeField] private GameObject resumeButton;


    private bool isPaused;
    
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
            inputManager.SetSelected(resumeButton);
            inputManager.GameState = GameState.Menus;
        }
            
        else
        {
            inputManager.SetSelected(null);
            FMODUnity.RuntimeManager.PlayOneShot("event:/UI/UI_Back");
            settingsManager.ResetAllScrollbarButtons();
            inputManager.GameState = GameState.Game;
        }
        
        
        
        if (inputManager.IsGamepad && pause) return; // If gamepad is used, don't show the cursor on opening
        Cursor.visible = pause;
        Cursor.lockState = pause ? CursorLockMode.None : CursorLockMode.Confined;
    }
    
    private void Update()
    {
        if (pauseAction.action.WasPerformedThisFrame())
        {
            PauseGame(!isPaused);
        }
    }


    public void Resume()
    {
        PauseGame(false);
    }
    
}
