Shader "Gondr/MaskShader"
{
    Properties
    {
    }
    SubShader
    {
        Tags { "RenderType" = "Opaque" }
        LOD 100
        ZWrite Off
        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS   : POSITION;  //À§Ä¡
            };

            struct Varyings
            {
                float4 positionCS  : SV_POSITION;
            };

            Varyings vert(Attributes IN)
            {
                Varyings OUT;

                OUT.positionCS = TransformObjectToHClip(IN.positionOS.xyz);

                return OUT;
            }

            half4 frag(Varyings i) : SV_Target
            {
                return (half4)0;
            }
            ENDHLSL

        }
    }
}