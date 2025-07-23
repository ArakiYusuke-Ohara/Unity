Shader "Custom/10_Outline"
{
    Properties
    {
        _Color ("Main Color", Color) = (1,1,1,1)                      // モデルのベースカラー
        _RampTex ("Ramp Texture", 2D) = "white" {}                    // トゥーン調の陰影テクスチャ
        _OutlineColor ("Outline Color", Color) = (0,0,0,1)            // アウトラインの色
        _OutlineWidth ("Outline Width", Range(0.0, 0.05)) = 0.02      // アウトラインの太さ
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        // ==============================
        // パス①：アウトライン描画用パス
        // ==============================
        Pass
        {
            Cull Front            // 表面をカリング → 裏側（外向き法線にスケールした面）だけ描画

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            fixed4 _OutlineColor;
            float _OutlineWidth;

            struct appdata
            {
                float4 vertex : POSITION;  // 頂点位置（ローカル空間）
                float3 normal : NORMAL;    // 法線ベクトル
            };

            struct v2f
            {
                float4 pos : SV_POSITION;  // クリップ空間に変換された頂点位置
            };

            v2f vert (appdata v)
            {
                v2f o;
                // 法線方向にアウトライン分だけ拡張
                float3 norm = normalize(v.normal);
                v.vertex.xyz += norm * _OutlineWidth;
   
                o.pos = UnityObjectToClipPos(v.vertex); // MVP変換
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                return _OutlineColor; // アウトライン色で塗る
            }
            ENDCG
        }

        // ==============================
        // パス②：Rampによるトゥーン描画
        // ==============================
        Pass
        {
            Tags { "LightMode" = "UniversalForward" }
            Cull Back        // 裏面をカリング（通常描画）
            CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            #include "Lighting.cginc"

            sampler2D _RampTex;     // Rampテクスチャ（1Dトーンマッピング）
            fixed4 _Color;          // モデルカラー（乗算用）

            struct appdata
            {
                float4 vertex : POSITION;  // 頂点座標
                float3 normal : NORMAL;    // 法線ベクトル
            };

            struct v2f
            {
                float4 pos : SV_POSITION;     // 最終的な描画座標（クリップ空間）
                float3 normalWS : TEXCOORD0;  // ワールド空間での法線
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);                        // モデル → MVP変換
                o.normalWS = UnityObjectToWorldNormal(v.normal);              // 法線をワールド空間に変換
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // 主光源の方向ベクトルを取得（_WorldSpaceLightPos0.xyz はワールド空間の光源）
                float3 normal = normalize(i.normalWS);
                float3 lightDir = normalize(_WorldSpaceLightPos0.xyz);

                // 法線と光の内積で明るさを取得（0〜1に制限）
                float NdotL = dot(normal, lightDir);
                float rampU = saturate(NdotL * 0.5 + 0.5); // -1~1 → 0~1

                // Rampテクスチャを参照（横1行なのでYは0.5固定）
                float3 rampColor = tex2D(_RampTex, float2(rampU, 0.5)).rgb;

                // Rampの色とモデルの色を掛け合わせて最終色に
                return float4(_Color.rgb * rampColor, 1.0);
            }

            ENDCG
        }
    }
}
