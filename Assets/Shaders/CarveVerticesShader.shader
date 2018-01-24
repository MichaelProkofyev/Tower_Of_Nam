// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Custom/CarveVertecesShader" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_CarvePoint("Pull point pos", Vector) = (0,1,0)
		_CarveStartDistance("Pull start distance", float) = 1

	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard vertex:vert

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;
		float3 _CarvePoint;
		float _PullForce;
		float _CarveStartDistance;

		struct Input {
			float2 uv_MainTex;
		};

		fixed4 _Color;

		void vert(inout appdata_full v)
		{
			float3 vertexPositionWorld = mul (unity_ObjectToWorld, v.vertex).xyz;
			float distance = length(_CarvePoint - vertexPositionWorld);
			if (distance < _CarveStartDistance)
			{
				v.vertex.xyz = float3(0,0,0);
			}
		}

		void surf (Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			o.Alpha = c.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
