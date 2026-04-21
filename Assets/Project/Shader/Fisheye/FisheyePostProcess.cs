using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using System;

[Serializable, VolumeComponentMenu("Post-processing/Custom/Fisheye")]
public class FisheyePostProcess : CustomPostProcessVolumeComponent, IPostProcessComponent
{
    // Tout est contrôlé via statics — le VolumeProfile n'est plus utilisé pour les valeurs
    public static bool    GlobalStrengthOverride  = false;
    public static float   GlobalStrength          = 0.4f;
    public static float   GlobalVignetteRadius    = 0.38f;
    public static float   GlobalVignetteSoftness  = 0.08f;
    public static Vector2 VignetteCenter          = new Vector2(0.5f, 0.5f);

    Material _material;

    public bool IsActive() => _material != null && GlobalStrengthOverride;

    public override CustomPostProcessInjectionPoint injectionPoint =>
        CustomPostProcessInjectionPoint.AfterPostProcess;

    public override void Setup()
    {
        _material = new Material(Shader.Find("Hidden/FisheyePostProcess"));
    }

    public override void Render(CommandBuffer cmd, HDCamera camera, RTHandle source, RTHandle destination)
    {
        if (_material == null) return;

        if (!GlobalStrengthOverride)
        {
            HDUtils.DrawFullScreen(cmd, _material, destination);
            return;
        }

        _material.SetFloat("_Strength",        GlobalStrength);
        _material.SetFloat("_VignetteRadius",   GlobalVignetteRadius);
        _material.SetFloat("_VignetteSoftness", GlobalVignetteSoftness);
        _material.SetVector("_VignetteCenter",  VignetteCenter);
        _material.SetTexture("_InputTexture",   source);

        HDUtils.DrawFullScreen(cmd, _material, destination);
    }

    public override void Cleanup() => CoreUtils.Destroy(_material);
}