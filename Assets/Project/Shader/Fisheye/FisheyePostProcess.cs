using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using System;

[Serializable, VolumeComponentMenu("Post-processing/Custom/Fisheye")]
public class FisheyePostProcess : CustomPostProcessVolumeComponent, IPostProcessComponent
{
    public static bool    GlobalStrengthOverride = false;
    public static float   GlobalStrength         = 0.4f;
    public static float   GlobalVignetteRadius   = 0.38f;
    public static float   GlobalVignetteSoftness = 0.08f;
    public static Vector2 VignetteCenter         = new Vector2(0.5f, 0.5f);

    const string ShaderName = "Hidden/FisheyePostProcess";

    Material _material;

    public bool IsActive() => _material != null && GlobalStrengthOverride;

    public override CustomPostProcessInjectionPoint injectionPoint =>
        CustomPostProcessInjectionPoint.AfterPostProcess;

    public override void Setup()
    {
        // add shader in Project Settings > Graphics > Always Included Shaders,
        // Shader.Find return null in build.
        var shader = Shader.Find(ShaderName);
        if (shader == null)
        {
            Debug.LogError($"Shader {ShaderName} introuvable (l'ajouter dans Always Included Shaders).");
            return;
        }
        _material = CoreUtils.CreateEngineMaterial(shader);
    }

    public override void Render(CommandBuffer cmd, HDCamera camera, RTHandle source, RTHandle destination)
    {
        if (_material == null || !GlobalStrengthOverride)
        {
            HDUtils.BlitCameraTexture(cmd, source, destination);
            return;
        }

        float aspect = (float)camera.actualWidth / Mathf.Max(1, camera.actualHeight);

        _material.SetFloat("_Strength",         GlobalStrength);
        _material.SetFloat("_VignetteRadius",   GlobalVignetteRadius);
        _material.SetFloat("_VignetteSoftness", GlobalVignetteSoftness);
        _material.SetVector("_VignetteCenter",  VignetteCenter);
        _material.SetFloat("_Aspect",           aspect);
        _material.SetTexture("_InputTexture",   source);

        HDUtils.DrawFullScreen(cmd, _material, destination);
    }

    public override void Cleanup() => CoreUtils.Destroy(_material);
}