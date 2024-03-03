Shader "Custom/LightSurface"
{
	Properties
	{
		_ASEOutlineColor( "Outline Color", Color ) = (0.8962264,0.7633126,0.2832412,0)
		_ASEOutlineWidth( "Outline Width", Float ) = 0.01
		_Cutoff( "Mask Clip Value", Float ) = 0.5
		_Color("Base Color", Color) = (1,1,1,0)
		_MainTex("Base Color Map", 2D) = "white" {}
		_MaskMap("Mask Map", 2D) = "white" {}
		[Normal]_BumpMap("Normal Map", 2D) = "bump" {}
		_LineMap("Line Map", 2D) = "white" {}
		[HDR]_LineColor("Line Color", Color) = (1.991077,1.306363,0.3513666,1)
		_LineAlpha("Line Alpha", Range( 0 , 1)) = 1
		_LineSize("Line Size", Range( 0.1 , 2)) = 1
		_LineSpeed("Line Speed", Range( 0.1 , 2)) = 1
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ }
		Cull Front
		CGPROGRAM
		#pragma target 3.0
		#pragma surface outlineSurf Outline  keepalpha noshadow noambient novertexlights nolightmap nodynlightmap nodirlightmap nometa noforwardadd vertex:outlineVertexDataFunc 
		
		
		
		
		struct Input {
			half filler;
		};
		float4 _ASEOutlineColor;
		float _ASEOutlineWidth;
		void outlineVertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			v.vertex.xyz += ( v.normal * _ASEOutlineWidth );
		}
		inline half4 LightingOutline( SurfaceOutput s, half3 lightDir, half atten ) { return half4 ( 0,0,0, s.Alpha); }
		void outlineSurf( Input i, inout SurfaceOutput o )
		{
			o.Emission = _ASEOutlineColor.rgb;
			o.Alpha = 1;
		}
		ENDCG
		

		Tags{ "RenderType" = "TransparentCutout"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows exclude_path:deferred 
		struct Input
		{
			float2 uv_texcoord;
			float3 worldPos;
		};

		uniform sampler2D _BumpMap;
		uniform float4 _BumpMap_ST;
		uniform float4 _Color;
		uniform sampler2D _MainTex;
		uniform float4 _MainTex_ST;
		uniform float _LineAlpha;
		uniform sampler2D _LineMap;
		uniform float _LineSpeed;
		uniform float _LineSize;
		uniform float4 _LineColor;
		uniform sampler2D _MaskMap;
		uniform float4 _MaskMap_ST;
		uniform float _Cutoff = 0.5;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_BumpMap = i.uv_texcoord * _BumpMap_ST.xy + _BumpMap_ST.zw;
			float3 tex2DNode2 = UnpackNormal( tex2D( _BumpMap, uv_BumpMap ) );
			float3 NormalMap266 = tex2DNode2;
			o.Normal = NormalMap266;
			float2 uv_MainTex = i.uv_texcoord * _MainTex_ST.xy + _MainTex_ST.zw;
			float4 tex2DNode1 = tex2D( _MainTex, uv_MainTex );
			o.Albedo = ( _Color * tex2DNode1 ).rgb;
			float2 temp_cast_1 = (_LineSpeed).xx;
			float3 ase_vertex3Pos = mul( unity_WorldToObject, float4( i.worldPos , 1 ) );
			float2 temp_cast_2 = (( ase_vertex3Pos.x * _LineSize )).xx;
			float2 panner243 = ( _Time.y * temp_cast_1 + temp_cast_2);
			float4 temp_output_119_0 = ( tex2D( _LineMap, panner243 ) * _LineColor );
			float3 desaturateInitialColor282 = float3( (tex2DNode2).xy ,  0.0 );
			float desaturateDot282 = dot( desaturateInitialColor282, float3( 0.299, 0.587, 0.114 ));
			float3 desaturateVar282 = lerp( desaturateInitialColor282, desaturateDot282.xxx, 1.0 );
			float4 lerpResult248 = lerp( float4( 0,0,0,0 ) , ( float4( 0,0,0,0 ) * temp_output_119_0 ) , float4( saturate( desaturateVar282 ) , 0.0 ));
			float2 uv_MaskMap = i.uv_texcoord * _MaskMap_ST.xy + _MaskMap_ST.zw;
			float4 tex2DNode3 = tex2D( _MaskMap, uv_MaskMap );
			float4 lerpResult278 = lerp( float4( 0,0,0,0 ) , ( lerpResult248 + ( temp_output_119_0 * float4( 0.245283,0.245283,0.245283,0 ) ) ) , tex2DNode3.g);
			float4 Line32 = ( _LineAlpha * lerpResult278 );
			o.Emission = Line32.rgb;
			o.Metallic = tex2DNode3.r;
			o.Smoothness = tex2DNode3.a;
			o.Occlusion = tex2DNode3.g;
			o.Alpha = 1;
			clip( tex2DNode1.a - _Cutoff );
		}

		ENDCG
	}
	Fallback "Diffuse"
}
