using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TVController : MonoBehaviour, IInteractable
{
    public GameObject blackBoardTV;
    public Image crosshair;
    public string textInteraction;
    public void Interact(PlayerPickUp interactor)
    {
        Debug.Log("TV Controller interacted");
        blackBoardTV.GetComponent<Animator>().SetTrigger("ChangeTV");
        crosshair.enabled = false;
    }
    
    public string GetText()
    {
        if (textInteraction == "") return "Press E to Interact";
        return textInteraction;
    }
}
