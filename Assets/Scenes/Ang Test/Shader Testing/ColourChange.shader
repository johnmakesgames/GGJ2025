Shader "Unlit/ColourChange"
{
    Properties
    {
        _MainTex("Main Texture", 2D) = "White" {}
        _RainbowTexture("Rainbow Texture", 2D) = "Red" {}
        _AnimateXY("Animate X Y", Vector) = (0, 0, 0, 0)
        _ColourMask("Colour Mask", Vector) = (0, 0, 0, 0)
    }
        SubShader
        {
            Tags { "RenderType" = "Opaque" }
            LOD 100
            Cull Off

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
                sampler2D _RainbowTexture;
                float4 _MainTex_ST;
                float4 _AnimateXY;
                float4 _ColourMask;


                int currentColourIndex = 0;
                float currentTime = 0;

                v2f vert(appdata v)
                {
                    v2f o;
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    o.uv = v.uv;
                    o.uv = TRANSFORM_TEX(v.uv, _MainTex);

                    return o;
                }

                fixed4 frag(v2f i) : SV_Target
                {
                    //Increment time
                    currentTime += _Time.y; //(0, 1) exclusive

                    // sample the texture
                    float2 uv = i.uv;
                    float2 rainbowUV = frac(uv + float2(currentTime, 0))/0.999999999f;
                    //uv.xy *= 2;
                    //uv.x += 0.5;
                    //return fixed4(uv, 0, 1);
                    fixed4 textCol = tex2D(_MainTex, uv);
                    if (textCol.w == 0)
                    {
                        discard;
                    }


                    fixed4 rainbowCol = tex2D(_RainbowTexture, rainbowUV);


                    _ColourMask = rainbowCol;



                    textCol += _ColourMask;


                    return textCol;
                }
                ENDCG
            }
        }
}
