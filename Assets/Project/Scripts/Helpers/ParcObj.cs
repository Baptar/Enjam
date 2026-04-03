using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class ParcObj : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private Transform targetParcTransform;
    [SerializeField] private float durationParcAppear = 2.0f;
    [SerializeField] private Ease easeParcAppear = Ease.InOutElastic;
    
    [Space(5)]
    [Header("Shake Settings")]
    [SerializeField] private float shakeStrengthPosition = 1.0f;
    
    [Space(5)]
    [Header("Event")]
    [SerializeField] private UnityEvent onParcAppear;
    
    public void MakeParcAppear()
    {
        MainManager.instance.AudioManager.PlaySound("event:/Park/ParkAppear", transform);
        MainManager.instance.AudioManager.PlaySound("event:/Park/RainNoneTrig", transform);
        Camera playerCam = MainManager.instance.Player.GetPlayerCamera();
        // Shake cam    
        Sequence sequence = DOTween.Sequence();
        sequence.Append(transform.DOMove(targetParcTransform.position, durationParcAppear).SetEase(easeParcAppear))
            .Insert(0.0f, playerCam.transform.DOShakePosition(durationParcAppear, shakeStrengthPosition)).SetEase(easeParcAppear)
            //.Insert(0.0f, playerCam.transform.DOShakeRotation(durationParcAppear, shakeStrengthPosition)).SetEase(easeParcAppear)
            .OnComplete(()=> onParcAppear?.Invoke());
    }
    
    
}
