Shader "Custom/RainOverlayGlassy"
{
    Properties
    {
        _MainTex("Raindrop Mask (Alpha)", 2D) = "white" {}
        _NormalMap("Normal Map", 2D) = "bump" {}
        _Tiling("Tiling", Vector) = (1,1,0,0)
        _Speed("Speed", Vector) = (0, -0.05, 0, 0)
        _Alpha("Alpha", Range(0,1)) = 0.8
    }

    SubShader
    {
        Tags { "Queue" = "Transparent" "RenderType" = "Transparent" }
        LOD 200
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            sampler2D _NormalMap;
            float4 _MainTex_ST;
            float4 _Tiling;
            float4 _Speed;
            float _Alpha;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv * _Tiling.xy + _Time.y * _Speed.xy;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 rain = tex2D(_MainTex, i.uv);
                float alpha = rain.a * _Alpha;

                // Хитрая подсветка капель с имитацией объёма
                float3 normal = UnpackNormal(tex2D(_NormalMap, i.uv));
                float lighting = dot(normal, float3(0, 0, 1)) * 0.5 + 0.5;

                fixed3 col = lerp(float3(1,1,1), float3(0.7,0.8,1.0), lighting);

                return fixed4(col, alpha);
            }
            ENDCG
        }
    }
}
