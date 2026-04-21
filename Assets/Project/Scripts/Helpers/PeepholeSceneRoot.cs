using UnityEngine;
using UnityEngine.Rendering;

// à dans chaque scène additive, sur un GameObject racine
public class PeepholeSceneRoot : MonoBehaviour
{
    [Header("Ref")]
    public Camera PeepholeCamera;
    public Volume FisheyeVolume;
    
    [Header("Vignette")]
    [Range(0.1f, 1f)]  public float vignetteRadius   = 0.38f;
    [Range(0f,   0.5f)] public float vignetteSoftness = 0.08f;
    [Range(0f,   1f)]  public float fisheyeStrength   = 0.4f;
    
    private Vector3 _judasForward;
    private bool    _initialized = false;

    
    public void InitializeJudasForward()
    {
        _judasForward = PeepholeCamera.transform.forward;
        _initialized  = true;

        // Pousser les valeurs directement dans les statics du shader
        FisheyePostProcess.GlobalStrength        = fisheyeStrength;
        FisheyePostProcess.GlobalVignetteRadius   = vignetteRadius;
        FisheyePostProcess.GlobalVignetteSoftness = vignetteSoftness;
    }

    private void Update()
    {
        if (!_initialized) return;

        Vector3 worldCenter = PeepholeCamera.transform.position + _judasForward * 10f;
        Vector3 screenPos   = PeepholeCamera.WorldToViewportPoint(worldCenter);
        FisheyePostProcess.VignetteCenter = new Vector2(screenPos.x, screenPos.y);
        
        FisheyePostProcess.GlobalStrength         = fisheyeStrength;
        FisheyePostProcess.GlobalVignetteRadius   = vignetteRadius;
        FisheyePostProcess.GlobalVignetteSoftness = vignetteSoftness;
    }
}
