Shader "Custom/Burn"
{
	Properties
	{
	_MainTex("Texture", 2D) = "white" {}
	_NoiseTex("Texture", 2D) = "white" {}
	_EdgeColor("Edge color", Color) = (1.0, 1.0, 1.0, 1.0)
	_Level("Dissolution level", Range(0.0, 1.0)) = 0.1
	_Edges("Edge width", Range(0.0, 1.0)) = 0.1
	_Color("Color", Color) = (1,1,1,1)
	}

		SubShader
	{
	Tags{ "Queue" = "Transparent" "IgnoreProjector" = "true" "RenderType" = "Transparent" }
	LOD 100

	Pass
		{
		Cull Off
		Lighting Off
		ZWrite Off
		Fog{ Mode Off }

		CGPROGRAM
		#pragma vertex vert
		#pragma fragment frag
		#pragma multi_compile DUMMY PIXELSNAP_ON
		#pragma fragmentoption ARB_precision_hint_fastest
		#include "UnityCG.cginc"

		struct appdata
			{
			float4 vertex : POSITION;
			float2 uv : TEXCOORD0;
			float4 color: COLOR;
			float2 texcoord : TEXCOORD0;
			};


		struct v2f
			{
			float2 uv : TEXCOORD0;
			float4 vertex : SV_POSITION;
			};

		sampler2D _MainTex;
		sampler2D _NoiseTex;
		float4 _EdgeColor;
		float _Level;
		float _Edges;
		float4 _MainTex_ST;
		float _Size;
		fixed4 _Color;
		fixed _Alpha;

		v2f vert(appdata v)
			{
			v2f o;

			o.vertex = UnityObjectToClipPos(v.vertex);
			o.uv = TRANSFORM_TEX(v.uv, _MainTex);

			return o;
			}

		fixed4 frag(v2f i) : SV_Target
			{
			float cutout = tex2D(_NoiseTex, i.uv).r;
			fixed4 col = tex2D(_MainTex, i.uv);

			if (cutout < _Level)
				discard;

			if (cutout < col.a && cutout < _Level + _Edges)
				col = lerp(_EdgeColor, _Color, (cutout - _Level) / _Edges);
			else
				col.rgb = _Color.rgb;

			return col;
			}
		ENDCG
		}
	}
}