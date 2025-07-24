Shader "Custom/09_RampToon"
{
    Properties
    {
        // テクスチャなしでも使える単色カラー
        _Color ("Main Color", Color) = (1,1,1,1)

        // Rampテクスチャ（影のトーンを決める1D画像）
        _RampTex ("Ramp Texture", 2D) = "white" {}
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            // 頂点シェーダー入力：モデル座標と法線
            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS   : NORMAL;
            };

            // 頂点→ピクセルに渡す情報
            struct Varyings
            {
                float4 positionHCS : SV_POSITION;  // 画面座標
                float3 normalWS    : TEXCOORD0;    // 法線（World Space）
            };

            // プロパティ変数（Material Inspectorから設定可能）
            float4 _Color;

            sampler2D _RampTex;

            // 頂点シェーダー
            Varyings vert (Attributes IN)
            {
                Varyings OUT;

                // モデル座標 → 画面座標に変換
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);

                // 法線をWorld Spaceに変換
                OUT.normalWS = TransformObjectToWorldNormal(IN.normalOS);

                return OUT;
            }

            // ピクセルシェーダー（1ピクセルごとの色計算）
            half4 frag (Varyings IN) : SV_Target
            {
                // 法線と光の方向の内積（どれだけ光が当たっているか）
                float3 normal = normalize(IN.normalWS);
                float3 lightDir = normalize(_MainLightPosition.xyz); // 主光源の方向
                float NdotL = dot(normal, lightDir); // 負値を0にクリップ
                float rampU = saturate(NdotL * 0.5 + 0.5); // -1~1 → 0~1
                
                // Rampテクスチャで陰影の色を決定
                // 横1行のテクスチャなので、Y=0.5で固定して読み取り
                float3 rampColor = tex2D(_RampTex, float2(rampU, 0.5)).rgb;

                // ベースカラーと合成して最終色に
                return float4(_Color.rgb * rampColor, 1.0);
            }
            ENDHLSL
        }
    }
}
