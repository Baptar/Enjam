using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class JudaEventManager : MonoBehaviour
{
    [Header("References")] 
    [SerializeField] private DoorCoridorInteract[] doors;
    [SerializeField] private ObjectGrabbable judasObject;
    
    [Space(10)]
    [Header("Debug")]
    [SerializeField] private List<GameObject> judasFound = new List<GameObject>();
    
    private Vector3 camLocalPositionBeforeJudas;
    private Quaternion camLocalRotationBeforeJudas;
    private float fovBeforeJudas;
    private DoorCoridorInteract currentDoorLooking;

    private String currentJudasSceneName;
    private Vector3 judaScaleStart;
    private Transform camJudaTransformTarget;

    private void Start()
    {
        judaScaleStart = judasObject.transform.localScale;
    }
    
    [ContextMenu("Start Judas Event")]
    public void StartJudasEvent()
    {
        foreach (DoorCoridorInteract door in doors)
            door.SetJudasEvent();
    }
    
    public void OnInteractJudas(DoorCoridorInteract judasDoor, Transform camJudaTarget, Transform judasTransformTarget, String judasSceneName, float fovCam)
    {
        if (!judasFound.Contains(judasDoor.gameObject))
            judasFound.Add(judasDoor.gameObject);
        
        PlayerManager player = MainManager.instance.Player;
        Camera playerCamera = MainManager.instance.PlayerCamera;
        UIManager uiManager = MainManager.instance.UIManager;
        if (!player || !playerCamera) return;
        
        uiManager.EnableCrosshair(false);
        uiManager.EnableInteractionText(false);
        currentDoorLooking = judasDoor;
        camJudaTransformTarget = camJudaTarget;
        
        // set player cant move
        player.SetCanMove(false);
        
        // save previous Cam settings
        camLocalPositionBeforeJudas = playerCamera.transform.localPosition;
        camLocalRotationBeforeJudas = playerCamera.transform.localRotation;
        fovBeforeJudas = playerCamera.fieldOfView;
        
        // Move Juda
        judasObject.bFollowTargetPoint = false;
        Sequence sequenceMoveJuda = DOTween.Sequence();
        sequenceMoveJuda.Append(judasObject.transform.DOMove(judasTransformTarget.position, 1.0f).SetEase(Ease.InOutFlash))
            .Join(judasObject.transform.DORotateQuaternion(judasTransformTarget.rotation, 1.0f).SetEase(Ease.InOutFlash))
            .Join(judasObject.transform.DOScale(judasTransformTarget.localScale, 1.0f).SetEase(Ease.InOutFlash))
            .InsertCallback(0.75f,()=> 
            {
                // make screen black
                uiManager.FadeScreen(true, 1.0f);
        
                // move playerCamToJudasTransform and change fov
                Sequence camSequence = DOTween.Sequence();
                camSequence.Append(playerCamera.transform.DOMove(camJudaTarget.position, 1.0f).SetEase(Ease.InOutFlash))
                    .Join(playerCamera.transform.DORotateQuaternion(camJudaTarget.rotation, 1.0f).SetEase(Ease.InOutFlash))
                    .Join(playerCamera.DOFieldOfView(fovBeforeJudas, 1.0f).SetEase(Ease.InOutFlash))
                    .OnComplete(() =>
                    {
                        StartCoroutine(LoadJudasSceneAndSwitch(judasSceneName));
                    });
            });
    }
    
    private IEnumerator LoadJudasSceneAndSwitch(string judasSceneName)
    {
        FisheyePostProcess.GlobalStrengthOverride = true;
        
        // wait to load additive scene
        var loadScene = SceneManager.LoadSceneAsync(judasSceneName, LoadSceneMode.Additive);
        yield return loadScene;

        // get PeepholeSceneRoot in loaded scene
        var loadedScene = SceneManager.GetSceneByName(judasSceneName);
        PeepholeSceneRoot peepholeRoot = FindRootInScene(loadedScene);

        if (peepholeRoot == null)
        {
            Debug.LogError($"no PeepholeSceneRoot in scene {judasSceneName}");
            yield break;
        }
        currentJudasSceneName = judasSceneName;

        // deactivate player cam, activate judas' one
        MainManager.instance.PlayerCamera.gameObject.SetActive(false);
        peepholeRoot.PeepholeCamera.gameObject.SetActive(true);
        peepholeRoot.InitializeJudasForward();

        // switch look mode
        MainManager.instance.Player.SetPeepholeRoot(peepholeRoot);
        MainManager.instance.Player.SetLookMode(PlayerManager.ELookMode.Peephole);
        //MainManager.instance.Player.SetCanMove(true);

        yield return new WaitForSeconds(0.2f);

        // fade in
        MainManager.instance.UIManager.FadeScreen(false, 0.5f);
        yield return new WaitForSeconds(1.0f);
        MainManager.instance.Player.SetIsInJudasMode(true);
    }
    
    private PeepholeSceneRoot FindRootInScene(Scene scene)
    {
        foreach (var go in scene.GetRootGameObjects())
        {
            var root = go.GetComponent<PeepholeSceneRoot>();
            if (root != null) return root;
        }
        return null;
    }

    public IEnumerator ExitJudas()
    {
        // get managers ref
        UIManager uiManager = MainManager.instance.UIManager;
        PlayerManager player = MainManager.instance.Player;
        Camera cam = MainManager.instance.PlayerCamera;
        
        // change screen look mode
        player.SetCanMove(false);
        player.SetLookMode(PlayerManager.ELookMode.CantLook);
        MainManager.instance.Player.SetIsInJudasMode(false);
        
        // fade screen
        uiManager.FadeScreen(true, 0.5f);
        yield return new WaitForSeconds(0.5f);
        FisheyePostProcess.GlobalStrengthOverride = false;

        // swap cam
        PeepholeSceneRoot root = FindRootInScene(SceneManager.GetSceneByName(currentJudasSceneName));
        root.PeepholeCamera.gameObject.SetActive(false);
        cam.gameObject.SetActive(true);

        // unload scene
        yield return SceneManager.UnloadSceneAsync(currentJudasSceneName);

        cam.transform.position = camJudaTransformTarget.position;
        cam.transform.rotation = camJudaTransformTarget.rotation;
        
        uiManager.FadeScreen(false, 0.5f);
        
        // move back player cam
        Sequence camSequence = DOTween.Sequence();
        camSequence.Append(cam.transform.DOLocalMove(camLocalPositionBeforeJudas, 1.0f).SetEase(Ease.InOutFlash))
            .Join(cam.transform.DOLocalRotateQuaternion(camLocalRotationBeforeJudas, 1.0f).SetEase(Ease.InOutFlash))
            .Join(cam.DOFieldOfView(fovBeforeJudas, 1.0f).SetEase(Ease.InOutFlash))
            .OnComplete(() =>
            {
                judasObject.bFollowTargetPoint = true;
                judasObject.transform.DOScale(judaScaleStart, 1.0f).SetEase(Ease.InOutFlash);
                player.SetLookMode(PlayerManager.ELookMode.Normal);
                player.SetCanMove(true);
            });
        
        yield return new WaitForSeconds(1.0f);
        if (judasFound.Count >= doors.Length) OnAllJudasLooked();

        yield return new WaitForSeconds(0.3f);
        uiManager.EnableCrosshair(true);
        uiManager.EnableInteractionText(true);
        
        if (currentDoorLooking) currentDoorLooking.SetInteractable(true);
        currentDoorLooking = null;
    }

    private void OnAllJudasLooked()
    {
        Debug.Log("All judas looked");
        enabled = false;
    }
}
