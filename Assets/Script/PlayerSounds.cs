using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using FMODUnity;
using UnityEngine;

public class PlayerSounds : MonoBehaviour
{
    private FMOD.Studio.EventInstance event_fmod;
    public LayerMask groundLayer;
    private float footstepTime;
    [HideInInspector] public int groundType;
    [SerializeField] float rate;
    [SerializeField] GameObject player;
    [SerializeField] FPSController controller;
    [SerializeField] private List<LightAudioDistance> lightAudioList = new ();
    

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
        if (controller.isWalking)
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
                Debug.Log("Yes");
                if (lightAudioList.Count == 0) return;
                LightAudioDistance closestLightAudio = lightAudioList[0];
                float minDistance = 10000.0f;
                for (int i = 0; i < lightAudioList.Count; i++)
                {
                    float currentDistance =
                        Vector3.Distance(player.transform.position, lightAudioList[i].transform.position);
                    if (currentDistance < minDistance )
                    {
                        minDistance = currentDistance;
                        closestLightAudio = lightAudioList[i];
                    }
                }
                groundType = ((int)soundMaterial.soundMaterial);
                closestLightAudio.PlayAudio(groundType);
            }
        }
    }
}