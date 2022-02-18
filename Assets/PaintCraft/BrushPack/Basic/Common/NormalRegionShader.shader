// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "PaintCraft/Basic NormalRegion" {	
	Properties {		
		_MainTex ("Swatch", 2D) = "white" {}			
		_RegionTex("Regions Texture", 2D) = "white" {}
	}

   SubShader {
	Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
	Cull Off
	Lighting Off
	ZWrite Off
    Blend One OneMinusSrcAlpha
    Pass {
     CGPROGRAM //Shader Start, Vertex Shader named vert, Fragment shader named frag

     #pragma vertex vert
     #pragma fragment frag
     #include "UnityCG.cginc"
     //Link properties to the shader
     
     sampler2D _MainTex;    
	 sampler2D 	_RegionTex;
     

     struct v2f 
     {
	     float4  pos : SV_POSITION;
	     float2  uv0 : TEXCOORD0;	   
		 float2  uv2 : TEXCOORD2;
		 float2  uv3 : TEXCOORD3;
	     fixed4  color : COLOR;
     };

     float4 _MainTex_ST;
	 float4 _RegionTex_ST;

     v2f vert (appdata_full v)
     {
	     v2f o;
	     o.pos = UnityObjectToClipPos (v.vertex); 
	     o.uv0= TRANSFORM_TEX (v.texcoord, _MainTex); 	  
		 o.uv2 = TRANSFORM_TEX(v.texcoord1, _MainTex);
		 o.uv3 = TRANSFORM_TEX(v.texcoord3, _MainTex);
	     o.color =  v.color;		
	     return o;
     }


     bool isInRegion(float2 uv, float2 origin){
         half4 region = tex2D(_RegionTex, uv);
         half4 original = tex2D(_RegionTex, origin);
         if (original.a == 0){
             return 0;
         }
         return distance(region, original) < 0.001;
       }

     half4 frag (v2f i) : COLOR
     {
         fixed4 color = i.color;
         color.a *= tex2D (_MainTex, i.uv0).a;
		 color *= isInRegion(i.uv2, i.uv3);
	 	 color.rgb *= color.a;	 	
         return color;
     }

     ENDCG //Shader End
    }
   }
}

