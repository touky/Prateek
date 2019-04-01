Shader "Prateek/DebugLineShader"
{
    Properties { }
    SubShader
    {
        Tags{ "RenderType" = "Opaque" "Queue" = "Geometry+449" }

        Pass
        {
            Name "Back"
            ZTest Greater
            ZWrite Off
            CGPROGRAM

            #include "UnityCG.cginc"

            StructuredBuffer<float4> positionBuffer;
            StructuredBuffer<float4> colorBuffer;

            #pragma target 5.0
            #pragma fragment frag
            #pragma vertex vert

            #define DIMMER half3(0.6,0.6,0.6)
            #define BORDER half3(0.5,0.5,0.5)
            #include "DebugLineShaderPixel.cginc"
            #include "DebugLineShaderVertex.cginc"
            #undef DIMMER
            #undef BORDER
            ENDCG
        }

        Pass
        {
            Name "Front"
            Cull Off
            ZTest LEqual
            CGPROGRAM

            #include "UnityCG.cginc"

            StructuredBuffer<float4> positionBuffer;
            StructuredBuffer<float4> colorBuffer;

            #pragma target 5.0
            #pragma fragment frag
            #pragma vertex vert

            #define DIMMER half3(1,1,1)
            #define BORDER half3(0,0,0)
            #include "DebugLineShaderPixel.cginc"
            #include "DebugLineShaderVertex.cginc"
            #undef DIMMER
            #undef BORDER
            ENDCG
        }
    }
}