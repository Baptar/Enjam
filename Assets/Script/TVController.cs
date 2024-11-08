using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TVController : MonoBehaviour, IInteractable
{
    public GameObject TV;
    public bool canTake = true;
    public string textInteraction;
    public string textCantInteract;
    public PlayerPickUp playerPickUp;
    public Pile pile;
    
    [SerializeField] private GameObject[] objectToSpawn;
    [SerializeField] private GameObject objectToDespawn;
    [SerializeField] private GameObject Banc;
    
    
    public void Interact(PlayerPickUp interactor)
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/Salon/InTeleComd", transform.position);
        canTake = false;
        textCantInteract = "";
        pile.OnPileTaken();
        Debug.Log("TV Controller interacted");
        if (TV.TryGetComponent(out CaptureIRLVideo tv)) tv.WatchTv();

        for (int i = 0; i < objectToSpawn.Length; i++)
        {
            objectToSpawn[i].SetActive(true);
        }
        objectToDespawn.SetActive(false);
        Banc.layer = LayerMask.NameToLayer("Ignore Raycast");   
    }
    
    public void SetCanTake(bool canTake)
    {
        this.canTake = canTake;
    }
    
    public bool GetCanTake()
    {
        return playerPickUp.bHasPile && canTake;
    }
    
    public string GetTextInteract()
    {
        if (textInteraction == "") return "Press E to Interact";
        return textInteraction;
    }
    
    public string GetTextCantInteract()
    {
        return textCantInteract;
    }
}
