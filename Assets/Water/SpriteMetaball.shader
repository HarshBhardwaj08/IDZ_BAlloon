// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "CustomRenderTexture/SpriteMetaball"
{
    Properties
    {
     _MainTex("Texture", 2D) = "white" {}
     _Color("Tint Color", Color) = (1,1,1,1)
     _EdgeThreshold("EdgeThreshold", Range(0,1)) = 0.2
     [MaterialToggle] PixelSnap("Pixel snap", Float) = 0
     _CenterSaturation("CenterSaturation", Range(1,100)) = 2
     _StepSize("StepSize", Range(0.1,1)) = 1
    }
        SubShader
     {
      Tags
      {
       "Queue" = "Transparent"
       "IgnoreProjector" = "True"
       "RenderType" = "Transparent"
       "PreviewType" = "Plane"
       "CanUseSpriteAtlas" = "True"
      }

      Cull Off
      Lighting Off
      ZWrite Off
      Blend One OneMinusSrcAlpha

      Pass
      {
       CGPROGRAM
       #pragma vertex vert
       #pragma fragment frag
       #pragma multi_compile DUMMY PIXELSNAP_ON
       #pragma shader_feature ETC1_EXTERNAL_ALPHA

       #include "UnityCG.cginc"

       struct appdata
       {
        float4 vertex : POSITION;
        fixed4 color : COLOR;
        float2 uv : TEXCOORD0;
       };

       struct v2f
       {
        float2 uv : TEXCOORD0;
        fixed4 color : COLOR;
        float4 vertex : SV_POSITION;
       };

       sampler2D _MainTex;
       float4 _MainTex_ST;
       //tint color
       float4 _Color;
       //when texture color's alpha is below _EdgeThreshold, that color is discarded and the edge of rendered result formed
       //but it turns out that I got the result I want with _EdgeThreshold as 0...
       half _EdgeThreshold;
       /*
        higher _CenterSaturation makes the color from texture saturated faster(easier to become 1),
        since the default sprite texture this shader used has higher alpha in the center,
        this value determined the how much the center part of rendered result is saturated
       */
       half _CenterSaturation;
       //I think this value originally is related to metaball isosurface calculation,
       //but it may not be so useful in current implementation since I multiply texture's alpha which is formed as radial gradient 
       half _StepSize;

       sampler2D _AlphaTex;

       v2f vert(appdata v)
       {
        v2f o;
        o.vertex = UnityObjectToClipPos(v.vertex);
        o.uv = TRANSFORM_TEX(v.uv, _MainTex);
        #ifdef PIXELSNAP_ON
        o.vertex = UnityPixelSnap(o.vertex);
        #endif
        return o;
       }

       float4 SampleSpriteTexture(float2 uv)
       {
        float4 color = tex2D(_MainTex, uv);

    #if ETC1_EXTERNAL_ALPHA
        // get the color from an external texture (usecase: Alpha support for ETC1 on android)
        color.a = tex2D(_AlphaTex, uv).r;
    #endif //ETC1_EXTERNAL_ALPHA

        return color;
       }

       float4 frag(v2f i) : SV_Target
       {
        float4 col = SampleSpriteTexture(i.uv);

        float4 finalColor = col;
        if (finalColor.a > _EdgeThreshold) {
            //tint color
            finalColor *= _Color;
            //metaball isosurface
            finalColor.rgb *= float3(floor(finalColor.r * _CenterSaturation) * _StepSize, floor(finalColor.g * _CenterSaturation) * _StepSize, floor(finalColor.b * _CenterSaturation) * _StepSize);
            //this line kind of makes the isosurface of metaball formed by floor function and _StepSize meaningless, but I think it makes the rendered result looks better    
            finalColor.rgb *= finalColor.a;
           }
           else {
            discard;
           }

           return finalColor;
          }
          ENDCG
         }
     }
         Fallback "VertexLit"
}
