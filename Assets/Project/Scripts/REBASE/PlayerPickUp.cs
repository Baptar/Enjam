using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using Sequence = Unity.VisualScripting.Sequence;

/*public class PlayerPickUp : MonoBehaviour
{
    [SerializeField] public FPSController fpsController;
    [SerializeField] private Transform playerCameraTransform;
    [SerializeField] public Transform objectGrabPointTransform;
    [SerializeField] private LayerMask pickUpLayerMask;
    [SerializeField] private float pickUpDistance = 2.0f;
    [SerializeField] private float timeToShrink = 2.0f;
    [SerializeField] private Vector3 shrinkScale;
    [SerializeField] private float shrinkSpeed = 0.5f;
    [SerializeField] private float gravityShrink = 4.0f;
    [SerializeField] private Pile pile;
    [SerializeField] private GameObject banc;
    [SerializeField] private GameObject rain;
    [SerializeField] private AudioRecorder audioRecorder;
    public GameObject parcExtCollider;

    [SerializeField] private Beer beer;
    
    
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
    

    private ObjectGrabbable objectGrabbable;


    void Start()
    {
        startSpeed = fpsController.walkSpeed;
    }
    
    // Update is called once per frame
    void Update()
    {
        if (bIsReading)
        {
            if (Input.GetKeyDown(KeyCode.E))
            { 
                paperCanvas.gameObject.SetActive(false);
                bIsReading = false;
                if (isDoor1)
                {
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
    }

    public void SetPaperText(string text)
    {
        textPaper.GetComponent<TMPro.TextMeshProUGUI>().text = text;
    }



    public void OnPileTaken()
    {
        parcExtCollider.SetActive(false);
        banc.GetComponent<Collider>().enabled = false;
        StartCoroutine(Grow());
    }
    
    IEnumerator Grow()
    {
        float timeElapsed = 0;
        Vector3 startScale = transform.localScale;
        float walkSpeedStart = fpsController.walkSpeed;
        float gravityStart = fpsController.gravityScale;
        FMODUnity.RuntimeManager.PlayOneShot("event:/Park/GoBig");
        FMODUnity.RuntimeManager.PlayOneShot("event:/MXStop");
        while (timeElapsed < timeToShrink)
        {
            transform.localScale = Vector3.Lerp(startScale, Vector3.one, timeElapsed / timeToShrink);
            fpsController.walkSpeed = Mathf.Lerp(walkSpeedStart, startSpeed, timeElapsed / timeToShrink);
            fpsController.gravityScale = Mathf.Lerp(gravityStart, 9.81f, timeElapsed / timeToShrink);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
    }
}*/