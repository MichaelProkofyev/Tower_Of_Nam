// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Custom/PullNoiseShader" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_PullPoint("Pull point pos", Vector) = (0,1,0)
		_PullForce("Amount of pull", float) = 0
		_DistanceMultiplier("Distance mult", float) = 0.1
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
		float _DistanceMultiplier;
		float _PullStartDistance;

		struct Input {
			float2 uv_MainTex;
		};

		fixed4 _Color;

		#include "Common.cginc"

		void vert(inout appdata_full v)
		{

			float3 vertexPositionWorld = mul (unity_ObjectToWorld, v.vertex).xyz;
			float distanceAffect = (1/length(_PullPoint - vertexPositionWorld)) * _DistanceMultiplier;

			float angle= RandomUnitVector(_Time[3] + v.vertex.x + v.vertex.y + v.vertex.z);
			//v.vertex.y += sin(v.vertex.z / 2 + angle);
			v.vertex.x += RandomUnitVector(_Time[3] * 100 + v.vertex.x) * _PullForce * distanceAffect;
			v.vertex.y += RandomUnitVector(_Time[3] * 100 + v.vertex.y) * _PullForce * distanceAffect;
			v.vertex.z += RandomUnitVector(_Time[3] * 100 + v.vertex.z) * _PullForce * distanceAffect;
			
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
