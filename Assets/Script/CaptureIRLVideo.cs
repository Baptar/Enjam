using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CaptureIRLVideo : MonoBehaviour
{
    WebCamTexture webCamTexture;
    [SerializeField] public int numberDevice;

    // Start is called before the first frame update
    void Start()
    {
        WebCamDevice device = WebCamTexture.devices[numberDevice];
        webCamTexture = new WebCamTexture(device.name);
        GetComponent<Renderer>().material.mainTexture = webCamTexture;
        if(!webCamTexture.isPlaying) webCamTexture.Play();

        /*webCamTexture = new WebCamTexture();
        GetComponent<Renderer>().material.mainTexture = webCamTexture;
        if(!webCamTexture.isPlaying) webCamTexture.Play();*/
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
