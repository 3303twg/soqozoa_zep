Shader "Custom/SpriteOutline"
{
    Properties
    {
        _MainTex ("Sprite Texture", 2D) = "white" {}
        _OutlineColor ("Outline Color", Color) = (0,0,0,1)
        _OutlineSize ("Outline Size", Float) = 0.05
    }

    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        LOD 100

        Pass
        {
            ZWrite Off
            Cull Off
            Lighting Off
            Blend SrcAlpha OneMinusSrcAlpha

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
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            sampler2D _MainTex;
            float4 _MainTex_TexelSize;
            fixed4 _OutlineColor;
            float _OutlineSize;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float alpha = tex2D(_MainTex, i.uv).a;

                // 주변 픽셀 체크
                float outline = 0.0;
                float2 offsets[8] = {
                    float2(-1, 0), float2(1, 0),
                    float2(0, -1), float2(0, 1),
                    float2(-1, -1), float2(-1, 1),
                    float2(1, -1), float2(1, 1)
                };

                for (int j = 0; j < 8; j++)
                {
                    float2 offsetUV = i.uv + offsets[j] * _OutlineSize * _MainTex_TexelSize.xy;
                    outline += tex2D(_MainTex, offsetUV).a;
                }

                // 외곽선 표시 조건
                if (alpha < 0.1 && outline > 0.1)
                {
                    fixed4 outColor = _OutlineColor;
                    outColor.a = 1.0; // 불투명하게 설정
                    return outColor;
                }

                return tex2D(_MainTex, i.uv);
            }
            ENDCG
        }
    }
}