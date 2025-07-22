Shader "Custom/04_HeightMap"
{
    Properties
    {
        // メインテクスチャ
        _MainTex ("Water Texture", 2D) = "white" {}

        // ハイトマップ
        _HeightMap ("Height Map", 2D) = "gray" {}

        // UVスクロール速度
        _ScrollSpeed ("Scroll Speed", Vector) = (0.05, 0.02, 0, 0)

        // ハイトマップの強さ（適用度）
        _HeightStrength ("Height Strength", Range(0,1)) = 0.1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            sampler2D _HeightMap;
            float4 _ScrollSpeed;
            float _HeightStrength;

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

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);

                // スクロールUV計算
                float2 scrollUV = v.uv + _ScrollSpeed.xy * _Time.x;
                o.uv = scrollUV;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                // 水面テクスチャ取得
                fixed4 baseCol = tex2D(_MainTex, i.uv);

                // ハイトマップ（高さ）取得
                // 今回は赤成分のみを取得する
                // 白い部分ほど色の値が大きいので高い値が取得できる
                fixed height = tex2D(_HeightMap, i.uv).r;

                // 高さで色を少し明るく（波の凹凸表現）
                fixed4 col = baseCol + height * _HeightStrength;

                return col;
            }
            ENDCG
        }
    }
}
