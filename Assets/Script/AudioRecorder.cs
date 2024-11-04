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
    
    //Other variables.
    private bool dspEnabled = false;
    private bool playOrPause = true;
    private bool playOkay = false;
    private bool inTheZone = false;

    private float timer = 0;
    private bool isRecording = false;
    [SerializeField] private GameObject textHold;
    //[SerializeField] private Canvas holdMCanvas;
    private bool displayCanvas;

    [SerializeField] private GameObject parc;
    
    // MAIN LOGIC
    
    // When the player enters the zone, the recording will start
    public void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            inTheZone = true;
            displayCanvas = true;
            channel.setPaused(!inTheZone);
            //Debug.Log("IN THE ZONE");
            //RuntimeManager.CoreSystem.recordStart(RecordingDeviceIndex, sound, true);
        }
    }
   
    // When the player exits the zone, the recording will stop
    public void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            inTheZone = false;
            displayCanvas = false;
            channel.setPaused(!inTheZone);
            //Debug.Log("NOT IN THE ZONE");
            //RuntimeManager.CoreSystem.recordStop(RecordingDeviceIndex);
        }
    }
    
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


        //Next we want to create an "FMOD Sound Object", but to do that, we first need to use our 
        //FMOD.CREATESOUNDEXINFO variable to hold and pass information such as the sample rate we're
        //recording at and the num of channels we're recording with into our Sound object.


        //Step 3: Store relevant information into FMOD.CREATESOUNDEXINFO variable.
        

        exinfo.cbsize = System.Runtime.InteropServices.Marshal.SizeOf(typeof(FMOD.CREATESOUNDEXINFO));
        exinfo.numchannels = NumOfChannels;
        exinfo.format = FMOD.SOUND_FORMAT.PCM16;
        exinfo.defaultfrequency = SampleRate;
        exinfo.length = (uint)SampleRate * sizeof(short) * (uint)NumOfChannels * 10;


        //Step 4: Create an FMOD Sound "object". This is what will hold our voice as it is recorded.


        RuntimeManager.CoreSystem.createSound(exinfo.userdata, FMOD.MODE.LOOP_OFF | FMOD.MODE.OPENUSER, 
            ref exinfo, out sound);

        //RuntimeManager.CoreSystem.createChannelGroup("MyChannelGroup", out channelGroup);

        //Step 5: Start recording through our chosen device into our Sound object.


        //RuntimeManager.CoreSystem.recordStart(RecordingDeviceIndex, sound, true);
    }


    void Update()
    {
        
        // Record when R is pressed
        if (!isRecording && Input.GetKeyDown(KeyCode.Space) && inTheZone)
        {
            RuntimeManager.CoreSystem.recordStart(RecordingDeviceIndex, sound, true);
            isRecording = true;
            displayCanvas = false;
            //Debug.Log("RECORDING STARTS");
        }
        
        // Increase counter if recording
        if (isRecording)
        {
            timer += Time.deltaTime;
        }

        // Stop record when R is released
        if (isRecording && (!inTheZone || Input.GetKeyUp(KeyCode.Space)))
        {
            RuntimeManager.CoreSystem.recordStop(RecordingDeviceIndex);
            isRecording = false;
            //Debug.Log("RECORDING STOPS, Time: " + timer);
            
            
            if (timer < 1)
            {
                timer = 0;
                displayCanvas = true;
                //Debug.Log("RECORDING CANCELLED");
            }
            else
            {
                //Debug.Log("RECORDING SAVED");
                parc.GetComponent<AppearParc>().ParcAppear();
                this.gameObject.SetActive(false);
            }
        }
        
        // show press hold M canvas
        if (displayCanvas)
        {
            //holdMCanvas.gameObject.SetActive(true);
            textHold.GetComponent<TMPro.TextMeshProUGUI>().text = "hold SPACE to use microphone";
            
        }
        // don't show hold M canvas
        else
        {
            //holdMCanvas.gameObject.SetActive(false);
            textHold.GetComponent<TMPro.TextMeshProUGUI>().text = "";
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            RuntimeManager.CoreSystem.playSound(sound, channelGroup, false, out channel);
            //Debug.Log("PLAYING");
        }
        
        
        /*
        //Optional
        //Step 8: Set a reverb to the Sound object we're recording into and turn it on or off with a new button.
        if (Input.GetKeyDown(ReverbOnOffSwitch))
        {
            FMOD.REVERB_PROPERTIES propOn = FMOD.PRESET.ROOM();
            FMOD.REVERB_PROPERTIES propOff = FMOD.PRESET.OFF();

            dspEnabled = !dspEnabled;

            RuntimeManager.CoreSystem.setReverbProperties(1, ref dspEnabled ? ref propOn : ref propOff);
        }*/

    }
}
