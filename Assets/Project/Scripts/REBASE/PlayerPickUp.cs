using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using Sequence = Unity.VisualScripting.Sequence;

/*public class PlayerPickUp : MonoBehaviour
{
    [SerializeField] public FPSController fpsController;
    
    [SerializeField] private float timeToShrink = 2.0f;
    [SerializeField] private Vector3 shrinkScale;
    [SerializeField] private float shrinkSpeed = 0.5f;
    [SerializeField] private float gravityShrink = 4.0f;
    [SerializeField] private GameObject banc;
    [SerializeField] private GameObject rain;
    [SerializeField] private AudioRecorder audioRecorder;
    public GameObject parcExtCollider;
    
    private float startSpeed;
    

    private ObjectGrabbable objectGrabbable;


    void Start()
    {
        startSpeed = fpsController.walkSpeed;
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