using UnityEditor;
using UnityEngine;

public class SettingsButton : MonoBehaviour
{
    [SerializeField] private MenuManager menuManager;
    [SerializeField] private GameObject backButton;
    
    public void Settings()
    {
        menuManager.SetPanel(PanelState.SettingsMain);
        InputManager.Instance.SetSelected(backButton);
    }
}
