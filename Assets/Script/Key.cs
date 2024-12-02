using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour, IInteractable
{
	public string textInteraction;
    public string textCantInteract;
    public bool canTake = true;
    [SerializeField] private FinalDoor finalDoor;

    public void Interact(PlayerPickUp interactor)
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/Park/KeyGrab", transform.position);
        interactor.bHasKey = true;
        finalDoor.SwitchTextToCanInteract();
        Destroy(gameObject);
    }
    
    public string GetTextCantInteract()
    {
        return textCantInteract;
    }
    
    public void SetCanTake(bool canTake)
    {
        this.canTake = canTake;
    }
    
    public bool GetCanTake()
    {
        return canTake;
    }

	public string GetTextInteract()
    {
		if (textInteraction == "") return "Press E to Interact";
        return textInteraction;
    }
}
