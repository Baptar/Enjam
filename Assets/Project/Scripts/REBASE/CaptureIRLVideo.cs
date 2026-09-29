using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class CaptureIRLVideo : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float delayWatchParc = 5f;
    [SerializeField] private float delayWatchIrl = 5f;
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private GameObject graveObj;
    
    [Space(10)]
    [Header("DEBUG")]
    [SerializeField] public int numberDevice;
    [FormerlySerializedAs("cam")] [SerializeField] private Camera cameraParc;
    
    
    private FMOD.Studio.EventInstance eventFMOD;
    private WebCamTexture webCamTexture;

    private void Start()
    {
        eventFMOD = FMODUnity.RuntimeManager.CreateInstance("event:/Salon/Tele");
        InitWebcamTexture();
        
        meshRenderer.material.mainTexture = null;
        meshRenderer.material.color = Color.black;
    }

    private void InitWebcamTexture()
    {
        if (WebCamTexture.devices.Length < numberDevice + 1) return;
        WebCamDevice device = WebCamTexture.devices[numberDevice];
        Debug.Log("Webcam détectée : " + WebCamTexture.devices[0].name);
        Debug.Log("device.lenght : " + WebCamTexture.devices.Length);
        webCamTexture = new WebCamTexture(device.name);
    }

    public void WatchTv()
    {
        MainManager.instance.Player.SetLookMode(PlayerManager.ELookMode.CantLook);
        MainManager.instance.Player.SetCanMove(false);
        
        StartTVParc();
        FMODUnity.RuntimeManager.PlayOneShot("event:/Salon/TeleStateToCamTrig");
        MainManager.instance.UIManager.EnableCrosshair(false);
        
        Sequence sequence = DOTween.Sequence();
        Image img = MainManager.instance.UIManager.fadeImage;

        sequence.Append(img.DOColor(Color.black, 0.4f).SetEase(Ease.InOutFlash));
        sequence.AppendCallback(()=>MainManager.instance.CameraManager.Cam_TV());   
        sequence.Append(img.DOColor(Color.clear, 0.4f).SetEase(Ease.InOutFlash));
        sequence.AppendCallback(MakeGraveAppear);
        sequence.AppendInterval(delayWatchParc);
        sequence.AppendCallback(() =>
        {
            if (webCamTexture)
            {
                StartTvirl();
            }
        });
        sequence.AppendInterval(delayWatchIrl);
        sequence.Append(img.DOColor(Color.black, 0.4f).SetEase(Ease.InOutFlash));
        sequence.AppendCallback(() =>
        {
            MainManager.instance.CameraManager.Cam_Player();
            MainManager.instance.Player.SetCanMove(true);
            MainManager.instance.Player.SetLookMode(PlayerManager.ELookMode.Normal);
            eventFMOD.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        });
        sequence.Append(img.DOColor(Color.clear, 0.4f).SetEase(Ease.InOutFlash));
        sequence.AppendCallback(()=>MainManager.instance.UIManager.EnableCrosshair(true));
    }


    
    
    private void StartTvirl()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/Salon/TeleStateToCamTrig");
        if(!webCamTexture.isPlaying) webCamTexture.Play();
        GetComponent<Renderer>().material.mainTexture = webCamTexture;
    }

    private void StartTVParc()
    {
        cameraParc.enabled = true;
        
        meshRenderer.material.mainTexture = cameraParc.targetTexture;
        Color c = new Color(0.75f, 0.75f, 0.75f, .75f);
        meshRenderer.material.color = c;
    }

    private void MakeGraveAppear()
    {
        graveObj.SetActive(true);
        
        Sequence sequence = DOTween.Sequence();
        sequence.Append(graveObj.transform.DOLocalMoveY(graveObj.transform.localPosition.y + 1.2f, 3.5f)
            .SetEase(Ease.InOutExpo))
            .Join(graveObj.transform.DOShakeRotation(3.5f, 10f));
    }
}
