using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [Header("Set in Inspector")]
    [SerializeField] private CanvasGroup mainMenu;
    [SerializeField] private CanvasGroup settingsMenu;
    [SerializeField] private CanvasGroup mainSettingsPanel;
    [SerializeField] private Button languageButton;
    [SerializeField] private Button vsyncButton;
    [SerializeField] private Button sensibilityButton;
    [SerializeField] private GameObject sensibilityScrollbar;
    [SerializeField] private SettingsManager settingsManager;
    [SerializeField] private InputManager inputManager;
    
    private PanelState panelState;
    
    public PanelState PanelState { get => panelState; set => panelState = value; }

    private void Start()
    {
        SetPanel(PanelState.Main);
        inputManager.GameState = GameState.Menus;
    }
    
    public static void ShowCanvasGroup(bool show, CanvasGroup canvasGroup)
    {
        canvasGroup.alpha = show ? 1 : 0;
        canvasGroup.blocksRaycasts = show;
        canvasGroup.interactable = show;
    }
    
    public void SetPanel(PanelState state)
    {
        ShowCanvasGroup(state == PanelState.Main, mainMenu);
        ShowCanvasGroup(state == PanelState.SettingsMain, mainSettingsPanel);
        
        ShowCanvasGroup(state != PanelState.Main, settingsMenu);
        settingsManager.ResetAllScrollbarButtons();

        panelState = state;
    }
}
