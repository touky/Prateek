Shader "Prateek/DebugLineShaderFront"
{
	Properties{ }
		SubShader
	{
		Tags{ "RenderType" = "Opaque" "Queue" = "Geometry+449" }

		UsePass "Prateek/DebugLineShader_ABSTRACT/Front"
	}
}