using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioRecorder : MonoBehaviour
{
    
    private AudioClip recordedClip;
    [SerializeField] AudioSource audioSource;
    
    public void StartRecording()
    {
        string device = Microphone.devices[0];
        int sampleRate = 44100;
        int length = 3599;
        
        recordedClip = Microphone.Start(device, false, length, sampleRate);
    }

    public void PlayRecording()
    {
        audioSource.clip = recordedClip;
        audioSource.Play();
    }
    
    public void StopRecording()
    {
        Microphone.End(Microphone.devices[0]);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            StartRecording();
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            PlayRecording();
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            StopRecording();
        }
    }
}
