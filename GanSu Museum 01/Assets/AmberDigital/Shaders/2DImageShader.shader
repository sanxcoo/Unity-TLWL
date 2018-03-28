Shader "Amber Shader/2DImageShader"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		[MaterialToggle]
		_FlipX("Flip X", Int) = 0
		[MaterialToggle]
		_FlipY("Flip Y", Int) = 0
	}
	SubShader
	{
		Tags { "RenderType"="Overlay" "Queue" = "Transparent" }
		LOD 100

		Cull Off
		ZWrite Off
		ZTest Always
		Blend SrcAlpha OneMinusSrcAlpha

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			//#pragma multi_compile_fog

			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				float4 diffuse : COLOR;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				//UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
				float4 diffuse : TEXCOORD1;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			float _FlipX;
			float _FlipY;

			float2 flipUV(float2 uv)
			{
				float2 ret = uv;

				if (_FlipX > 0)
					ret.x = 1.0f - ret.x;
				if (_FlipY > 0)
					ret.y = 1.0f - ret.y;
				
				return ret;
			}
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				float2 uv = flipUV(v.uv);
				o.uv = TRANSFORM_TEX(uv, _MainTex);
				//UNITY_TRANSFER_FOG(o,o.vertex);
				o.diffuse = v.diffuse;
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv);
				// apply fog
				//UNITY_APPLY_FOG(i.fogCoord, col);
				return col * i.diffuse;
			}
			ENDCG
		}
	}
}
