using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] private float backgroundAlpha = 0.8f;
    
    [Header("Set in Inspector")] 
    [SerializeField] private CanvasGroup pauseMenu;
    [SerializeField] private InputActionReference pauseAction;
    [SerializeField] private PlayerManager playerManager;


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
