using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class SwitchCamera : MonoBehaviour
{
    public GameObject Camera_Player;
    public GameObject Camera_Parc;
    public GameObject Camera_TV;
    [SerializeField] private CaptureIRLVideo captureIrl;

    public void Cam_Player()
    {
        Camera_Player.GetComponent<Camera>().enabled = true;
        Camera_Parc.GetComponent<Camera>().enabled = false;
        Camera_TV.GetComponent<Camera>().enabled = false;
    }
    
    public void Cam_Parc()
    {
        Camera_Player.GetComponent<Camera>().enabled = false;
        Camera_Parc.GetComponent<Camera>().enabled = true;
        Camera_TV.GetComponent<Camera>().enabled = false;
    }
    
    public void Cam_TV()
    {
        Camera_Player.GetComponent<Camera>().enabled = false;
        Camera_Parc.GetComponent<Camera>().enabled = false;
        Camera_TV.GetComponent<Camera>().enabled = true;
        
        captureIrl.TurnOnTv();
    }
}
