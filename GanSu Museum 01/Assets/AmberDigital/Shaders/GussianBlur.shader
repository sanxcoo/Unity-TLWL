Shader "Amber Shader/GussianBlur"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_TintColor("Tint", Color) = (1,1,1,1)
		_BlurRadius("Blur radius", Range(0,15)) = 1

		//_PassID("Pass", Int) = 0

		//[HideInInspector]
		//_Gussian1("", Vector) = (0.000118,0.010631,0.000118,0)
		//_Gussian2("", Vector) = (0.010631,0.957002,0.010631,0)
		//_Gussian3("", Vector) = (0.000118,0.010631,0.000118,0)
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
			// Upgrade NOTE: excluded shader from DX11, Xbox360, OpenGL ES 2.0 because it uses unsized arrays
			//#pragma exclude_renderers d3d11 xbox360 gles
			#pragma target 3.0
			#pragma vertex vert
			#pragma fragment frag
			

			// make fog work
			//#pragma multi_compile_fog
			
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
				//UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
				float4 color : TEXCOORD1;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			float4 _MainTex_TexelSize;
			float4 _TintColor;
			float _BlurRadius;

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				//UNITY_TRANSFER_FOG(o,o.vertex);
				o.color = v.color;
				return o;
			}

			//
			// 2-dimensional gussian generater
			//
			float Gussian2(int2 coord, float radius)
			{
				if (0 == radius)
					return 1.0f;

				float sigma = radius / 3.0f;
				float sigma2 = 2.0f * sigma * sigma;
				float sigmap = sigma2 * UNITY_PI;

				return exp(-(coord.x*coord.x + coord.y*coord.y) / sigma2) / sigmap;
			}
			
			
			//
			// Original method
			//
			fixed4 frag (v2f input) : SV_Target
			{
				float4 origCol = tex2D(_MainTex, input.uv);

				float3 blurCol = float3(0, 0, 0);
				float sum = 0.0f;
				int nRadius = floor(_BlurRadius);

				for (int i = -nRadius; i <= nRadius; i++) {
					for (int j = -nRadius; j <= nRadius; j++) {
						float gussian = Gussian2(int2(i, j), nRadius);

						sum += gussian;

						blurCol += gussian * tex2Dlod(_MainTex, float4(input.uv + float2(i, j) * _MainTex_TexelSize.xy, 0, 0)).rgb;
					}
				}
				blurCol /= sum;

				fixed4 finalCol = fixed4(blurCol, origCol.a);

				return finalCol * _TintColor * input.color;
			}

			ENDCG
		}
	}
}
