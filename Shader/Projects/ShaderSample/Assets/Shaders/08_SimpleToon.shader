Shader "Custom/08_SimpleToon"
{
    Properties
    {
        // オブジェクトの基本色
        _Color ("Main Color", Color) = (1,1,1,1)

        // 影になるかどうかを決める光の当たり具合のしきい値
        _ShadeThreshold ("Shade Threshold", Range(0,1)) = 0.5

        // 影側の色の割合（_Colorに乗算される）
        _ShadeColor ("Shade Color", Color) = (0.5, 0.5, 0.5, 1)
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        Pass
        {
            HLSLPROGRAM
            // 頂点/ピクセルシェーダー指定
            #pragma vertex vert
            #pragma fragment frag

            // URPの共通ライブラリをインクルード
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            // 頂点シェーダーに渡される構造体
            struct Attributes
            {
                float4 positionOS : POSITION; // モデル座標（Object Space）
                float3 normalOS   : NORMAL;   // 法線ベクトル（Object Space）
            };

            // 頂点→ピクセルに渡される構造体
            struct Varyings
            {
                float4 positionHCS : SV_POSITION; // 画面座標（Homogeneous Clip Space）
                float3 normalWS    : TEXCOORD0;   // 法線ベクトル（World Space）
            };

            // プロパティ（マテリアルで設定される値）
            float4 _Color;
            float4 _ShadeColor;
            float  _ShadeThreshold;

            // 頂点シェーダー：頂点ごとの情報を計算してピクセルシェーダーに渡す
            Varyings vert (Attributes IN)
            {
                Varyings OUT;

                // Object Space → Clip Space（画面座標）に変換
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);

                // 法線をWorld Spaceに変換
                OUT.normalWS = TransformObjectToWorldNormal(IN.normalOS);

                return OUT;
            }

            // ピクセルシェーダー：各ピクセルの色を決定
            half4 frag (Varyings IN) : SV_Target
            {
                // 法線と主光源の方向を正規化
                float3 normal = normalize(IN.normalWS);
                float3 lightDir = normalize(_MainLightPosition.xyz); // 主光源の方向（逆方向）

                // 法線と光の内積を取る → ランバート反射の基本
                float NdotL = dot(normal, lightDir);

                // しきい値で陰影を二分する（トゥーン調）
                if (NdotL > _ShadeThreshold)
                {
                    // 明るい側（ライトがよく当たる）
                    return _Color;
                }
                else
                {
                    // 影側（ライトが当たらない）→ 色を暗くする
                    return _Color * _ShadeColor;
                }
            }
            ENDHLSL
        }
    }
}
