Shader "Unlit/Square2CircleShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _AlphaScale("Alpha Scale",Range(0,1)) = 1
    }
    SubShader
    {
        Tags {"Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent"}
        LOD 100

        Pass
        {
            ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            fixed _AlphaScale;

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
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed2 uv = (i.uv - fixed2(0.5f, 0.5f)) * 2;
                fixed l = sqrt(uv.x * uv.x + uv.y * uv.y);
                if (l >= 1)
                {
                    discard;
                }

                if (abs(uv.x) >= abs(uv.y))
                {
                    if (uv.y >= 0) uv.y = abs(uv.y * l / uv.x);
                    else uv.y = -abs(uv.y * l / uv.x);

                    if (uv.x >= 0) uv.x = l;
                    else uv.x = -l;
                }
                else
                {
                    if (uv.x >= 0) uv.x = abs(uv.x * l / uv.y);
                    else uv.x = -abs(uv.x * l / uv.y);

                    if (uv.y >= 0) uv.y = l;
                    else uv.y = -l;
                }

                // sample the texture
                fixed4 col = tex2D(_MainTex, uv * 0.5 + fixed2(0.5f, 0.5f));
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);

                col.a = _AlphaScale;;
                return col;
            }
            ENDCG
        }
    }
}
