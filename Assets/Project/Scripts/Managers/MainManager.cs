using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MainManager : MonoBehaviour
{
    public static MainManager instance;
    
    [SerializeField] private PlayerManager playerManager;
    [SerializeField] private AudioManager audioManager;
    [SerializeField] private UIManager uiManager;
    [SerializeField] private PaperManager paperManager;
    private PlayerInputController playerInputManager;

    public PlayerManager Player => playerManager;
    public PlayerInputController PlayerInputManager => playerManager.gameObject.GetComponentInChildren<PlayerInputController>();
    public AudioManager AudioManager => audioManager;
    public UIManager UIManager => uiManager;
    public PaperManager PaperManager => paperManager;
    

    private void Awake()
    {
        if (instance == null) instance = this;
    }
}
