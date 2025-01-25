Shader "Unlit/ColourChange"
{
    Properties
    {
        _MainTexture("Main Texture", 2D) = "White" {}
        _RainbowTexture("Rainbow Texture", 2D) = "Red" {}
        _AnimateXY("Animate X Y", Vector) = (0, 0, 0, 0)
        _ColourMask("Colour Mask", Vector) = (0, 0, 0, 0)
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

                sampler2D _MainTexture;
                sampler2D _RainbowTexture;
                float4 _MainTexture_ST;
                float4 _AnimateXY;
                float4 _ColourMask;

                //float4 _ColourArray[5];
                //{
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
                    o.uv = TRANSFORM_TEX(v.uv, _MainTexture);

                    //o.uv += frac(_AnimateXY.xy * _MainTexture_ST * _Time.yy); //Time goes funky the longe unity is open


                    //o.uv += frac(_AnimateXY.xy * _MainTexture_ST * _Time.yy); //Time goes funky the longe unity is open

                    return o;
                }

                fixed4 frag(v2f i) : SV_Target
                {
                    //Increment time
                    currentTime += frac(_Time.y);
                     
                    
                    // sample the texture
                    float2 uv = i.uv;
                    float2 rainbowUV = float2(currentTime, 0);
                    //uv.xy *= 2;
                    //uv.x += 0.5;
                    //return fixed4(uv, 0, 1);
                    fixed4 textCol = tex2D(_MainTexture, uv);

                    fixed4 rainbowCol = tex2D(_RainbowTexture, rainbowUV);

                    //textCol.xy += frac(_Time.yy);


                   //
                   // if (currentTime >= 1)
                   // {
                   //     currentColourIndex += 1;
                   //
                   //     if (currentColourIndex > 4)
                   //     {
                   //         currentColourIndex = 0;
                   //     }
                   // }

                    _ColourMask = rainbowCol;//float4(1, 0, 0, 0);// colourArray[currentColourIndex];


                    //if (_ColourMask.x >= 0.9)
                    //{
                    //    _ColourMask.y += frac(_Time.y);
                    //    _ColourMask.x = 1.0;
                    //}
                    //
                    //if (_ColourMask.y >= 0.9)
                    //{
                    //    _ColourMask.x -= 1;
                    //    _ColourMask.y = 1.0;
                    //}


                    
                    //_ColourMask.x -= frac(_Time.y);
                    //
                    //_ColourMask.z += frac(_Time.y);
                    //
                    //_ColourMask.y -= frac(_Time.y);
                    //
                    //_ColourMask.x += frac(_Time.y);
                    //
                    //_ColourMask.z -= frac(_Time.y);


                    //float4 colourMask = ()
                    //textCol.w = 0.2;
                    //textCol.x += frac(_Time.y);
                    //textCol.y += frac(_Time.y);
                    //textCol.z += frac(_Time.y);

                    //fixed4 col = fixed4(i.uv, 0, 1);

                    textCol += _ColourMask;


                    return textCol;
                }
                ENDCG
            }
        }
}
