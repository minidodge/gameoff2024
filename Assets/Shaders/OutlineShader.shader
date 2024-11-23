Shader "Custom/URPOutlineShader" {
    Properties {
        _OutlineColor ("Outline Color", Color) = (0,0,0,1)
        _OutlineWidth ("Outline Width", Range (.002, 0.03)) = .005
    }
    SubShader {
        Tags { "RenderType"="Opaque" "Queue"="Overlay" }
        Pass {
            Name "OUTLINE"
            Tags { "LightMode" = "UniversalForward" }
            Cull Front

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            struct Attributes {
                float4 positionOS : POSITION;
                float3 normalOS : NORMAL;
            };

            struct Varyings {
                float4 positionHCS : SV_POSITION;
                float4 color : COLOR;
            };

            float _OutlineWidth;
            float4 _OutlineColor;

            Varyings vert (Attributes input) {
                Varyings output;
                float3 normalWS = TransformObjectToWorldNormal(input.normalOS);
                float3 positionWS = TransformObjectToWorld(input.positionOS).xyz;
                positionWS += normalWS * _OutlineWidth;
                output.positionHCS = TransformWorldToHClip(positionWS);
                output.color = _OutlineColor;
                return output;
            }

            half4 frag (Varyings input) : SV_Target {
                return input.color;
            }
            ENDHLSL
        }
    }
    FallBack "Hidden/InternalErrorShader"
}