using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        Debug.Log("Key interacted");
        Destroy(gameObject);
    }
}
