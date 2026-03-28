using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

public class LightAudioDistance : MonoBehaviour
{
    public Transform player;
    public float distLight;
    [SerializeField] EventReference FootstepsEvent;
    

    void Update() => LightAudio();

    private void LightAudio() => distLight = Vector3.Distance(gameObject.transform.position, player.position);
    
    public void PlayAudio(int groundType)
    {
        FMODUnity.RuntimeManager.StudioSystem.setParameterByName("Ground", groundType);
        FMODUnity.RuntimeManager.PlayOneShotAttached(FootstepsEvent, gameObject);
    }
}
