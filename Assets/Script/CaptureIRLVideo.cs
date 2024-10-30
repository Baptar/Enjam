using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    [SerializeField] private float delayWatchIRL = 5f;
    
    public void StartTVIRL()
    {
        WebCamDevice device = WebCamTexture.devices[numberDevice];
        webCamTexture = new WebCamTexture(device.name);
        GetComponent<Renderer>().material.mainTexture = webCamTexture;
        if(!webCamTexture.isPlaying) webCamTexture.Play();
    }

    public void StartTVParc()
    {
        cam.GetComponent<Camera>().enabled = true;
        GetComponent<Renderer>().material.mainTexture = cam.targetTexture;
    }

    public void WatchTv()
    {
        StartCoroutine(StartWatchTV());
    }

    IEnumerator StartWatchTV()
    {
        blackBoardTV.GetComponent<Animator>().SetTrigger("ChangeTV");
        crosshair.enabled = false;
        StartTVParc();
        yield return new WaitForSeconds(delayWatchParc);
        StartTVIRL();
        yield return new WaitForSeconds(delayWatchIRL);
        blackBoardPlayer.GetComponent<Animator>().SetTrigger("ChangePlayer");
        crosshair.enabled = true;
    }
}
