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

            #define DIMMER half4(0.4,0.4,0.4,0.4)
            #include "DebugLineShaderPixel.cginc"
            #include "DebugLineShaderVertex.cginc"
            #undef DIMMER
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

            #define DIMMER half4(1,1,1,1)
            #include "DebugLineShaderPixel.cginc"
            #include "DebugLineShaderVertex.cginc"
            #undef DIMMER
            ENDCG
        }
    }
}