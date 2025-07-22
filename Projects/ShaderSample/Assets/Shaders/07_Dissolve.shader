Shader "Custom/07_Dissolve"
{
    Properties
    {
        _MainTex ("Base Texture", 2D) = "white" {}              // メインとなるベースのテクスチャ
        _NoiseTex ("Noise Texture", 2D) = "gray" {}             // ディゾルブ処理に使うノイズテクスチャ
        _DissolveAmount ("Dissolve Amount", Range(0,1)) = 0     // ディゾルブ進行度（0で完全表示、1で完全消滅）
        _EdgeColor ("Edge Color", Color) = (1,1,1,1)            // エッジ部分の色（光る縁）
        _EdgeWidth ("Edge Width", Range(0,0.3)) = 0.05            // エッジの幅（しきい値からどれだけ広げるか）
    }

    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }  // 半透明としてレンダリング
        LOD 200
        Blend SrcAlpha OneMinusSrcAlpha                        // アルファブレンド
        ZWrite Off                                             // Zバッファへの書き込みを無効化（透過のため）

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            // テクスチャとプロパティの宣言
            sampler2D _MainTex;       // メインテクスチャ
            sampler2D _NoiseTex;      // ノイズテクスチャ
            float _DissolveAmount;    // ディゾルブの進行度
            fixed4 _EdgeColor;        // エッジの色
            float _EdgeWidth;         // エッジの幅

            // 頂点データ構造体（モデルから受け取る）
            struct appdata
            {
                float4 vertex : POSITION;  // 頂点座標
                float2 uv : TEXCOORD0;     // UV座標
            };

            // 頂点→フラグメントへ渡すデータ
            struct v2f
            {
                float2 uv : TEXCOORD0;     // UV座標
                float4 vertex : SV_POSITION; // 画面座標
            };

            // 頂点シェーダー：クリップ空間へ変換
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex); // モデル座標→ワールド→ビュー→クリップ変換
                o.uv = v.uv;                                // UVそのまま渡す
                return o;
            }

            // フラグメントシェーダー：色を出力
            fixed4 frag (v2f i) : SV_Target
            {
                // ノイズテクスチャからグレースケール値を取得（0〜1）
                float noise = tex2D(_NoiseTex, i.uv).r;

                float threshold = _DissolveAmount; // 現在の消失しきい値

                // smoothstepでエッジをなだらかに取得（しきい値近くを0〜1に変換）
                float edge = smoothstep(threshold - _EdgeWidth, threshold, noise);

                // メインテクスチャの色を取得
                fixed4 col = tex2D(_MainTex, i.uv);

                // エッジ部分の色をブレンド（エッジ：EdgeColor、それ以外：元の色）
                col.rgb = lerp(_EdgeColor.rgb, col.rgb, edge);

                // エッジに応じてアルファ値を変更（エッジ部分以外を徐々に透明に）
                col.a *= edge;

                // 完全に消す部分は描画しない
                if (edge <= 0.01) discard;

                return col;
            }
            ENDCG
        }
    }
}
