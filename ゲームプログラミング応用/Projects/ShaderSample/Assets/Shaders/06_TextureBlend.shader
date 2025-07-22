Shader "Custom/06_TextureBlend"
{
    Properties
    {
        _TexA("Texture A", 2D) = "white" {}              // ブレンド元のテクスチャ
        _TexB("Texture B", 2D) = "black" {}              // ブレンド先のテクスチャ
        _NoiseTex("Noise Texture", 2D) = "gray" {}       // ブレンド率に使うノイズテクスチャ（グレースケール）
        _BlendStrength("Blend Strength", Range(0,1)) = 1.0 // ブレンド強度（0=完全TexA、1=ノイズに従ってブレンド）
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" } // 不透明として扱う
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            sampler2D _TexA;         // テクスチャA
            sampler2D _TexB;         // テクスチャB
            sampler2D _NoiseTex;     // ノイズテクスチャ
            float _BlendStrength;    // ブレンドの強さ

            struct appdata
            {
                float4 vertex : POSITION;  // 頂点座標
                float2 uv : TEXCOORD0;     // UV座標
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;      // フラグメント用UV座標
                float4 vertex : SV_POSITION; // クリップ空間座標
            };

            // 頂点シェーダー
            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex); // 画面描画用変換
                o.uv = v.uv;                               // UVをそのまま渡す
                return o;
            }

            // フラグメントシェーダー
            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 colA = tex2D(_TexA, i.uv);               // テクスチャAの色取得
                fixed4 colB = tex2D(_TexB, i.uv);               // テクスチャBの色取得
                fixed noise = tex2D(_NoiseTex, i.uv).r;         // ノイズの明るさ（Rチャンネル）

                fixed blendFactor = saturate(noise * _BlendStrength); // ブレンド係数を0～1にクランプ
                return lerp(colA, colB, blendFactor);                 // ノイズに基づき線形補間で合成
            }
            ENDCG
        }
    }
}
