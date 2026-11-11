Shader "Hidden/FisheyePostProcess"
{
    HLSLINCLUDE
    #pragma target 4.5
    #pragma only_renderers d3d11 playstation xboxone xboxseries vulkan metal switch

    #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
    #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
    #include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderVariables.hlsl"

    struct Attributes
    {
        uint vertexID : SV_VertexID;
        UNITY_VERTEX_INPUT_INSTANCE_ID
    };

    struct Varyings
    {
        float4 positionCS : SV_POSITION;
        float2 texcoord   : TEXCOORD0;
        UNITY_VERTEX_OUTPUT_STEREO
    };

    Varyings Vert(Attributes input)
    {
        Varyings output;
        UNITY_SETUP_INSTANCE_ID(input);
        UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);
        output.positionCS = GetFullScreenTriangleVertexPosition(input.vertexID);
        output.texcoord   = GetFullScreenTriangleTexCoord(input.vertexID);
        return output;
    }

    TEXTURE2D_X(_InputTexture);

    float  _Strength;
    float  _VignetteRadius;
    float  _VignetteSoftness;
    float2 _VignetteCenter;
    float  _Aspect;

    float2 FisheyeDistort(float2 uv, float strength)
    {
        float2 c = uv * 2.0 - 1.0;
        c.x *= _Aspect;
        
        float maxDist = length(float2(_Aspect, 1.0));
        c /= maxDist;

        float dist = length(c);
        float s = max(strength, 1e-4);
        float distortion = (dist > 1e-5) ? atan(dist * s) / (dist * s) : 1.0;
        c *= distortion;

        c *= maxDist;
        c.x /= _Aspect;
        return c * 0.5 + 0.5;
    }

    float4 Frag(Varyings input) : SV_Target
    {
        UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);

        float2 uv = input.texcoord;
        float2 distortedUV = saturate(FisheyeDistort(uv, _Strength));

        float4 color = SAMPLE_TEXTURE2D_X_LOD(_InputTexture, s_linear_clamp_sampler,
                                              distortedUV * _RTHandleScale.xy, 0);

        float2 diff = uv - _VignetteCenter;
        diff.x *= _Aspect;
        float vignette = 1.0 - smoothstep(_VignetteRadius - _VignetteSoftness,
                                          _VignetteRadius,
                                          length(diff));

        color.rgb *= vignette;
        return color;
    }
    ENDHLSL

    SubShader
    {
        Tags { "RenderPipeline" = "HDRenderPipeline" }
        Pass
        {
            ZWrite Off
            ZTest Always
            Blend Off
            Cull Off

            HLSLPROGRAM
                #pragma fragment Frag
                #pragma vertex Vert
            ENDHLSL
        }
    }
    Fallback Off
}