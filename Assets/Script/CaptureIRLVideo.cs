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
    
    private FMOD.Studio.EventInstance eventFMOD;

    void Start()
    {
        eventFMOD = FMODUnity.RuntimeManager.CreateInstance("event:/Salon/Tele");
        
        WebCamDevice device = WebCamTexture.devices[numberDevice];
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
        GetComponent<Renderer>().material = null;
        FMODUnity.RuntimeManager.PlayOneShot("event:/Salon/TeleStateToCamTrig");
        StartCoroutine(StartWatchTV());
    }

    IEnumerator StartWatchTV()
    {
        blackBoardTV.GetComponent<Animator>().SetTrigger("ChangeTV");
        crosshair.enabled = false;
        yield return new WaitForSeconds(0.3f);
        StartTVParc();
        yield return new WaitForSeconds(delayWatchParc);
        StartTvirl();
        yield return new WaitForSeconds(delayWatchIrl);
        blackBoardPlayer.GetComponent<Animator>().SetTrigger("ChangePlayer");
        crosshair.enabled = true;
        eventFMOD.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }
}
