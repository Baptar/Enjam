using UnityEngine;

public class MainManager : MonoBehaviour
{
    public static MainManager instance;
    
    [SerializeField] private PlayerManager playerManager;
    [SerializeField] private AudioManager audioManager;
    [SerializeField] private UIManager uiManager;
    [SerializeField] private PaperManager paperManager;
    [SerializeField] private JudaEventManager judasEventManager;
    [SerializeField] private CameraManager cameraManager;
    private PlayerInputController playerInputManager;

    public PlayerManager Player => playerManager;
    public PlayerInputController PlayerInputManager => playerManager.gameObject.GetComponentInChildren<PlayerInputController>();
    public AudioManager AudioManager => audioManager;
    public UIManager UIManager => uiManager;
    public PaperManager PaperManager => paperManager;
    public JudaEventManager JudasesManager => judasEventManager;
    public Camera PlayerCamera => playerManager.GetPlayerCamera();
    public CameraManager CameraManager => cameraManager;
    

    private void Awake()
    {
        if (instance == null) instance = this;
    }
}
