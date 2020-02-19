Shader "DesktopMascotMaker/MascotMakerShader"
{
	Properties
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
	}

	SubShader
	{
		Pass
		{
			ZTest Always Cull Off ZWrite Off
			Fog { Mode off }
			
			CGPROGRAM

				#pragma vertex vert_img
				#pragma fragment frag
				#pragma fragmentoption ARB_precision_hint_fastest 
				#include "UnityCG.cginc"

				sampler2D _MainTex;

				fixed4 frag(v2f_img i):COLOR
				{
					half4 input = tex2D(_MainTex, i.uv);
					input.a = max((input.r+input.g+input.b)/3,input.a);

					//half3 output = (input.rrr * half3(0,0,1))
					//			 + (input.ggg * half3(0,1,0))
					//			 + (input.bbb * half3(1,0,0));
					//return fixed4(output, input.a);
					
					// return fixed4(input.aaa,1);
					return input;
				}

			ENDCG
		}
	}
	FallBack off
}
