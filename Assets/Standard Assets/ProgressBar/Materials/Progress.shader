﻿// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/Progress" {
	Properties {
		_MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
		_MaskTex("Mask Tex", 2D)=""{}
		_Progress("Progress", Range(-0.01,1)) = 0.1
		_AlphaValue("AlphaValue", Range(-0.01,1)) = 0
	}
	
	CGINCLUDE
	#include "UnityCG.cginc"

	sampler2D _MainTex;
	sampler2D _MaskTex;
	float _Progress;
	float _AlphaValue;
	//Color _Color;

	struct v2f{
		float4 pos:SV_POSITION;
		float2 uv:TEXCOORD;
	};

	v2f vert(appdata_base i){
		v2f o;
		o.pos = UnityObjectToClipPos(i.vertex);
		o.uv = i.texcoord.xy;
		return o;
	}

	float4 frag(v2f i):COLOR{
		float4 c = tex2D(_MainTex,i.uv);
		//clip(_Progress - tex2D(_MaskTex,i.uv).a);
		if(_Progress - tex2D(_MaskTex,i.uv).a > 0){
			c.a = tex2D(_MainTex,i.uv).a * _AlphaValue;
		}
		else{
			c.a = 0;
		}

		
		return c;
	}


	ENDCG
	
	SubShader {
		Tags {"Queue"="Transparent" "RenderType"="Transparent" }
		LOD 200

		Pass{
			Blend SrcAlpha OneMinusSrcAlpha
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			ENDCG
		}

   		
	} 
FallBack "Diffuse"
}
