using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LanguageButton : MonoBehaviour
{
    [SerializeField] private MenuManager menuManager;
    [SerializeField] private GameObject englishButton;
    [SerializeField] private Button spanishButton;
    [SerializeField] private Button backButton;
    

    public void Language()
    {
        menuManager.SetPanel(PanelState.SettingsLanguage);
        InputManager.Instance.SetSelected(englishButton);
        
        // Update back button navigation
        Navigation nav = backButton.navigation;
        nav.selectOnUp = spanishButton;
        backButton.navigation = nav;
    }
}
