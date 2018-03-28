Shader "Amber Shader/TextCanvasSlide"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_YOffset ("V Offset", Float) = 1.07 //Range(1.07,0)
		_OffsetProgress("Offset progress", Range(1,0)) = 0
		_YScale ("Y Scale", Float) = 0.0
	}
	SubShader
	{
		Tags { "RenderType"="Overlay" "Queue" = "Transparent" }
		LOD 100

		Cull Off
		ZWrite Off
		ZTest Always
		Blend SrcAlpha OneMinusSrcAlpha
		Lighting Off

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				float4 color : COLOR;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
				float4 color : TEXCOORD1;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			float _YOffset;
			float _OffsetProgress;
			float _YScale;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				//o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.uv = v.uv;
				UNITY_TRANSFER_FOG(o,o.vertex);
				o.color = v.color;
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// Modify uv coordinates
				float2 uv = i.uv;
				
				// 预留滚动间隔空白
				_OffsetProgress = _OffsetProgress * 3 - 1;

				uv.y += _YOffset * _OffsetProgress;
				uv.y *= _YScale;
				//uv.y = frac(uv.y);
				

				// sample the texture
				fixed4 col = tex2D(_MainTex, frac(uv));

				// 上下边的淡入淡出效果
				// Fade edge: 0.1
				float fadeEdge = 0.1;
				if (i.uv.y < fadeEdge)
					col.a *= i.uv.y * 10;
				else if (i.uv.y > 1.0 - fadeEdge)
					col.a *= (1.0 - i.uv.y) * 10;

				if (uv.y > 1.0 || uv.y < 0.0)
					col.a = 0.0f;

				//// apply fog
				//UNITY_APPLY_FOG(i.fogCoord, col);
				return col * i.color;
			}
			ENDCG
		}
	}
}
