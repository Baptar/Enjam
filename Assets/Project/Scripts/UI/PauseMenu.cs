using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    [Header("Set in Inspector")] 
    [SerializeField] private CanvasGroup pauseMenu;
    [SerializeField] private InputActionReference pauseAction;

    private bool isPaused;

    private void PauseGame(bool pause)
    {
        isPaused = pause;
        
        MenuManager.ShowCanvasGroup(pause, pauseMenu);
        
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
