Shader "Unlit/TestingTextureSample"
{
    Properties
    {
        _MainTexture("Main Texture", 2D) = "White" {}
        _AnimateXY("Animate X Y", Vector) = (0, 0, 0, 0)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
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

            sampler2D _MainTexture;
            float4 _MainTexture_ST;
            float4 _AnimateXY;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.uv = TRANSFORM_TEX(v.uv, _MainTexture);
                
                o.uv += frac(_AnimateXY.xy * _MainTexture_ST *  _Time.yy); //Time goes funky the longe unity is open
                //o.uv += frac(_AnimateXY.xy * _Time.yy); //Time goes funky the longe unity is open

                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                // sample the texture
                float2 uv = i.uv;
                //uv.xy *= 2;
                //uv.x += 0.5;
                //return fixed4(uv, 0, 1);
                fixed4 textCol = tex2D(_MainTexture, uv);

                //fixed4 col = fixed4(i.uv, 0, 1);

                return textCol;
            }
            ENDCG
        }
    }
}
