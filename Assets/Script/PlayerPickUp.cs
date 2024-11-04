using System;
using System.Collections;
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
    [SerializeField] private FPSController fpsController;
    [SerializeField] private Transform playerCameraTransform;
    [SerializeField] public Transform objectGrabPointTransform;
    [SerializeField] private LayerMask pickUpLayerMask;
    [SerializeField] private float pickUpDistance = 2.0f;
    [SerializeField] private float timeToShrink = 2.0f;
    [SerializeField] private Vector3 shrinkScale;
    [SerializeField] private float shrinkSpeed = 0.5f;
    [SerializeField] private Pile pile;

    [SerializeField] private Beer beer;
    
    // PRESS E VARIABLES
	[SerializeField] private GameObject textInteractObject;
    
    [SerializeField] public Canvas paperCanvas;
    [SerializeField] private GameObject textPaper;

    public bool bHasKey;
    public bool bHasBeer;
    public bool bHasPile;
    public bool bHasPelle;
    public bool bHasCandy;
    public bool bHasGrabbleObject;
    public bool bIsReading;
    public bool bInBancZone;

    public int door1Number = 1;
    public int door2Number = 1;
    
    public bool isDoor1 = true;

    private float startSpeed;
    
    [SerializeField] public PaperCandy paperCandy;
    [SerializeField] public PaperParcFell paperParcFell;
    [SerializeField] public PaperChillBeer paperChillBeer;
    
    [SerializeField] public PaperIndice paperIndice;
    [SerializeField] public PaperLookInfo paperLookInfo;
    

    private ObjectGrabbable objectGrabbable;


    void Start()
    {
        startSpeed = fpsController.walkSpeed;
    }
    
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
                            if (objectGrabbable.TryGetComponent(out Pile _pile))
                            { 
                                bHasPile = true;
                                _pile.OnGrabPile(); 
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
                            else if (objectGrabbable.TryGetComponent(out Beer _))
                            {
                                bHasBeer = true;
                                Debug.Log("Player found beer");
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



    public void MoveVision(Transform newTransform, float timeBetweenChangeVision)
    {
        DisableCollision();
        fpsController.canMove = false;
        Debug.Log("Move VIsion");
        StopAllCoroutines();
        StartCoroutine(MovePlayer(newTransform, timeBetweenChangeVision));
    }
    
    
    IEnumerator MovePlayer(Transform newTransform, float timeBetweenChangeVision)
    {
        float timeElapsed = 0;
        Vector3 startPosition = transform.position;
        Quaternion startRotation = transform.rotation;
        Quaternion cameraRot = fpsController.playerCamera.transform.localRotation;

        while (timeElapsed < timeBetweenChangeVision)
        {
            transform.position = Vector3.Lerp(startPosition, newTransform.position, timeElapsed / timeBetweenChangeVision);
            transform.rotation = Quaternion.Lerp(startRotation, newTransform.rotation, timeElapsed / timeBetweenChangeVision);
            fpsController.playerCamera.transform.localRotation = Quaternion.Lerp(cameraRot, Quaternion.Euler(30, 0, 0), timeElapsed / timeBetweenChangeVision);
            //fpsController.playerCamera.transform.rotation = Quaternion.LookRotation(newTransform.forward, Vector3.up);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        beer.BeerAnimation();
    }

    public void DisableCollision()
    {
        GetComponent<Collider>().enabled = false;
    }

    public void OnBeerDrunken()
    {
        Debug.Log("Beer Drunken");
        fpsController.canMove = true;
        StartCoroutine(Shrink());
    }

    IEnumerator Shrink()
    {
        float timeElapsed = 0;
        Vector3 startScale = transform.localScale;
        float walkSpeedStart = fpsController.walkSpeed;

        while (timeElapsed < timeToShrink)
        {
            transform.localScale = Vector3.Lerp(startScale, shrinkScale, timeElapsed / timeToShrink);
            fpsController.walkSpeed = Mathf.Lerp(walkSpeedStart, shrinkSpeed, timeElapsed / timeToShrink);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        pile.gameObject.SetActive(true);
    }

    public void OnPileTaken()
    {
        StartCoroutine(Grow());
    }
    
    IEnumerator Grow()
    {
        float timeElapsed = 0;
        Vector3 startScale = transform.localScale;
        float walkSpeedStart = fpsController.walkSpeed;
        while (timeElapsed < timeToShrink)
        {
            transform.localScale = Vector3.Lerp(startScale, Vector3.one, timeElapsed / timeToShrink);
            fpsController.walkSpeed = Mathf.Lerp(walkSpeedStart, startSpeed, timeElapsed / timeToShrink);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
    }
}