using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public void PlaySound(string eventLocName, Transform soundTransform)
    {
        if (eventLocName !="") FMODUnity.RuntimeManager.PlayOneShot(eventLocName, soundTransform.position);
    }
}
