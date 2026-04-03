using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public void PlaySound(string eventLocName, Transform soundTransform)
    {
        if (eventLocName !="") FMODUnity.RuntimeManager.PlayOneShot(eventLocName, soundTransform.position);
    }
    
    public void PlaySound(FMODUnity.EventReference eventLocName, Transform soundTransform)
    {
        if (!eventLocName.IsNull) FMODUnity.RuntimeManager.PlayOneShot(eventLocName, soundTransform.position);
    }
    
    public void PlayerSoundTocLittle(Transform soundTransform)
    {
        PlaySound("event:/Hall/DoorToc1ActiveTrig", soundTransform);
    }

    private void PlayerSoundPaperSound(Transform soundTransform)
    {
        PlaySound("event:/Hall/PaperAppear", soundTransform);
    }
    
    public void PlayerSoundTocHard(Transform soundTransform)
    {
        PlaySound("event:/Hall/BigDoorTocActiveTrig", soundTransform);
    }

    public void StopSoundTocLittle(Transform soundTransform)
    {
        PlaySound("event:/Hall/DoorToc1NoneTrig", soundTransform);
    }
    
    public void StopSoundTocHard(Transform soundTransform)
    {
        PlaySound("event:/Hall/BigDoorTocNoneTrig", soundTransform);
    }
}
