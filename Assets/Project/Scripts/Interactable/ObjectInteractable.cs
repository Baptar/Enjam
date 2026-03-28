using UnityEngine;
using UnityEngine.Events;

public class ObjectInteractable : MonoBehaviour
{
    [SerializeField] protected bool bInteractable = true;
    [SerializeField] protected string textInteraction = "Press E to Interact";
    [SerializeField] protected string textCantInteract;
    [SerializeField] protected string eventSoundOnInteract; // test later with EventReference
    [SerializeField] protected UnityEvent eventOnInteract;
    [SerializeField] protected UnityEvent eventOnInteractButNotInteractable;


    public virtual void Interact()
    {
        if (GetInteractable())
        {
            SetInteractable(false);
            if (eventSoundOnInteract != "") PlaySound(eventSoundOnInteract);
            eventOnInteract?.Invoke();
        }
        else eventOnInteractButNotInteractable?.Invoke();
    }

    public string GetTextInteract() => textInteraction == "" ? "Press E to Interact" : textInteraction;

    public string GetTextCantInteract() => textCantInteract;

    public virtual bool GetInteractable() => bInteractable;
    
    public void SetInteractable(bool value) => bInteractable = value;

    public void PlaySound(string eventLocName) => MainManager.instance.AudioManager.PlaySound(eventLocName, transform);

}
