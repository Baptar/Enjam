using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class CustomGrabber : MonoBehaviour
{
    private GameObject interactable;
    [SerializeField] private PlayerPickUp playerPickUp;

    private void OnTriggerEnter(Collider other)
    {
        print("trigger");
        interactable = other.gameObject;
            
        ObjectGrabbable obj = interactable.GetComponent<ObjectGrabbable>();
        if (obj)
        {
            if (obj.canTake) playerPickUp.ShowCanvaInteract(obj.GetText());
            return;
        }
            
        if (interactable.TryGetComponent(out IInteractable interactObj))
        {
            print("touch chair");
            if (interactObj.GetCanTake())
            {
                playerPickUp.ShowCanvaInteract(interactObj.GetTextInteract());
            }
            else
            {
                playerPickUp.ShowCanvaInteract(interactObj.GetTextCantInteract());
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        print("exit");
        if (interactable == other.transform.gameObject)
        {
            interactable = null;
            playerPickUp.ShowCanvaInteract("");
        }
    }

    // Call this method to grab an interactable object
    public void Grab()
    {
        print("grab");
        if (!interactable) return;
        
        playerPickUp.Interact(interactable);
    }

    // Call this method to release the currently held object
    public void Release() 
    {
        print("release");
    }
}
