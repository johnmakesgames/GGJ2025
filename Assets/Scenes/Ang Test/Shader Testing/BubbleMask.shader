Shader "CustomRenderTexture/BubbleMask"
{
    Properties
    {
        _MainTex("Main Texture", 2D) = "White" {}
        _BubbleMask("Bubble Mask", 2D) = "Red" {}
        _Color ("Color", Color) = (1,1,1,1)
    }
SubShader
    {
        Tags { "RenderType" = "Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;

                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            sampler2D _RainbowTexture;
            float4 _Color;

            //float4 _ColourArray[5];
            //{dcxfxfxfxfxfxfxfxfxfxfxfxfxfxfxfxfxfxfxfxfxfxfxfxfxfxfxfxfxfxfxfxfxfxfxfxfxfxfxfxfxfxfxfxfxfxfxfxfxfxfxfxfxfxfxfxfxfxfxfxfxfxfxfxfxfxfxfxfxf                                                    
            //    {1, 0, 0, 0},
            //    {1, 1, 0, 0},
            //    {0, 1, 0, 0},
            //    {0, 1, 1, 0},
            //    {0, 0, 1, 0},
            //};

            int currentColourIndex = 0;
            float currentTime = 0;

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);

                //o.uv += frac(_AnimateXY.xy * _MainTexture_ST * _Time.yy); //Time goes funky the longe unity is open


                //o.uv += frac(_AnimateXY.xy * _MainTexture_ST * _Time.yy); //Time goes funky the longe unity is open

                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {

                float2 uv = i.uv;

                fixed4 textCol = tex2D(_MainTex, uv);
                if (textCol.w == 0)
                {
                    discard;
                }


                return textCol;
            }
            ENDCG
        }
    }
}