Shader "Unlit/UVMove"
{
	Properties
	{
		_MainTex ("Base (RGB), Alpha (A)", 2D) = "black" {}
		_SizeX ("Size_X",float) =1 
		_SizeY("Size_Y",float) =1 
		_Speed("Speed",float)=150
	}
	
	SubShader
	{
		LOD 200

		Tags
		{
			"Queue" = "Transparent"
			"IgnoreProjector" = "True"
			"RenderType" = "Transparent"
		}
		
		Pass
		{
			Cull Off
			Lighting Off
			ZWrite Off
			Fog { Mode Off }
			Offset -1, -1
			Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag			
			#include "UnityCG.cginc"

			sampler2D _MainTex;
			float4 _MainTex_ST;
			float _SizeX,_SizeY,_Speed;
	
			struct appdata_t
			{
				float4 vertex : POSITION;
				float2 texcoord : TEXCOORD0;
				fixed4 color : COLOR;
			};
	
			struct v2f
			{
				float4 vertex : SV_POSITION;
				half2 texcoord : TEXCOORD0;
				fixed4 color : COLOR;
			};
	
			v2f vert (appdata_t v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.texcoord = v.texcoord;
				o.color = v.color;
				return o;
			}
				
			fixed4 frag (v2f IN) : COLOR
			{ 
			     float2 cellUV = float2(IN.texcoord.x, IN.texcoord.y);
			     cellUV.x = cellUV.x + sin(_Time *(_Speed*_SizeX));
			     cellUV.y = cellUV.y + sin(_Time *(_Speed*_SizeY));
			     if(cellUV.y>1)
			        cellUV.y = cellUV.y -1;
			     if(cellUV.x>1)
			        cellUV.x = cellUV.x -1;
			     if(cellUV.y<0)
			     {
			         cellUV.y = cellUV.y +1;
			     }

			     if(cellUV.x<0)
			        cellUV.x = cellUV.x +1;
			          
			     fixed4 colo = tex2D(_MainTex, cellUV) * IN.color;
				 return colo;
			}
			ENDCG
		}
	}

	SubShader
	{
		LOD 100

		Tags
		{
			"Queue" = "Transparent"
			"IgnoreProjector" = "True"
			"RenderType" = "Transparent"
		}
		
		Pass
		{
			Cull Off
			Lighting Off
			ZWrite Off
			Fog { Mode Off }
			Offset -1, -1
			ColorMask RGB
			Blend SrcAlpha OneMinusSrcAlpha
			ColorMaterial AmbientAndDiffuse
			
			SetTexture [_MainTex]
			{
				Combine Texture * Primary
			}
		}
	}
}
