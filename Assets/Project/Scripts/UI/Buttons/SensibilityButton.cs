using UnityEngine;
using UnityEngine.UI;

public class SensibilityButton : MonoBehaviour
{
    [SerializeField] MenuManager menuManager;
    [SerializeField] private Button languageButton;
    [SerializeField] private Button vsyncButton;
    [SerializeField] private Scrollbar sensibilityScrollbar;
    [SerializeField] private GameObject sensibilityScrollbarContainer;

    public void Sensibility()
    {
        InputManager.Instance.SetSelected(sensibilityScrollbar.gameObject);
        
        // Update language button navigation
        Navigation nav = languageButton.navigation;
        nav.selectOnDown = sensibilityScrollbar;
        languageButton.navigation = nav;
        
        // Update Vsync button navigation
        nav = vsyncButton.navigation;
        nav.selectOnUp = sensibilityScrollbar;
        vsyncButton.navigation = nav;
        
        sensibilityScrollbarContainer.SetActive(true);
        gameObject.SetActive(false);
    }
}
