Shader "Chris/reactive_object"
{
    Properties
    {
        //exposing the properties
        _MainTex ("Texture", 2D) = "white" {}
        _Mult ("vMultiply", Range(0,5)) = 0.5
        _Clamp ("vClamp", Range(0,5)) = 1
        _HClamp ("hClamp", Range(0,5)) = 1
        _Motion ("vMotion", Range(0,1)) = .2

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
            // make fog work
            #pragma multi_compile_fog
            half _Mult = 4;
            half _Clamp = 1;
            half _HClamp = 1;
            half _Motion = .2;

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
                float3 worldPos : TEXCOORD2;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            uniform float4 _ObjectPos;

            //vertex shader
            v2f vert (appdata v)
            {
                v2f o;
                o.worldPos = mul(unity_ObjectToWorld, v.vertex);

                float dist = distance(_ObjectPos, o.worldPos);
                dist = saturate(_Clamp-dist);

                dist *= _Mult;
                v.vertex.y -= dist;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);

                return o;
            }

            //pixel shader / fragment shader
            fixed4 frag (v2f i) : SV_Target
            {

                float dist = distance(_ObjectPos, i.worldPos);

                //invert the pixels
                float highlight = saturate(_HClamp-dist);
                
                

                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);


                
                float2 uv = i.uv - 0.5;
                float d = length(uv);

                //col = float4(col.r,col.g,col.b, col.a)*highlight;

                return col - highlight;
            }
            ENDCG
        }
    }
}
