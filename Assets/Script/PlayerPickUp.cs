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

    private ObjectGrabbable objectGrabbable;
    
    // Update is called once per frame
    void Update()
    {
        bool displayCanvaInteract = false;
        
        // we dont grab anything and we can grab something
        if (objectGrabbable == null) {
            // test if we are casting something
            if (Physics.Raycast(playerCameraTransform.position, playerCameraTransform.forward,
                    out RaycastHit raycastHit, pickUpDistance, pickUpLayerMask))
            {
                // we can grab something
                if (raycastHit.transform.TryGetComponent(out objectGrabbable))
                {
                    if (objectGrabbable.canTake)
                    {
                        displayCanvaInteract = true;
                        textInteractObject.GetComponent<TMPro.TextMeshProUGUI>().text = objectGrabbable.GetText();
                    
                        if (Input.GetKeyDown(KeyCode.E))
                        {
                            objectGrabbable.Grab(objectGrabPointTransform);
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
        }
        // we are grabbing something
        else if (Input.GetKeyDown(KeyCode.E))
        {
            objectGrabbable.Drop();
            objectGrabbable = null;
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
        

        
        // OLD CODE (keep it just in case)
        /*
        if (Input.GetKeyDown(KeyCode.E))
        {
            // we dont grab anything
            if (objectGrabbable == null)
            {
                if (Physics.Raycast(playerCameraTransform.position, playerCameraTransform.forward, out RaycastHit raycastHit, pickUpDistance, pickUpLayerMask))
                {
                    Debug.Log(raycastHit.transform);
                    if (raycastHit.transform.TryGetComponent(out objectGrabbable))
                    {
                        objectGrabbable.Grab(objectGrabPointTransform);
                    }
                    else if (raycastHit.collider.TryGetComponent(out IInteractable interactObject))
                    {
                        interactObject.Interact(this);
                    }
                }
            }
            // we are Grabbing something
            else
            {
                objectGrabbable.Drop();
                objectGrabbable = null;
            }
        }*/
    }
}