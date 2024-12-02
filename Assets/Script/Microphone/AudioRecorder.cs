using System.Collections;
using UnityEngine;
using FMODUnity;

public class AudioRecorder : MonoBehaviour
{
   // FMOD Code from : https://scottgamesounds.com/c-scripts/
   
    //public variables
    [Header("Choose A Microphone")]
    public int RecordingDeviceIndex = 0;
    [TextArea] public string RecordingDeviceName = null;
    [Header("How Long In Seconds Before Recording Plays")]
    public float Latency = 1f;
    
    public KeyCode ReverbOnOffSwitch;

    //FMOD Objects
    private FMOD.Sound sound;
    private FMOD.CREATESOUNDEXINFO exinfo;
    private FMOD.Channel channel;
    private FMOD.ChannelGroup channelGroup;

    //How many recording devices are plugged in for us to use.
    private int numOfDriversConnected = 0;
    private int numofDrivers = 0;

    //Info about the device we're recording with.
    private System.Guid MicGUID;
    private int SampleRate = 0;
    private FMOD.SPEAKERMODE FMODSpeakerMode;
    private int NumOfChannels = 0;
    private FMOD.DRIVER_STATE driverState;
    
    // Test new Logic
    private AudioClip recordedClip;
    [SerializeField] AudioSource audioSource;


    
    void Start()
    {
        //Step 1: Check to see if any recording devices (or drivers) are plugged in and available for us to use.
        
        RuntimeManager.CoreSystem.getRecordNumDrivers(out numofDrivers, out numOfDriversConnected);

        if (numOfDriversConnected == 0)
            Debug.Log("Hey! Plug a Microhpone in ya dummy!!!");
        else
            Debug.Log("You have " + numOfDriversConnected + " microphones available to record with.");


        //Step 2: Get all of the information we can about the recording device (or driver) that we're
        //        going to use to record with.
        
        RuntimeManager.CoreSystem.getRecordDriverInfo(RecordingDeviceIndex, out RecordingDeviceName, 50,
            out MicGUID, out SampleRate, out FMODSpeakerMode, out NumOfChannels, out driverState);
        

        //Step 3: Store relevant information into FMOD.CREATESOUNDEXINFO variable.

        exinfo.cbsize = System.Runtime.InteropServices.Marshal.SizeOf(typeof(FMOD.CREATESOUNDEXINFO));
        exinfo.numchannels = NumOfChannels;
        exinfo.format = FMOD.SOUND_FORMAT.PCM16;
        exinfo.defaultfrequency = SampleRate;
        exinfo.length = (uint)SampleRate * sizeof(short) * (uint)NumOfChannels * 10;


        //Step 4: Create an FMOD Sound "object". This is what will hold our voice as it is recorded.

        RuntimeManager.CoreSystem.createSound(exinfo.userdata, FMOD.MODE.LOOP_OFF | FMOD.MODE.OPENUSER, 
            ref exinfo, out sound);
        
        //StartRecording();
    }

    public void StartRecording()
    {
        RuntimeManager.CoreSystem.recordStart(RecordingDeviceIndex, sound, true);
        StartCoroutine(StopRec());
    }
    
    public void StopRecording()
    {
        RuntimeManager.CoreSystem.recordStop(RecordingDeviceIndex);
    }
    
    public void PlayRecording()
    {
        RuntimeManager.CoreSystem.playSound(sound, channelGroup, false, out channel);
    }
    
    IEnumerator StopRec()
    {
        yield return new WaitForSeconds(5.0f);

        StopRecording();
    }
}
