Shader "Custom/PainterlyToon"
{
    Properties
    {
        _BaseMap ("Base Texture", 2D) = "white" {}
        _BaseColor ("Base Color", Color) = (1,1,1,1)

        _ShadowColor ("Shadow Color", Color) = (0.55, 0.55, 0.7, 1)
        _LightThreshold ("Light Threshold", Range(0,1)) = 0.45
        _Softness ("Toon Softness", Range(0.001,0.5)) = 0.08

        _PaintNoise ("Painterly Noise", 2D) = "white" {}
        _NoiseStrength ("Noise Strength", Range(0,0.4)) = 0.12
        _BrushScale ("Brush Scale", Float) = 3

        _RimColor ("Rim Color", Color) = (1,1,1,1)
        _RimPower ("Rim Power", Range(0.5,8)) = 3
        _RimStrength ("Rim Strength", Range(0,1)) = 0.25
    }

    SubShader
    {
        Tags
        {
            "RenderPipeline"="UniversalPipeline"
            "RenderType"="Opaque"
            "Queue"="Geometry"
        }

        Pass
        {
            Name "ForwardLit"
            Tags { "LightMode"="UniversalForward" }

            HLSLPROGRAM

            #pragma vertex vert
            #pragma fragment frag

            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
            #pragma multi_compile _ _SHADOWS_SOFT

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float3 positionWS : TEXCOORD0;
                float3 normalWS : TEXCOORD1;
                float2 uv : TEXCOORD2;
                float4 shadowCoord : TEXCOORD3;
            };

            TEXTURE2D(_BaseMap);
            SAMPLER(sampler_BaseMap);

            TEXTURE2D(_PaintNoise);
            SAMPLER(sampler_PaintNoise);

            CBUFFER_START(UnityPerMaterial)
                float4 _BaseMap_ST;
                float4 _BaseColor;
                float4 _ShadowColor;
                float _LightThreshold;
                float _Softness;
                float _NoiseStrength;
                float _BrushScale;
                float4 _RimColor;
                float _RimPower;
                float _RimStrength;
            CBUFFER_END

            Varyings vert(Attributes IN)
            {
                Varyings OUT;

                VertexPositionInputs posInputs = GetVertexPositionInputs(IN.positionOS.xyz);
                VertexNormalInputs normalInputs = GetVertexNormalInputs(IN.normalOS);

                OUT.positionHCS = posInputs.positionCS;
                OUT.positionWS = posInputs.positionWS;
                OUT.normalWS = normalize(normalInputs.normalWS);
                OUT.uv = TRANSFORM_TEX(IN.uv, _BaseMap);
                OUT.shadowCoord = TransformWorldToShadowCoord(OUT.positionWS);

                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                float3 normalWS = normalize(IN.normalWS);
                float3 viewDirWS = normalize(GetWorldSpaceViewDir(IN.positionWS));

                Light mainLight = GetMainLight(IN.shadowCoord);

                float NdotL = saturate(dot(normalWS, mainLight.direction));

                // Toon lighting band
                float toonLight = smoothstep(
                    _LightThreshold - _Softness,
                    _LightThreshold + _Softness,
                    NdotL * mainLight.shadowAttenuation
                );

                float4 baseTex = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, IN.uv);
                float3 baseColor = baseTex.rgb * _BaseColor.rgb;

                // Painterly brush/noise variation
                float2 brushUV = IN.uv * _BrushScale;
                float noise = SAMPLE_TEXTURE2D(_PaintNoise, sampler_PaintNoise, brushUV).r;
                noise = (noise - 0.5) * _NoiseStrength;

                float3 litColor = baseColor * mainLight.color;
                float3 shadowColor = baseColor * _ShadowColor.rgb;

                float3 finalColor = lerp(shadowColor, litColor, toonLight);

                // Add painterly color breakup
                finalColor += noise;

                // Soft rim light
                float rim = pow(1.0 - saturate(dot(normalWS, viewDirWS)), _RimPower);
                finalColor += rim * _RimColor.rgb * _RimStrength;

                return half4(finalColor, baseTex.a * _BaseColor.a);
            }

            ENDHLSL
        }
    }
}