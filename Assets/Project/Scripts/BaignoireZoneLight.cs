using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

public class BaignoireZoneLight : MonoBehaviour
{
    [SerializeField] private UnityEvent onZoneEnter;
    [SerializeField] private UnityEvent onZoneExit;
    [SerializeField] private string colliderTag = "Player";
    
    [SerializeField] private Volume volumeProfileA;
    [SerializeField] private Volume volumeProfileB;
    [SerializeField] private Light directionalLight;

    private Coroutine transitionCoroutine;
    
    private bool isTriggered;

    void Start()
    {
        isTriggered = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isTriggered) return; // Skip if already entered once
        
        if (!other.CompareTag(colliderTag)) return;
        onZoneEnter?.Invoke();
        
        if (transitionCoroutine != null) StopCoroutine(transitionCoroutine);
        transitionCoroutine = StartCoroutine(SwitchToNightRoutine());

        enabled = false;
        isTriggered = true;
        
        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX_end_lightswitch");
    }

    private IEnumerator SwitchToNightRoutine()
    {
        directionalLight.enabled = false;
        volumeProfileA.weight = 0f;

        yield return null;
        yield return null;

        volumeProfileB.weight = 1f;
    }
}