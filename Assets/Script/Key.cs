using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour, IInteractable
{

	public string textInteraction;

    public void Interact(PlayerPickUp interactor)
    {
        Debug.Log("Key interacted");
        interactor.bHasKey = true;
        Destroy(gameObject);
    }

	public string GetText()
    {
		if (textInteraction == "") return "Press E to Interact";
        return textInteraction;
    }
}
