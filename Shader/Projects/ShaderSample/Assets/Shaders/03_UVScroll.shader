Shader "Custom/03_UVScroll"
{
    Properties
    {
        // テクスチャはインスペクターで設定
        _MainTex ("Texture", 2D) = "white" {}

        // スクロール向きと速度はインスペクターで設定
        _ScrollSpeed ("Scroll Speed", Vector) = (0.1, 0.0, 0, 0)
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

            sampler2D _MainTex;
            float4 _ScrollSpeed;

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

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);

                // 時間経過でUV座標をずらしていく
                // _TimeはUnity側で用意されている経過時間が格納されている変数
                o.uv = v.uv + _ScrollSpeed.xy * _Time.x;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // UV座標を元にテクスチャのどの部分の色を使うか決めて返却
                return tex2D(_MainTex, i.uv);
            }
            ENDCG
        }
    }
}
