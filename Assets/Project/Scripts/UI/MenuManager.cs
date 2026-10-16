using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [Header("Set in Inspector")]
    [SerializeField] private CanvasGroup mainMenu;
    [SerializeField] private CanvasGroup settingsMenu;
    [SerializeField] private CanvasGroup mainSettingsPanel;
    [SerializeField] private CanvasGroup languageSettingsPanel;
    [SerializeField] private Button languageButton;
    [SerializeField] private Button vsyncButton;
    [SerializeField] private Button sensibilityButton;
    [SerializeField] private GameObject sensibilityScrollbar;
    [SerializeField] private SettingsManager settingsManager;
    
    private PanelState panelState;
    
    public PanelState PanelState { get => panelState; set => panelState = value; }

    private void Start()
    {
        SetPanel(PanelState.Main);
    }
    
    public static void ShowCanvasGroup(bool show, CanvasGroup canvasGroup)
    {
        canvasGroup.alpha = show ? 1 : 0;
        canvasGroup.blocksRaycasts = show;
        canvasGroup.interactable = show;
    }

    public void ResetSensibilityButton()
    {
        sensibilityButton.gameObject.SetActive(true);
        sensibilityScrollbar.gameObject.SetActive(false);
        
        // Update language button navigation
        Navigation nav = languageButton.navigation;
        nav.selectOnDown = sensibilityButton;
        languageButton.navigation = nav;
        
        // Update Vsync button navigation
        nav = vsyncButton.navigation;
        nav.selectOnUp = sensibilityButton;
        vsyncButton.navigation = nav;
    }
    
    public void SetPanel(PanelState state)
    {
        ShowCanvasGroup(state == PanelState.Main, mainMenu);
        ShowCanvasGroup(state == PanelState.SettingsMain, mainSettingsPanel);
        ShowCanvasGroup(state == PanelState.SettingsLanguage, languageSettingsPanel);
        
        ShowCanvasGroup(state != PanelState.Main, settingsMenu);
        ResetSensibilityButton();

        panelState = state;
    }

    public void SetLanguage(int language)
    {
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[language];
        settingsManager.SetVsyncButtonsText();
        settingsManager.SetFullscreenButtonsText();
    }
}
