// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Custom/PullVertecesShader" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_PullPoint("Pull point pos", Vector) = (0,1,0)
		_PullForce("Amount of pull", float) = 0
		_PullStartDistance("Pull start distance", float) = 1

	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Lambert vertex:vert

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;
		float3 _PullPoint;
		float _PullForce;
		float _PullStartDistance;

		struct Input {
			float2 uv_MainTex;
		};

		fixed4 _Color;

		#include "Common.cginc"

		void vert(inout appdata_full v)
		{
			float3 vertexPositionWorld = mul (unity_ObjectToWorld, v.vertex).xyz;
			float distance = length(_PullPoint - vertexPositionWorld);
			if (distance < _PullStartDistance)
			{
				float3 direction = normalize(_PullPoint - vertexPositionWorld);
				direction *= RandomUnitVector(unity_DeltaTime[2]);
				v.vertex.xyz += mul(unity_WorldToObject, direction) * _PullForce;
			}
		}

		void surf (Input IN, inout SurfaceOutput o) {
			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			o.Alpha = c.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
