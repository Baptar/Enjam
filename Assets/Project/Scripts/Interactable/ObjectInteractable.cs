using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization;

public class ObjectInteractable : MonoBehaviour
{
    [Space(5)]
    [Header("Global Settings")]
    [SerializeField] protected bool bInteractable = true;
    [SerializeField] protected LocalizedString localizedTextInteraction = new LocalizedString { TableReference = "InteractionsTable", TableEntryReference = "interaction_default" };
    [SerializeField] protected LocalizedString localizedTextCantInteract;
    private string textInteraction = "";
    private string textCantInteract = "";
    [SerializeField] protected FMODUnity.EventReference eventSoundOnInteract; // test later with EventReference
    
    [SerializeField] protected UnityEvent eventOnStart;
    [SerializeField] protected UnityEvent eventOnInteract;
    [SerializeField] protected UnityEvent eventOnInteractButNotInteractable;

    protected virtual void Start() => eventOnStart?.Invoke();

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
    public string GetTextInteract() => textInteraction == "" ? localizedTextInteraction.GetLocalizedString() : textInteraction;

    public string GetTextCantInteract() => localizedTextCantInteract.IsEmpty ? textCantInteract : localizedTextCantInteract.GetLocalizedString();

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
