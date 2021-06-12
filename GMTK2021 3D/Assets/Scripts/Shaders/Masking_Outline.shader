Shader "Custom/Masking_Outline"
{
	Properties{
		_Color("Main Color", Color) = (.5,.5,.5,1)
		_OutlineColor("Outline Color", Color) = (0,0,0,1)
		_Outline("Outline width", Range(1.0, 1.5)) = 1.01
		_MainTex("Base (RGB)", 2D) = "white" { }
	}

	CGINCLUDE
	#include "UnityCG.cginc"

	struct appdata {
		float4 vertex : POSITION;
		float3 normal : NORMAL;
	};

	struct v2f {
		float4 pos : POSITION;
		float3 normal : NORMAL;
	};

	uniform float _Outline;
	uniform float4 _OutlineColor;

	uniform fixed4 GLOBAL_WorldPos;
	uniform half GLOBAL_Radius;
	uniform half GLOBAL_FallOff;

	v2f vert(appdata v) {
		v.vertex.xyz *= _Outline;
		v2f o;
		o.pos = UnityObjectToClipPos(v.vertex);
		return o;
	}


	ENDCG

		SubShader{
			Tags { "Queue" = "Transparent" }

			// note that a vertex shader is specified here but its using the one above
			Pass {
				Name "OUTLINE"

				Tags { "LightMode" = "Always" }
				Cull Off
				ZWrite Off
				ZTest Always
				ColorMask RGB 
				
				Blend SrcAlpha OneMinusSrcAlpha

				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag

				half4 frag(v2f i) :COLOR {
					return _OutlineColor;
				}
				ENDCG
			}
			Pass {
					Name "Base"
					ZWrite On
					ZTest LEqual
					Material{
						Diffuse[_Color]
						Ambient[_Color]
					}

					Lighting On

					SetTexture[_MainTex]
					{
						ConstantColor[_Color]
					}
					SetTexture[_MainTex]
					{
						Combine previous * primary DOUBLE
					}
			}
		}
		Fallback "Diffuse"
}