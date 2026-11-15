using UnityEngine;
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
    [SerializeField] private GameObject ResumeButton;


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

        if (pause)
        {
            InputManager.Instance.SetSelected(ResumeButton);
            inputManager.GameState = GameState.Menus;
        }
            
        else
        {
            InputManager.Instance.SetSelected(null);
            inputManager.GameState = GameState.Game;
        }
        
        Show(pause);

        
        if (inputManager.IsGamepad) return; // If gamepad is used, don't show the cursor on opening
        
        playerManager.SetPaused(pause);
        Cursor.visible = pause;
        Cursor.lockState = pause ? CursorLockMode.None : CursorLockMode.Confined;
    }
    
    private void Update()
    {
        if (pauseAction.action.WasPerformedThisFrame())
        {
            settingsManager.ResetAllScrollbarButtons();
            PauseGame(!isPaused);
        }
    }


    public void Resume()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/UI/UI_Back");
        settingsManager.ResetAllScrollbarButtons();
        PauseGame(false);
    }
    
}
