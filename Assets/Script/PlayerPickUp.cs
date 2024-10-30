using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IInteractable
{
    public void Interact(PlayerPickUp interactor);
	public string GetTextInteract();
    public string GetTextCantInteract();
    public void SetCanTake(bool canTake);
    public bool GetCanTake();
}

public class PlayerPickUp : MonoBehaviour
{
    [SerializeField] private Transform playerCameraTransform;
    [SerializeField] private Transform objectGrabPointTransform;
    [SerializeField] private LayerMask pickUpLayerMask;
    [SerializeField] private float pickUpDistance = 2.0f;
    [SerializeField] private BoxCollider candyBoxCollider;
    
    // PRESS E VARIABLES
    [SerializeField] private Canvas pressECanvas;
	[SerializeField] private GameObject textInteractObject;

    public bool bHasKey = false;
    public bool bHasPile = false;
    public bool bHasGrabbleObject = false;

    private ObjectGrabbable objectGrabbable;
    
    // Update is called once per frame
    void Update()
    {
        bool displayCanvaInteract = false;
        
        // test if we are casting something
        if (Physics.Raycast(playerCameraTransform.position, playerCameraTransform.forward,
            out RaycastHit raycastHit, pickUpDistance, pickUpLayerMask))
        {
            // we can grab something
            if (raycastHit.transform.TryGetComponent(out objectGrabbable) && !bHasGrabbleObject)
            {
                if (objectGrabbable.canTake)
                {
                    displayCanvaInteract = true;
                    textInteractObject.GetComponent<TMPro.TextMeshProUGUI>().text = objectGrabbable.GetText();
                    
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        bHasGrabbleObject = true;
                        objectGrabbable.Grab(objectGrabPointTransform);
                        if (objectGrabbable.TryGetComponent(out Pile pile))
                        { 
                            bHasPile = true;
                            Debug.Log("Player found pile");
                        }
                    }
                    else
                    { 
                        objectGrabbable = null; // very quick and dirty solution (don't judge)
                    }
                }
                else
                { 
                    objectGrabbable = null; // very quick and dirty solution (don't judge)
                }
            }
            // we can interact with something
            else if (raycastHit.collider.TryGetComponent(out IInteractable interactObject))
            {
                displayCanvaInteract = true;
                if (interactObject.GetCanTake())
                {
                    //displayCanvaInteract = true;
                    textInteractObject.GetComponent<TMPro.TextMeshProUGUI>().text = interactObject.GetTextInteract();

                    if (Input.GetKeyDown(KeyCode.E))
                    { 
                        interactObject.Interact(this);
                        interactObject = null;
                    }
                }
                else
                {
                    textInteractObject.GetComponent<TMPro.TextMeshProUGUI>().text = interactObject.GetTextCantInteract();
                    interactObject = null;
                }
                    
            }
        }
        // show press E canvas
        if (displayCanvaInteract)
        {
            pressECanvas.gameObject.SetActive(true);
        }
        // don't show press E canvas
        else
        {
            pressECanvas.gameObject.SetActive(false);
        }
    }
}