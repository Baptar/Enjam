using TMPro;
using UnityEngine;

public class RecordingZone : MonoBehaviour
{
    private bool inTheZone = false; // If the player is in the recording zone

    private float timer = 0; // Timer for recording
    private bool isRecording = false; // If the audio is being recorded
    
    private bool displayCanvas; // If the canvas should be displayed
    [SerializeField] private TMP_Text textHold; // Text to display on the canvas

    [SerializeField] private ParcObj parc; // Parc to appear when recording is saved
    [SerializeField] private DSPCapture dspCapture; // DSPCapture to get loudness
    [SerializeField] private AudioRecorder audioRecorder; // AudioRecorder to record audio

    private bool shouldSpawnParc;
    
    // When the player enters the zone
    public void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            inTheZone = true;
            displayCanvas = true;
        }
    }
   
    // When the player exits the zone
    public void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            inTheZone = false;
            displayCanvas = false;
        }
    }
    
    void Update()
    {
        if (timer >= 0)
        {
            // Start record when the player is in the zone and speaks louder than 5 dB
            if (!isRecording && dspCapture.GetLoudness() > 2.5f && inTheZone)
            {
                isRecording = true;
                displayCanvas = false;
                dspCapture.gameObject.SetActive(false);
                //audioRecorder.SetActive(true);
                audioRecorder.StartRecording();
                Debug.Log("RECORDING STARTS");
            }

            // Increase timer if recording
            if (isRecording)
            {
                timer += Time.deltaTime;
            }

            // Appear Parc after 1.5 second
            if (isRecording && timer > 1.5f)
            {
                isRecording = false;
                // Make Parc appears
                parc.MakeParcAppear();
                // Remove Parc collision
                gameObject.GetComponent<CapsuleCollider>().enabled = false;
                Debug.Log("RECORDING STOPS, Time: " + timer);
                timer = -1;
            }

            // show press hold M canvas
            if (displayCanvas)
            {
                textHold.text = "SCREAM IN THE HOLE!";

            }
            // don't show hold M canvas
            else
            {
                textHold.text = "";
            }
        }
    }


    [ContextMenu("Start Parc event")]
    public void StartParcEvent()
    {
        isRecording = false;
        // Make Parc appears
        parc.MakeParcAppear();
        // Remove Parc collision
        gameObject.GetComponent<CapsuleCollider>().enabled = false;
        Debug.Log("RECORDING STOPS, Time: " + timer);
        timer = -1;
    }
}
