using UnityEngine;
using UnityEngine.Localization.Settings;

public class MenuManager : MonoBehaviour
{
    [Header("Set in Inspector")]
    [SerializeField] private CanvasGroup mainMenu;
    [SerializeField] private CanvasGroup settingsMenu;
    [SerializeField] private CanvasGroup mainSettingsPanel;
    [SerializeField] private CanvasGroup languageSettingsPanel;
    
    private PanelState panelState;
    
    public PanelState PanelState { get => panelState; set => panelState = value; }

    private void Start()
    {
        SetPanel(PanelState.Main);
    }
    
    private static void ShowCanvasGroup(bool show, CanvasGroup canvasGroup)
    {
        canvasGroup.alpha = show ? 1 : 0;
        canvasGroup.blocksRaycasts = show;
        canvasGroup.interactable = show;
    }
    
    public void SetPanel(PanelState state)
    {
        ShowCanvasGroup(state == PanelState.Main, mainMenu);
        ShowCanvasGroup(state == PanelState.SettingsMain, mainSettingsPanel);
        ShowCanvasGroup(state == PanelState.SettingsLanguage, languageSettingsPanel);
        
        ShowCanvasGroup(state != PanelState.Main, settingsMenu);

        panelState = state;
    }

    public void SetLanguage(int language)
    {
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[language];
    }
}
