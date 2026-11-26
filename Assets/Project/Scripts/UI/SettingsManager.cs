using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    [Header("Sensibility")]
    [SerializeField] private Scrollbar sensibilityScrollbar;
    [SerializeField] private GameObject sensibilityScrollbarContainer;
    
    [Header("Volume")]
    [SerializeField] private Button sensibilityButton;
    [SerializeField] private Button languageButton;
    [SerializeField] private Button volumeSFXButton;
    [SerializeField] private Button volumeAmbButton;
    [SerializeField] private Scrollbar volumeSFXScrollbar;
    [SerializeField] private GameObject volumeSFXScrollbarContainer;
    [SerializeField] private Scrollbar volumeAmbScrollbar;
    [SerializeField] private GameObject volumeAmbScrollbarContainer;
    
    [Header("Language")]
    [SerializeField] private TMP_Text languageText;
    [SerializeField] private LocalizedString languageTextKey;
    [SerializeField] private LocalizedString englishTextKey;
    [SerializeField] private LocalizedString frenchTextKey;
    [SerializeField] private LocalizedString spanishTextKey;
    
    [Header("Vsync")]
    [SerializeField] private TMP_Text vsyncText;
    [SerializeField] private LocalizedString vsyncTextKey;
    [SerializeField] private LocalizedString enabledTextKey;
    [SerializeField] private LocalizedString disabledTextKey;
    
    [Header("Texture quality")]
    [SerializeField] private TMP_Text textureQualityText;
    [SerializeField] private LocalizedString textureQualityTextKey;
    [SerializeField] private LocalizedString highQualityTextKey;
    [SerializeField] private LocalizedString mediumQualityTextKey;
    [SerializeField] private LocalizedString lowQualityTextKey;
    
    [Header("Display mode")]
    [SerializeField] private TMP_Text displayModeText;
    [SerializeField] private LocalizedString displayModeTextKey;
    [SerializeField] private LocalizedString fullscreenTextKey;
    [SerializeField] private LocalizedString fullscreenWindowedTextKey;
    [SerializeField] private LocalizedString windowedTextKey;

    private float sensibility = 0.5f;
    private bool vsyncEnabled = true;
    private int language = 0;
    private int textureQuality = 0;
    private int displayMode = 0;

    public float Sensibility => (sensibility * 2) > 0.1f ? sensibility * 2 : 0.1f ;

    private IEnumerator Start()
    {
        yield return LocalizationSettings.InitializationOperation;

        LoadSettings();
        SetAllButtonsText();
    }
    
    #region ButtonCalls

    public void SensibilityButton()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/UI/UI_Click");
        InputManager.Instance.SetSelected(sensibilityScrollbar.gameObject);
        
        // Update language button navigation
        Navigation nav = volumeSFXButton.navigation;
        nav.selectOnUp = sensibilityScrollbar;
        volumeSFXButton.navigation = nav;
        
        sensibilityScrollbarContainer.SetActive(true);
        sensibilityButton.gameObject.SetActive(false);
        
        ResetVolumeSFXScrollbarButton();
        ResetVolumeAmbScrollbarButton();
    }

    public void VolumeSFXButton()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/UI/UI_Click");
        InputManager.Instance.SetSelected(volumeSFXScrollbar.gameObject);
        
        // Update sensibility button navigation
        Navigation nav = sensibilityButton.navigation;
        nav.selectOnDown = volumeSFXScrollbar;
        sensibilityButton.navigation = nav;
        
        // Update volume ambiance button navigation
        nav = volumeAmbButton.navigation;
        nav.selectOnUp = volumeSFXScrollbar;
        volumeAmbButton.navigation = nav;
        
        volumeSFXScrollbarContainer.SetActive(true);
        volumeSFXButton.gameObject.SetActive(false);
        
        ResetSensibilityScrollbarButton();
        ResetVolumeAmbScrollbarButton();
    }
    
    public void VolumeAmbButton()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/UI/UI_Click");
        InputManager.Instance.SetSelected(volumeAmbScrollbar.gameObject);
        
        // Update volume sfx button navigation
        Navigation nav = volumeSFXButton.navigation;
        nav.selectOnDown = volumeAmbScrollbar;
        volumeSFXButton.navigation = nav;
        
        // Update language navigation
        nav = languageButton.navigation;
        nav.selectOnUp = volumeAmbScrollbar;
        languageButton.navigation = nav;
        
        volumeAmbScrollbarContainer.SetActive(true);
        volumeAmbButton.gameObject.SetActive(false);
        
        ResetSensibilityScrollbarButton();
        ResetVolumeSFXScrollbarButton();
    }

    public void OnSensibilityChanged()
    {
        PlayerPrefs.SetFloat("Sensibility", sensibilityScrollbar.value);
        sensibility = sensibilityScrollbar.value;
        if (!InputManager.Instance.IsGamepad) return;
        FMODUnity.RuntimeManager.PlayOneShot("event:/UI/UI_Click");
    }
    
    public void OnVolumeSFXChanged()
    {
        PlayerPrefs.SetFloat("VolumeSFX", volumeSFXScrollbar.value);
        if (!InputManager.Instance.IsGamepad) return;
        FMODUnity.RuntimeManager.PlayOneShot("event:/UI/UI_Click");
    }
    
    public void OnVolumeAmbChanged()
    {
        PlayerPrefs.SetFloat("VolumeAmb", volumeAmbScrollbar.value);
        if (!InputManager.Instance.IsGamepad) return;
        FMODUnity.RuntimeManager.PlayOneShot("event:/UI/UI_Click");
    }
    
    public void OnSensibilityReleased()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/UI/UI_Click");
        PlayerPrefs.SetFloat("Sensibility", sensibilityScrollbar.value);
        sensibility = sensibilityScrollbar.value;
        PlayerPrefs.Save();
    }

    public void OnVolumeSFXReleased()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/UI/UI_Click");
        PlayerPrefs.SetFloat("VolumeSFX", volumeSFXScrollbar.value);
        PlayerPrefs.Save();
        
        // MAEL : update son
    }
    
    public void OnVolumeAmbReleased()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/UI/UI_Click");
        PlayerPrefs.SetFloat("VolumeAmb", volumeAmbScrollbar.value);
        PlayerPrefs.Save();
        
        // MAEL : update son
    }

    public void LanguageButton()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/UI/UI_Click");
        InputManager.Instance.SetSelected(languageText.gameObject);
        language = (language + 1) % 3;
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[language];
        SetAllButtonsText();
        SaveSettings();
        ResetAllScrollbarButtons();
    }
    
    public void VSyncButton()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/UI/UI_Click");
        InputManager.Instance.SetSelected(vsyncText.gameObject);
        vsyncEnabled = !vsyncEnabled;
        QualitySettings.vSyncCount = vsyncEnabled ? 1 : 0;
        SetVsyncButtonText();
        SaveSettings();
        ResetAllScrollbarButtons();
    }

    public void TextureQualityButton()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/UI/UI_Click");
        InputManager.Instance.SetSelected(textureQualityText.gameObject);
        textureQuality = (textureQuality + 1) % 3;
        
        QualitySettings.SetQualityLevel(textureQuality, true);
        
        SetTextureQualityButtonText();
        SaveSettings();
        ResetAllScrollbarButtons();
    }
    
    private void ApplyDisplayMode()
    {
        switch (displayMode)
        {
            case 0:
                Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
                break;

            case 1:
                Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
                break;

            case 2:
                Screen.fullScreenMode = FullScreenMode.Windowed;
                break;
        }
    }

    public void DisplayModeButton()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/UI/UI_Click");
        InputManager.Instance.SetSelected(displayModeText.gameObject);
        displayMode = (displayMode + 1) % 3;
        
        ApplyDisplayMode();
        
        SetDisplayModeButtonText();
        SaveSettings();
        ResetAllScrollbarButtons();
    }
    
    #endregion
    
    #region UpdateButtonText

    private void SetLanguageButtonText()
    {
        languageText.text = languageTextKey.GetLocalizedString();
        languageText.text += " ";

        string l = language == 0 ? englishTextKey.GetLocalizedString() :
            language == 1 ? frenchTextKey.GetLocalizedString() : spanishTextKey.GetLocalizedString();
        
        languageText.text += l;
    }
    
    private void SetVsyncButtonText()
    {
        vsyncText.text = vsyncTextKey.GetLocalizedString();
        vsyncText.text += " ";
        vsyncText.text += vsyncEnabled ? enabledTextKey.GetLocalizedString() : disabledTextKey.GetLocalizedString();
    }

    private void SetTextureQualityButtonText()
    {
        textureQualityText.text = textureQualityTextKey.GetLocalizedString();
        textureQualityText.text += " ";

        string l = textureQuality == 0 ? highQualityTextKey.GetLocalizedString() :
            textureQuality == 1 ? mediumQualityTextKey.GetLocalizedString() : lowQualityTextKey.GetLocalizedString();
        
        textureQualityText.text += l;
    }

    private void SetDisplayModeButtonText()
    {
        displayModeText.text = displayModeTextKey.GetLocalizedString();
        displayModeText.text += " ";

        string l = displayMode == 0 ? fullscreenTextKey.GetLocalizedString() :
            displayMode == 1 ? fullscreenWindowedTextKey.GetLocalizedString() : windowedTextKey.GetLocalizedString();
        
        displayModeText.text += l;
    }

    public void SetAllButtonsText()
    {
        SetLanguageButtonText();
        SetVsyncButtonText();
        SetTextureQualityButtonText();
        SetDisplayModeButtonText();
    }
    
    private void ResetSensibilityScrollbarButton()
    {
        // Update language button navigation
        Navigation nav = volumeSFXButton.navigation;
        nav.selectOnUp = sensibilityButton;
        volumeSFXButton.navigation = nav;
        
        sensibilityScrollbarContainer.SetActive(false);
        sensibilityButton.gameObject.SetActive(true);
    }
    
    private void ResetVolumeSFXScrollbarButton()
    {
        // Update sensibility button navigation
        Navigation nav = sensibilityButton.navigation;
        nav.selectOnDown = volumeSFXButton;
        sensibilityButton.navigation = nav;
        
        // Update language button navigation
        nav = volumeAmbButton.navigation;
        nav.selectOnUp = volumeSFXButton;
        volumeAmbButton.navigation = nav;
        
        volumeSFXScrollbarContainer.SetActive(false);
        volumeSFXButton.gameObject.SetActive(true);
    }
    
    private void ResetVolumeAmbScrollbarButton()
    {
        // Update volume sfx button navigation
        Navigation nav = volumeSFXButton.navigation;
        nav.selectOnDown = volumeAmbButton;
        volumeSFXButton.navigation = nav;
        
        // Update language button navigation
        nav = languageButton.navigation;
        nav.selectOnUp = volumeAmbButton;
        languageButton.navigation = nav;
        
        volumeAmbScrollbarContainer.SetActive(false);
        volumeAmbButton.gameObject.SetActive(true);
    }

    public void ResetAllScrollbarButtons()
    {
        ResetSensibilityScrollbarButton();
        ResetVolumeSFXScrollbarButton();
        ResetVolumeAmbScrollbarButton();
    }
    
    #endregion

    #region SavingSystem
    
    private void SaveSettings()
    {
        PlayerPrefs.SetFloat("Sensibility", sensibilityScrollbar.value);
        PlayerPrefs.SetFloat("VolumeSFX", volumeSFXScrollbar.value);
        PlayerPrefs.SetFloat("VolumeAmb", volumeAmbScrollbar.value);

        PlayerPrefs.SetInt("Language", language);
        PlayerPrefs.SetInt("VSync", vsyncEnabled ? 1 : 0);
        PlayerPrefs.SetInt("TextureQuality", textureQuality);
        PlayerPrefs.SetInt("DisplayMode", displayMode);

        PlayerPrefs.Save();
    }
    
    private void LoadSettings()
    {
        if (!PlayerPrefs.HasKey("SettingsInitialized"))
        {
            // First launch
            sensibility = 0.5f;
            sensibilityScrollbar.value = 0.5f;
            volumeSFXScrollbar.value = 1f;
            volumeAmbScrollbar.value = 1f;
            language = LocalizationSettings.AvailableLocales.Locales.IndexOf(
                LocalizationSettings.SelectedLocale);
            vsyncEnabled = true;
            textureQuality = 0;
            displayMode = 0;

            
            
            PlayerPrefs.SetInt("SettingsInitialized", 1);
            SaveSettings();
        }
        else
        {
            // Sensibility
            sensibilityScrollbar.SetValueWithoutNotify(
                PlayerPrefs.GetFloat("Sensibility", sensibilityScrollbar.value));
            
            sensibility = sensibilityScrollbar.value;

            // Volume SFX
            volumeSFXScrollbar.SetValueWithoutNotify(
                PlayerPrefs.GetFloat("VolumeSFX", volumeSFXScrollbar.value));
            
            // Volume Amb
            volumeAmbScrollbar.SetValueWithoutNotify(
                PlayerPrefs.GetFloat("VolumeAmb", volumeAmbScrollbar.value));

            // Language
            var locales = LocalizationSettings.AvailableLocales.Locales;

            if (locales.Count > 0)
            {
                language = Mathf.Clamp(
                    PlayerPrefs.GetInt("Language",
                        locales.IndexOf(LocalizationSettings.SelectedLocale)),
                    0,
                    locales.Count - 1
                );

                LocalizationSettings.SelectedLocale = locales[language];
            }

            // VSync
            vsyncEnabled = PlayerPrefs.GetInt(
                "VSync", QualitySettings.vSyncCount > 0 ? 1 : 0) == 1;

            QualitySettings.vSyncCount = vsyncEnabled ? 1 : 0;

            // Texture quality
            textureQuality = PlayerPrefs.GetInt(
                "TextureQuality", QualitySettings.GetQualityLevel());

            textureQuality = Mathf.Clamp(
                textureQuality, 0, QualitySettings.names.Length - 1);

            QualitySettings.SetQualityLevel(textureQuality, true);

            // Display mode
            displayMode = PlayerPrefs.GetInt(
                "DisplayMode", (int)Screen.fullScreenMode switch
                {
                    0 => 0,
                    1 => 1,
                    _ => 2
                });

            displayMode = Mathf.Clamp(displayMode, 0, 2);
        }

        // Apply all settings
        QualitySettings.vSyncCount = vsyncEnabled ? 1 : 0;

        QualitySettings.SetQualityLevel(textureQuality, true);

        ApplyDisplayMode();

        if (LocalizationSettings.AvailableLocales.Locales.Count > 0)
        {
            LocalizationSettings.SelectedLocale =
                LocalizationSettings.AvailableLocales.Locales[language];
        }
    }
    
    #endregion
}
