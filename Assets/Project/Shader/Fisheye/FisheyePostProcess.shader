Shader "Hidden/FisheyePostProcess"
{
    HLSLINCLUDE
    #pragma target 4.5
    #pragma only_renderers d3d11 playstation xboxone xboxseries vulkan metal switch

    #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
    #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
    #include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderVariables.hlsl"
    #include "Packages/com.unity.render-pipelines.high-definition/Runtime/PostProcessing/Shaders/RTUpscale.hlsl"

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

    /*float2 FisheyeDistort(float2 uv, float strength)
{
    float2 centered = uv * 2.0 - 1.0;
    float aspect = _ScreenParams.x / _ScreenParams.y;
    centered.x *= aspect;

    float dist = length(centered);
    
    // Normalise : distord fort au centre, revient à 1 sur les bords
    float distortion = 1.0 + strength * dist * (1.0 - dist);
    centered *= distortion;

    centered.x /= aspect;
    return centered * 0.5 + 0.5;
}*/
    float2 FisheyeDistort(float2 uv, float strength)
    {
        float2 centered = uv * 2.0 - 1.0;
        float aspect = _ScreenParams.x / _ScreenParams.y;
        centered.x *= aspect;

        float dist = length(centered);
        
        // atan est naturellement borné → jamais de répétition ni d'étirement
        float distortion = (dist > 0.0) ? (atan(dist * strength) / (dist * strength)) : 1.0;
        centered *= distortion;

        centered.x /= aspect;
        return centered * 0.5 + 0.5;
    }

    float4 Frag(Varyings input) : SV_Target
    {
        UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);

        float2 uv = input.texcoord;
        float2 distortedUV = FisheyeDistort(uv, _Strength);

        // Clamp au lieu de noir
        distortedUV = saturate(distortedUV);
        float4 color = SAMPLE_TEXTURE2D_X(_InputTexture, s_linear_clamp_sampler, distortedUV);

        // Vignette
        float2 diff = uv - _VignetteCenter;
        float aspect = _ScreenParams.x / _ScreenParams.y;
        diff.x *= aspect;
        float vignetteDist = length(diff);

        float vignette = 1.0 - smoothstep(
            _VignetteRadius - _VignetteSoftness,
            _VignetteRadius,
            vignetteDist
        );

        color.rgb *= vignette;
        return color;
    }

    ENDHLSL

    SubShader
    {
        Tags{ "RenderPipeline" = "HDRenderPipeline" }
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


/*
Shader "Hidden/FisheyePostProcess"
{
    HLSLINCLUDE
    #pragma target 4.5
    #pragma only_renderers d3d11 playstation xboxone xboxseries vulkan metal switch

    #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
    #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
    #include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderVariables.hlsl"
    #include "Packages/com.unity.render-pipelines.high-definition/Runtime/PostProcessing/Shaders/RTUpscale.hlsl"

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

    float2 FisheyeDistort(float2 uv, float strength)
{
    float2 centered = uv * 2.0 - 1.0;
    float aspect = _ScreenParams.x / _ScreenParams.y;
    centered.x *= aspect;

    float dist = length(centered);
    // Division au lieu de multiplication → tout reste dans [0,1]
    float distortion = 1.0 / (1.0 + strength * dist * dist);
    centered *= distortion;

    centered.x /= aspect;
    return centered * 0.5 + 0.5;
}

    float4 Frag(Varyings input) : SV_Target
{
    UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);

    float2 uv = input.texcoord;
    float2 distortedUV = FisheyeDistort(uv, _Strength);

    float4 color = SAMPLE_TEXTURE2D_X(_InputTexture, s_linear_clamp_sampler, distortedUV);

    // Vignette circulaire
    float2 diff = uv - _VignetteCenter;
    float aspect = _ScreenParams.x / _ScreenParams.y;
    diff.x *= aspect;
    float vignetteDist = length(diff);

    float vignette = 1.0 - smoothstep(
        _VignetteRadius - _VignetteSoftness,
        _VignetteRadius,
        vignetteDist
    );

    color.rgb *= vignette;
    return color;
}

    ENDHLSL

    SubShader
    {
        Tags{ "RenderPipeline" = "HDRenderPipeline" }
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
}*
*/