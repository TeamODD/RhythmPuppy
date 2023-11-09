Shader "Custom/Grayscale" 
{
	Properties 
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Grayscale("Grayscale", Range(0.0, 1.0)) = 0.0
	}

	SubShader 
	{
		Pass
		{
			CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag
			#include "UnityCG.cginc"

			sampler2D _MainTex;
			float _Grayscale;

			fixed4 frag(v2f_img i) : COLOR
			{
				fixed4 currentText = tex2D(_MainTex, i.uv);

				// simple grayscale
				float grayscale = (currentText.r + currentText.g + currentText.b) / 3;

				//YUV 
				//float grayscale = 0.299 * currentText.r + 0.587 * currentText.g + 0.114 * currentText.b;
				
				fixed4 color = lerp(currentText, grayscale, _Grayscale);

				currentText.rgb = color;
				
				return currentText;
			}
		
		ENDCG
		}
	}
	FallBack off
}