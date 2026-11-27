using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class SettingsButtonBehavior : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
{
    [SerializeField] SettingsManager settingsManager;
    
    public void OnPointerEnter(PointerEventData e) => SetHighlight(true);
    public void OnPointerExit(PointerEventData e)  => SetHighlight(false);
    public void OnSelect(BaseEventData e)          => SetHighlight(true);
    public void OnDeselect(BaseEventData e)        => SetHighlight(false);

    private ButtonState state;
    
    private void Start()
    {
        state =  ButtonState.Normal;
    }
    
    void SetHighlight(bool hovered)
    {
        if (hovered && InputManager.Instance.IsGamepad)
        {
            settingsManager.ResetAllScrollbarButtons();
        }
    }
}