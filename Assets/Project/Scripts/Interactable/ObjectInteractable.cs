using UnityEngine;
using UnityEngine.Events;

public class ObjectInteractable : MonoBehaviour
{
    [SerializeField] protected bool bInteractable = true;
    [SerializeField] protected string textInteraction = "Press E to Interact";
    [SerializeField] protected string textCantInteract;
    [SerializeField] protected FMODUnity.EventReference eventSoundOnInteract; // test later with EventReference
    
    [SerializeField] protected UnityEvent eventOnStart;
    [SerializeField] protected UnityEvent eventOnInteract;
    [SerializeField] protected UnityEvent eventOnInteractButNotInteractable;

    private void Start() => eventOnStart?.Invoke();

    public virtual void Interact()
    {
        if (GetInteractable())
        {
            SetInteractable(false);
            if (!eventSoundOnInteract.IsNull) PlaySound(eventSoundOnInteract);
            eventOnInteract?.Invoke();
        }
        else eventOnInteractButNotInteractable?.Invoke();
    }

    #region Getter
    public string GetTextInteract() => textInteraction == "" ? "Press E to Interact" : textInteraction;

    public string GetTextCantInteract() => textCantInteract;

    public virtual bool GetInteractable() => bInteractable;
    #endregion
    
    #region Setter
    public void SetTextInteract(string text) => textInteraction = text;
    public void SetTextCantInteract(string text) => textCantInteract = text;
    public void SetInteractable(bool value) => bInteractable = value;
    #endregion

    public void PlaySound(FMODUnity.EventReference eventLocName) => MainManager.instance.AudioManager.PlaySound(eventLocName, transform);
    public void PlaySound(string eventLocName) => MainManager.instance.AudioManager.PlaySound(eventLocName, transform);

}
