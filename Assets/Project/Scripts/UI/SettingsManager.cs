using TMPro;
using UnityEngine;
using UnityEngine.Localization;

public class SettingsManager : MonoBehaviour
{
    [SerializeField] private TMP_Text vsyncText;
    [SerializeField] private TMP_Text fullscreenText;
    [SerializeField] private LocalizedString vsyncTextKey;
    [SerializeField] private LocalizedString fullscreenTextKey;
    [SerializeField] private LocalizedString enabledTextKey;
    [SerializeField] private LocalizedString disabledTextKey;

    private bool vsyncEnabled = true;
    private bool fullscreenEnabled = true;

    void Start()
    {
        SetVsyncButtonsText();
        SetFullscreenButtonsText();
        
        vsyncEnabled = QualitySettings.vSyncCount > 0;
        fullscreenEnabled = Screen.fullScreen;
    }

    public void VSyncButton()
    {
        InputManager.Instance.SetSelected(vsyncText.gameObject);
        vsyncEnabled = !vsyncEnabled;
        QualitySettings.vSyncCount = vsyncEnabled ? 1 : 0;
        SetVsyncButtonsText();
    }

    public void FullscreenButton()
    {
        InputManager.Instance.SetSelected(fullscreenText.gameObject);
        fullscreenEnabled = !fullscreenEnabled;
        
        if (fullscreenEnabled)
        {
            // Plein écran exclusif
            Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
        }
        else
        {
            // Plein écran fenêtré sans bordure
            Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
        }
        
        SetFullscreenButtonsText();
    }
    
    public void SetVsyncButtonsText()
    {
        // Main menu
        vsyncText.text = vsyncTextKey.GetLocalizedString();
        vsyncText.text += " ";
        vsyncText.text += vsyncEnabled ? enabledTextKey.GetLocalizedString() : disabledTextKey.GetLocalizedString();
    }

    public void SetFullscreenButtonsText()
    {
        fullscreenText.text = fullscreenTextKey.GetLocalizedString();
        fullscreenText.text += " ";
        fullscreenText.text += fullscreenEnabled ? enabledTextKey.GetLocalizedString() : disabledTextKey.GetLocalizedString();
    }
}
