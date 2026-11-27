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
    
    public void PlaySound(string eventLocName)
    {
        if (eventLocName !="") FMODUnity.RuntimeManager.PlayOneShot(eventLocName);
    }
    
    public void PlayerSoundTocLittleDoor(Transform soundTransform)
    {
        //PlaySound("event:/Hall/DoorToc1ActiveTrig", soundTransform);
        KnockMarkerRelay.Play("event:/Hall/Knock/SFX_DoorKnockExperiment", soundTransform);
    }
    
    public void PlayerSoundTocHard(Transform soundTransform)
    {
        //PlaySound("event:/Hall/BigDoorTocActiveTrig", soundTransform);
        //KnockMarkerRelay.Play("event:/Hall/DoorToc1ActiveTrig", soundTransform);
        KnockMarkerRelay.Play("event:/Hall/Knock/SFX_DoorBIGKnockExperiment", soundTransform);

    }

    public void StopSoundTocLittle(Transform soundTransform)
    {
        PlaySound("event:/Hall/DoorToc1NoneTrig", soundTransform);
        PlaySound("event:/Hall/DoorToc2NoneTrig", soundTransform);
        PlaySound("event:/Hall/DoorToc4NoneTrig", soundTransform);
        PlaySound("event:/Hall/Knock/SFX_DoorStop", soundTransform);
    }
    
    public void StopSoundTocHard(Transform soundTransform)
    {
        PlaySound("event:/Hall/BigDoorTocNoneTrig", soundTransform);
        PlaySound("event:/Hall/Knock/SFX_DoorStop", soundTransform);
    }
    
    private void PlayerSoundPaperSound(Transform soundTransform)
    {
        PlaySound("event:/Hall/PaperAppear", soundTransform);
    }
}
