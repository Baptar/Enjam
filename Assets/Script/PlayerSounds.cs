using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using FMODUnity;
using UnityEngine;

public class PlayerSounds : MonoBehaviour
{
    public LayerMask groundLayer;
    private float footstepTime;
    [HideInInspector] public int groundType;
    [HideInInspector] public bool isWalking = true;
    [SerializeField] float rate;
    [SerializeField] EventReference FootstepsEvent;
    [SerializeField] GameObject player;
    [SerializeField] FPSController controller;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        ManageFootSteps();
    }

    void ManageFootSteps()
    {
        footstepTime += Time.deltaTime;
        if (isWalking)
        {
            if (footstepTime >= rate)
            {
                PlayFootStepSound();
            }
        }
    }
    
    public void PlayFootStepSound()
    {
        footstepTime = 0;
        Ray ray = new Ray(transform.position, Vector3.down);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, transform.localScale.y + 0.1f, groundLayer))
        {
            SoundMaterial soundMaterial = hit.transform.GetComponent<SoundMaterial>();
            if (soundMaterial)
            {
                groundType = ((int)soundMaterial.soundMaterial);
                FMODUnity.RuntimeManager.StudioSystem.setParameterByName("Ground", groundType);
                FMODUnity.RuntimeManager.PlayOneShotAttached(FootstepsEvent, player);
            }
        }
    }
}