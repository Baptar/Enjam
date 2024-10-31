using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
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
    [SerializeField] public Transform objectGrabPointTransform;
    [SerializeField] private LayerMask pickUpLayerMask;
    [SerializeField] private float pickUpDistance = 2.0f;
    [SerializeField] private BoxCollider candyBoxCollider;
    
    // PRESS E VARIABLES
    [SerializeField] private Canvas pressECanvas;
	[SerializeField] private GameObject textInteractObject;
    
    [SerializeField] public Canvas paperCanvas;
    [SerializeField] private GameObject textPaper;

    public bool bHasKey = false;
    public bool bHasPile = false;
    public bool bHasPelle = false;
    public bool bHasCandy = false;
    public bool bHasGrabbleObject = false;
    public bool bIsReading = false;

    public int door1number = 1;
    public int door2number = 1;
    
    public bool isDoor1 = true;
    
    [SerializeField] public PaperCandy paperCandy;
    [SerializeField] public PaperParcFell paperParcFell;
    [SerializeField] public PaperChillBeer paperChillBeer;
    
    [SerializeField] public PaperIndice paperIndice;
    [SerializeField] public PaperLookInfo paperLookInfo;
    

    private ObjectGrabbable objectGrabbable;
    
    // Update is called once per frame
    void Update()
    {
        bool displayCanvaInteract = false;

        if (!bIsReading)
        {
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
                            objectGrabbable.OnTook(this);
                            if (objectGrabbable.TryGetComponent(out Pile pile))
                            { 
                                bHasPile = true;
                                Debug.Log("Player found pile");
                            }
                            else if (objectGrabbable.TryGetComponent(out Candy candy))
                            { 
                                bHasCandy = true;
                                Debug.Log("Player found Candy");
                            }
                            else if (objectGrabbable.TryGetComponent(out Pelle pelle))
                            { 
                                bHasPelle = true;
                                Debug.Log("Player found Pelle");
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
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.E))
            { 
                paperCanvas.gameObject.SetActive(false);
                bIsReading = false;
                if (isDoor1)
                {
                    Debug.Log("doo1");
                    switch (door1number)
                    {
                        case 1:
                            paperCandy.OnStopRead();
                            break;
                        case 2:
                            paperParcFell.OnStopRead();
                            break;
                        case 3:
                            paperChillBeer.OnStopRead(this);
                            break;
                    }
                }
                else
                {
                    Debug.Log("doo2");
                    switch (door2number)
                    {
                        case 1:
                            paperLookInfo.OnStopRead();
                            break;
                        case 2:
                            paperIndice.OnStopRead();
                            break;
                    }
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

    public void SetPaperText(string _text)
    {
        textPaper.GetComponent<TMPro.TextMeshProUGUI>().text = _text;
    }
}