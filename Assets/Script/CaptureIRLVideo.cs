using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class CaptureIRLVideo : MonoBehaviour
{
    WebCamTexture webCamTexture;
    public Image crosshair;
    public GameObject blackBoardTV;
    public GameObject blackBoardPlayer;
    [SerializeField] public int numberDevice;
    [SerializeField] private Camera cam;
    [SerializeField] private float delayWatchParc = 5f;
    [SerializeField] private float delayWatchIrl = 5f;
    [SerializeField] private FPSController fpsController;
    
    private FMOD.Studio.EventInstance eventFMOD;

    void Start()
    {
        eventFMOD = FMODUnity.RuntimeManager.CreateInstance("event:/Salon/Tele");
        
        //if (WebCamTexture.devices.Length < numberDevice + 1) return;
        
        WebCamDevice device = WebCamTexture.devices[numberDevice];
        Debug.Log("Webcam détectée : " + WebCamTexture.devices[0].name);
        webCamTexture = new WebCamTexture(device.name);
    }
    
    public void StartTvirl()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/Salon/TeleStateToCamTrig");
        if(!webCamTexture.isPlaying) webCamTexture.Play();
        GetComponent<Renderer>().material.mainTexture = webCamTexture;
    }

    public void StartTVParc()
    {
        cam.GetComponent<Camera>().enabled = true;
        GetComponent<Renderer>().material.mainTexture = cam.targetTexture;
    }

    public void WatchTv()
    {
        //GetComponent<Renderer>().material = null;
        StartTVParc();
        FMODUnity.RuntimeManager.PlayOneShot("event:/Salon/TeleStateToCamTrig");
        blackBoardTV.GetComponent<Animator>().SetTrigger("ChangeTV");
        crosshair.enabled = false;
    }

    IEnumerator StartWatchTV()
    {
        //StartTVParc();
        yield return new WaitForSeconds(delayWatchParc);
        if (WebCamTexture.devices.Length <= 0)
        {
            StartTvirl();
            yield return new WaitForSeconds(delayWatchIrl);
        }
        
        blackBoardPlayer.GetComponent<Animator>().SetTrigger("ChangePlayer");
        crosshair.enabled = true;
        fpsController.canMove = true;
        eventFMOD.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }

    public void TurnOnTv()
    {
        StartCoroutine(StartWatchTV());
    }
}
