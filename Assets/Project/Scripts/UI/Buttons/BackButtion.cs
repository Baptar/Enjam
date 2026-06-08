using UnityEngine;

public class BackButton : MonoBehaviour
{
    [SerializeField] MenuManager menuManager;
    
    public void Back()
    {
        if (menuManager.PanelState == PanelState.SettingsLanguage)
        {            
            menuManager.SetPanel(PanelState.SettingsMain);
        }
        else if (menuManager.PanelState == PanelState.SettingsMain)
        {
            menuManager.SetPanel(PanelState.Main);
        }
    }
}
