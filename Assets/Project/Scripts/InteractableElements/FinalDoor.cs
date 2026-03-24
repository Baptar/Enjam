using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalDoor : MonoBehaviour, IInteractable
{
	public string textInteraction;
    public string textCantInteract;
    public string textDoorInteractable;
    public bool canTake = true;
    public PlayerPickUp playerPickUp;
    [SerializeField] private GameObject start;
    [SerializeField] private GameObject end;
    [SerializeField] private Animator paperAnimation;
    
    public void Interact(PlayerPickUp interactor)
    {
        
        //Debug.Log("Door interacted");
        if (interactor.bHasKey)
        {
            paperAnimation.Play("Door Opened", 0, 0.0f);
            canTake = false;
            GetComponent<BoxCollider>().enabled = false;
            FMODUnity.RuntimeManager.PlayOneShot("event:/DOOR/DOOROpen");
            
            start.gameObject.SetActive(false);
            end.gameObject.SetActive(true);
            //DisplayObject(end.gameObject, true);
            
        }
        else
        {
            FMODUnity.RuntimeManager.PlayOneShot("event:/DOOR/DOORClose");
            //Debug.Log(UnityEngine.Rendering.GraphicsSettings.renderPipelineAsset.GetType().Name);
        }
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
		if (textInteraction == "") return "Press E to open door";
        return textInteraction;
    }

    public void SwitchTextToCanInteract()
    {
        textInteraction = textDoorInteractable;
    }

    private void DisplayObject(GameObject gameobject, bool bShow)
    {
        gameobject.SetActive(bShow);
        // do same thing for childs
        for (int i = 0; i < gameobject.transform.childCount; i++)
        {
            GameObject child = gameobject.transform.GetChild(i).gameObject;
            DisplayObject(child, bShow);
        }
    }
    
    public string GetTextCantInteract()
    {
        return textCantInteract;
    }
}
