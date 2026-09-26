using UnityEngine;
using UnityEngine.UI;

public class BackButton : MonoBehaviour
{
    [SerializeField] MenuManager menuManager;
    [SerializeField] GameObject settingsButton; 
    [SerializeField] Button sensibilityButton; 
    
    public void Back()
    {
        if (menuManager.PanelState == PanelState.SettingsLanguage)
        {
            menuManager.SetPanel(PanelState.SettingsMain);
            InputManager.Instance.SetSelected(gameObject);

            // Update back button navigation
            Button button = GetComponent<Button>();
            Navigation nav = button.navigation;
            nav.selectOnUp = sensibilityButton;
            button.navigation = nav;
        }
        else if (menuManager.PanelState == PanelState.SettingsMain)
        {
            menuManager.SetPanel(PanelState.Main);
            InputManager.Instance.SetSelected(settingsButton);

        }
    }
}
