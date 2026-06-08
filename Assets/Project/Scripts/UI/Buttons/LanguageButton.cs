using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanguageButton : MonoBehaviour
{
    [SerializeField] private MenuManager menuManager;

    public void Language()
    {
        menuManager.SetPanel(PanelState.SettingsLanguage);
    }
}
