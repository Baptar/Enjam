using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ZoneEvents : MonoBehaviour
{
    [SerializeField] private UnityEvent onZoneEnter;
    [SerializeField] private UnityEvent onZoneExit;
    [SerializeField] private string colliderTag = "Player";
    
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(colliderTag)) return;
        onZoneEnter?.Invoke();
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag(colliderTag)) return;
        onZoneExit?.Invoke();
    }
}
