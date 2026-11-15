using UnityEngine;
using UnityEngine.UI;

public class BackButton : MonoBehaviour
{
    [SerializeField] MenuManager menuManager;
    [SerializeField] SettingsManager settingsManager;
    [SerializeField] GameObject settingsButton; 
    [SerializeField] Button fullscreenButton; 
    
    public void Back()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/UI/UI_Back");
        settingsManager.ResetAllScrollbarButtons();
        menuManager.SetPanel(PanelState.Main);
        InputManager.Instance.SetSelected(settingsButton);
    }
}
