
using Unity.VisualScripting;
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
    
    // PRESS E VARIABLES
	[SerializeField] private GameObject textInteractObject;
    
    [SerializeField] public Canvas paperCanvas;
    [SerializeField] private GameObject textPaper;

    public bool bHasKey;
    public bool bHasPile;
    public bool bHasPelle;
    public bool bHasCandy;
    public bool bHasGrabbleObject;
    public bool bIsReading;

    public int door1Number = 1;
    public int door2Number = 1;
    
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
                            if (objectGrabbable.TryGetComponent(out Pile _))
                            { 
                                bHasPile = true;
                                Debug.Log("Player found pile");
                            }
                            else if (objectGrabbable.TryGetComponent(out Candy _))
                            { 
                                bHasCandy = true;
                                Debug.Log("Player found Candy");
                            }
                            else if (objectGrabbable.TryGetComponent(out Pelle _pelle))
                            { 
                                bHasPelle = true;
                                Debug.Log("Player found Pelle");
                                _pelle.PelleAccessible();
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
                    switch (door1Number)
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
                    switch (door2Number)
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
        if (!displayCanvaInteract)
        {
            textInteractObject.GetComponent<TMPro.TextMeshProUGUI>().text = "";
        }
    }

    public void SetPaperText(string text)
    {
        textPaper.GetComponent<TMPro.TextMeshProUGUI>().text = text;
    }
}