using System;
using System.Collections;
using UnityEngine;
using FMODUnity;

public class DSPCapture : MonoBehaviour
{
    [Header("Choose a Microphone")]
    public int RecordingDeviceIndex = 0;
    [TextArea] public string RecordingDeviceName = null;
    [Header("How Long In Seconds Before Recording Plays")]
    public float Latency = 1f;
    [Header("Choose A Key To Play/Pause/Add To Reverb To Recording")]
    public KeyCode PlayAndPause;
    public KeyCode ReverbOnOffSwitch;
    [Header("Volume")]
    public float volume;

    // FMOD Objects
    private FMOD.Sound sound;
    private FMOD.CREATESOUNDEXINFO exinfo;
    private FMOD.Channel channel;
    private FMOD.ChannelGroup channelGroup;
    private FMOD.Studio.EventInstance micEventInstance;

    // Recording info
    private int numOfDriversConnected = 0;
    private int numOfDrivers = 0;
    private System.Guid MicGUID;
    private int SampleRate = 0;
    private FMOD.SPEAKERMODE fmodSpeakerMode;
    private int NumOfChannels = 0;
    private FMOD.DRIVER_STATE driverState;
    private FMOD.DSP playerDSP;


    // Start is called before the first frame update
    void Start()
    {
        // Check if any recording devices are connected
        RuntimeManager.CoreSystem.getRecordNumDrivers(out numOfDrivers, out numOfDriversConnected);

        if (numOfDriversConnected == 0)
        {
            Debug.Log("<color=#FF00FF>Hey! Plug a Mic in ya dummy</color>");
            return;
        }
        else
        {
            Debug.Log("<color=#FF00FF>You have " + numOfDriversConnected + " microphones available to record with</color>");
        }

        // Get information about the recording device
        RuntimeManager.CoreSystem.getRecordDriverInfo(
            RecordingDeviceIndex,
            out RecordingDeviceName,
            50,
            out MicGUID,
            out SampleRate,
            out fmodSpeakerMode,
            out NumOfChannels,
            out driverState);

        // Set up the sound info
        exinfo.cbsize = System.Runtime.InteropServices.Marshal.SizeOf(typeof(FMOD.CREATESOUNDEXINFO));
        exinfo.numchannels = NumOfChannels;
        exinfo.format = FMOD.SOUND_FORMAT.PCM16;
        exinfo.defaultfrequency = SampleRate;
        exinfo.length = (uint)SampleRate * sizeof(short) * (uint)NumOfChannels;

        // Create the sound object (used for recording only)
        RuntimeManager.CoreSystem.createSound(exinfo.userdata, FMOD.MODE.LOOP_NORMAL | FMOD.MODE.OPENUSER, ref exinfo, out sound);

        // Start recording from the microphone
        RuntimeManager.CoreSystem.recordStart(RecordingDeviceIndex, sound, true);

        StartCoroutine(Wait());
    }
    IEnumerator Wait()
    {
        yield return new WaitForSeconds(Latency);

        RuntimeManager.CoreSystem.createChannelGroup("Recording", out channelGroup);
        RuntimeManager.CoreSystem.playSound(sound, channelGroup, true, out channel);


        Debug.Log("Ready to play");
    }

    public void PlayBackSound(bool play,float volume)
    {
        channel.setVolume(volume);
        channel.setPaused(play);
    }

    public float GetLoudness()
    {
        // Get the recording position and buffer size
        uint recordPos = 0;
        RuntimeManager.CoreSystem.getRecordPosition(RecordingDeviceIndex, out recordPos);

        // Get the current audio data
        float rms = 0f;
        IntPtr soundData;
        sound.@lock(0, exinfo.length, out soundData, out IntPtr _, out uint len1, out uint _);

        // Calculate RMS (Root Mean Square) for loudness
        short[] buffer = new short[len1 / sizeof(short)];
        System.Runtime.InteropServices.Marshal.Copy(soundData, buffer, 0, buffer.Length);

        foreach (short sample in buffer)
        {
            rms += sample * sample;
        }

        rms = Mathf.Sqrt(rms / buffer.Length) / 32768f;
        sound.unlock(soundData, IntPtr.Zero, len1, 0);

        return rms * 100f; // Scale to a more usable range
    }
    
    void Update()
    {
        //Debug.Log("Loudness: " + GetLoudness());
    }
}