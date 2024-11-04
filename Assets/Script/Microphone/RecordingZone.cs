using UnityEngine;

public class RecordingZone : MonoBehaviour
{
    private bool inTheZone = false; // If the player is in the recording zone

    private float timer = 0; // Timer for recording
    private bool isRecording = false; // If the audio is being recorded
    
    private bool displayCanvas; // If the canvas should be displayed
    [SerializeField] private GameObject textHold; // Text to display on the canvas
    //[SerializeField] private Canvas holdMCanvas; // Old canvas 

    [SerializeField] private GameObject parc; // Parc to appear when recording is saved
    [SerializeField] private GameObject dspCapture; // DSPCapture to get loudness
    [SerializeField] private GameObject audioRecorder; // AudioRecorder to record audio
    
    // When the player enters the zone
    public void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            inTheZone = true;
            displayCanvas = true;
            //Debug.Log("IN THE ZONE");
        }
    }
   
    // When the player exits the zone
    public void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            inTheZone = false;
            displayCanvas = false;
            //Debug.Log("NOT IN THE ZONE");
        }
    }

    void Update()
    {
        if (timer >= 0)
        {

            // Start record when the player is in the zone and speaks louder than 5 dB
            if (!isRecording && dspCapture.GetComponent<DSPCapture>().GetLoudness() > 5 && inTheZone)
            {
                isRecording = true;
                displayCanvas = false;
                dspCapture.SetActive(false);
                audioRecorder.SetActive(true);
                //Debug.Log("RECORDING STARTS");
            }

            // Increase timer if recording
            if (isRecording)
            {
                timer += Time.deltaTime;
            }

            // Stop record after 1 second
            if (isRecording && timer > 1.5f)
            {
                isRecording = false;
                timer = -1;
                audioRecorder.GetComponent<AudioRecorder>().StopRecording();
                parc.GetComponent<AppearParc>().ParcAppear();
                this.gameObject.GetComponent<CapsuleCollider>().enabled = false;
                //Debug.Log("RECORDING STOPS, Time: " + timer);
            }

            // show press hold M canvas
            if (displayCanvas)
            {
                //holdMCanvas.gameObject.SetActive(true);
                textHold.GetComponent<TMPro.TextMeshProUGUI>().text = "SCREAM IN THE HOLE!";

            }
            // don't show hold M canvas
            else
            {
                //holdMCanvas.gameObject.SetActive(false);
                textHold.GetComponent<TMPro.TextMeshProUGUI>().text = "";
            }
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            audioRecorder.GetComponent<AudioRecorder>().PlayRecording();
            //Debug.Log("PLAYING");
        }
    }
}
