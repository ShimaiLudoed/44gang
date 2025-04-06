Shader "Custom/RainOverlay"
{
    Properties
    {
        _RainTex("Rain Texture", 2D) = "white" {}
        _Tiling("Tiling", Vector) = (1,1,0,0)
        _Speed("Speed", Vector) = (0,-0.05,0,0)
        _Alpha("Alpha", Range(0,1)) = 0.6
    }

    SubShader
    {
        Tags { "Queue" = "Transparent" "RenderType" = "Transparent" }
        LOD 100
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha
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

            sampler2D _RainTex;
            float4 _RainTex_ST;
            float4 _Tiling;
            float4 _Speed;
            float _Alpha;

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                float2 scrollUV = v.uv * _Tiling.xy + _Time.y * _Speed.xy;
                o.uv = scrollUV;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 col = tex2D(_RainTex, i.uv);
                col.a *= _Alpha;
                return col;
            }
            ENDCG
        }
    }
}
