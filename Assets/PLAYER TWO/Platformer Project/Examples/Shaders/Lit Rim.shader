Shader "PLAYER TWO/Platformer Project/Lit Rim"
{
	Properties
	{
		_Color ("Color", Color) = (1, 1, 1, 1)
		_Rim ("Rim", Range(0, 1)) = 1
		_RimPower ("Rim Power", Range(0.5, 8)) = 3
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
	}

	SubShader
	{
		Tags { "RenderType" = "Opaque" }
		LOD 200
		CULL OFF

		CGPROGRAM

		#pragma surface surf Standard fullforwardshadows
		#pragma target 3.0

		sampler2D _MainTex;

		struct Input
		{
			float2 uv_MainTex;
			float3 viewDir;
		};

		fixed3 _Color;

		fixed _Rim;
		fixed _RimPower;

		void surf (Input IN, inout SurfaceOutputStandard o)
		{
			fixed4 base = tex2D (_MainTex, IN.uv_MainTex);
			fixed rim = 1.0 - saturate(dot(normalize(IN.viewDir), o.Normal));

			o.Albedo = base.rgb * _Color;
			o.Emission = base.rgb * pow(rim, _RimPower) * _Rim;
		}

		ENDCG
	}

	FallBack "Diffuse"
}
