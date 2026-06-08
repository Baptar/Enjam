using UnityEditor;
using UnityEngine;

public class SettingsButton : MonoBehaviour
{
    [SerializeField] private MenuManager menuManager;
    
    public void Settings()
    {
        menuManager.SetPanel(PanelState.SettingsMain);
    }
}
