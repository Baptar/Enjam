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
    
    public void PlayerSoundTocLittleDoor1(Transform soundTransform)
    {
        PlaySound("event:/Hall/DoorToc1ActiveTrig", soundTransform);
    }
    
    public void PlayerSoundTocLittleDoor2(Transform soundTransform)
    {
        PlaySound("event:/Hall/DoorToc2ActiveTrig", soundTransform);
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
        PlaySound("event:/Hall/DoorToc2NoneTrig", soundTransform);
    }
    
    public void StopSoundTocHard(Transform soundTransform)
    {
        PlaySound("event:/Hall/BigDoorTocNoneTrig", soundTransform);
    }
}
