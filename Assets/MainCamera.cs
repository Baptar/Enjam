using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class MainCamera : MonoBehaviour
{
    private void Start()
    {
        var data = GetComponent<HDAdditionalCameraData>();
        data.customRenderingSettings = true;

        data.renderingPathCustomFrameSettings.SetEnabled(FrameSettingsField.FPTLForForwardOpaque, false);
        data.renderingPathCustomFrameSettingsOverrideMask.mask[(uint)FrameSettingsField.FPTLForForwardOpaque] = true;
    }
}
