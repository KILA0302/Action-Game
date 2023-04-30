Shader "PLAYER TWO/Platformer Project/Triplanar"
{
	Properties
	{
		_Color ("Color", Color) = (1, 1, 1, 1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Scale ("Scale", Float) = 1
	}

	SubShader
	{
		Tags { "RenderType" = "Opaque" }
		LOD 100

		CGPROGRAM

		#pragma surface surf Lambert

		sampler2D _MainTex;
		half4 _Color;
		float _Scale;

		struct Input
		{
			float2 uv_MainTex;
			float3 worldPos;
			float3 worldNormal;
		};

		void surf (Input IN, inout SurfaceOutput o)
		{
			float3 blendNormal = saturate(pow(IN.worldNormal * 1.4, 4));

			float4 x = tex2D(_MainTex, IN.worldPos.zy * _Scale);
			float4 y = tex2D(_MainTex, IN.worldPos.zx * _Scale);
			float4 z = tex2D(_MainTex, IN.worldPos.xy * _Scale);

			fixed4 c = z;
			c = lerp(c, x, blendNormal.x);
			c = lerp(c, y, blendNormal.y);

			o.Albedo = c.rgb * _Color;
		}

		ENDCG
	}

	FallBack "Diffuse"
}
