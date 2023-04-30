Shader "PLAYER TWO/Platformer Project/Lit Reflective"
{
	Properties
	{
		_Color ("Color", Color) = (1, 1, 1, 1)
		_ReflectColor ("Reflection Color", Color) = (1, 1, 1, 1)
		_MainTex ("Base (RGB) RefStrength (A)", 2D) = "white" {}
		_Cube ("Reflection Cubemap", Cube) = "" {}
		_Smoothness ("Smoothness", Range(0, 1)) = 0
	}

	SubShader
	{
		Tags { "RenderType" = "Opaque" }
		LOD 200

		CGPROGRAM

		#pragma surface surf Standard fullforwardshadows
		#pragma target 3.0

		sampler2D _MainTex;
		samplerCUBE _Cube;

		struct Input
		{
			float2 uv_MainTex;
			float3 worldRefl;
		};

		fixed4 _Color;
		fixed4 _ReflectColor;

		fixed _Smoothness;

		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			fixed4 base = tex2D(_MainTex, IN.uv_MainTex);
			fixed4 reflcol = texCUBE(_Cube, IN.worldRefl);

			reflcol *= base.a;

			o.Albedo = base.rgb * _Color;
			o.Smoothness = _Smoothness;
			o.Emission = reflcol.rgb * _ReflectColor.rgb;
		}

		ENDCG
	}

	FallBack "Diffuse"
}
