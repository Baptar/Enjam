using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Camera Camera_Player;
    public Camera Camera_Parc;
    public Camera Camera_TV;

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
        Camera_TV.GetComponent<Camera>().enabled = true;
    }
}
