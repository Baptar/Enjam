using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonBehavior : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
{
    [SerializeField] TMP_Text text;
    [SerializeField] ColorVariable normalColor;
    [SerializeField] ColorVariable highlightColor;
    
    public void OnPointerEnter(PointerEventData e) => SetHighlight(true);
    public void OnPointerExit(PointerEventData e)  => SetHighlight(false);
    public void OnSelect(BaseEventData e)          => SetHighlight(true);
    public void OnDeselect(BaseEventData e)        => SetHighlight(false);

    private ButtonState state;
    
    private void Start()
    {
        state =  ButtonState.Normal;
        text.color = normalColor.Color;
    }
    
    void SetHighlight(bool hovered)
    {
        if (hovered && state ==  ButtonState.Normal)
        {
            state =  ButtonState.Hovered;
            text.color = highlightColor.Color;
            if (EventSystem.current.currentSelectedGameObject != gameObject)
                EventSystem.current.SetSelectedGameObject(gameObject);  
        }
        else if (!hovered && state == ButtonState.Hovered)
        {
            text.color = normalColor.Color;
            state =  ButtonState.Normal;
        }
    }
}
