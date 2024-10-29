using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour, IInteractable
{
    public void Interact(PlayerPickUp interactor)
    {
        Debug.Log("Key interacted");
        interactor.bHasKey = true;
        Destroy(gameObject);
    }
}
