using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TMP_Text interactionText;

    
    public void EnableCrosshair(bool value)
    {
        
    }

    public void SetCanvaTextInteract(string newText) => interactionText.text = newText;

    public void SetPaperText(string newText)
    {
        
    }
}
