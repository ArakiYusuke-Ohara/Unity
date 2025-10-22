Shader "Custom/05_NormalMap"
{
    Properties
    {
        _MainTex ("Water Texture", 2D) = "white" {}            // 水面の基本テクスチャ
        _HeightMap ("Height Map", 2D) = "gray" {}              // 高さ情報（波）用のハイトマップ
        _NormalMap ("Normal Map", 2D) = "bump" {}                // 水面の凹凸感を出す法線マップ
        _ScrollSpeed ("Scroll Speed", Vector) = (0.05, 0.02, 0, 0) // UVスクロール速度 (x, y)
        _HeightStrength ("Height Strength", Range(0,1)) = 0.1  // ハイトマップによる明るさ変化の強さ
        _NormalStrength ("Normal Strength", Range(0,2)) = 1.0  // 法線マップの強さ（凸凹感）
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

            // テクスチャやパラメータ
            sampler2D _MainTex;            // 水面テクスチャ
            sampler2D _HeightMap;          // ハイトマップ
            sampler2D _NormalMap;          // ノーマルマップ
            float4 _ScrollSpeed;           // UVスクロール速度（x, y 成分のみ使用）
            float _HeightStrength;         // 高さによる色の影響
            float _NormalStrength;         // ノーマルマップの影響度

            // 頂点シェーダー入力
            struct appdata
            {
                float4 vertex : POSITION;   // 頂点位置（ローカル空間）
                float3 normal : NORMAL;     // 頂点法線
                float4 tangent : TANGENT;   // 接線（ノーマルマップ変換に使用）
                float2 uv : TEXCOORD0;      // UV座標
            };

            // 頂点シェーダー出力
            struct v2f
            {
                float4 vertex : SV_POSITION;    // クリップ空間位置
                float2 uv : TEXCOORD0;          // 水面テクスチャ用UV
                float3 worldPos : TEXCOORD1;    // ワールド空間位置
                float3 viewDir : TEXCOORD2;     // カメラからの視線ベクトル
                float3 tangent : TEXCOORD3;     // 接線
                float3 binormal : TEXCOORD4;    // 従法線
                float3 normal : TEXCOORD5;      // 法線
            };

            // 頂点シェーダー
            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);  // クリップ空間へ変換

                // UVをスクロールさせてアニメーション
                float2 scrollUV = v.uv + _ScrollSpeed.xy * _Time.x;
                o.uv = scrollUV;

                // 頂点のワールド座標
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;

                // カメラ方向（視線ベクトル）
                o.viewDir = normalize(_WorldSpaceCameraPos - o.worldPos);

                // TBN行列（ノーマルマップのベクトル変換用）
                o.normal = UnityObjectToWorldNormal(v.normal);
                o.tangent = UnityObjectToWorldDir(v.tangent.xyz);
                o.binormal = cross(o.normal, o.tangent) * v.tangent.w;

                return o;
            }

            // フラグメントシェーダー
            fixed4 frag(v2f i) : SV_Target
            {
                float3x3 tbn = float3x3(i.tangent, i.binormal, i.normal);

                // 水面テクスチャ取得
                fixed4 baseCol = tex2D(_MainTex, i.uv);

                // ハイトマップから高さ取得
                fixed height = tex2D(_HeightMap, i.uv).r;

                // ノーマルマップをサンプリングする
                // RGBの値をXYZに変換して法線を補正するベクトルとする
                // ただし、RGBは0～1の値なのでベクトルにする場合は-1～1の範囲に変換する必要がある
                fixed3 normalTex = tex2D(_NormalMap, i.uv).xyz * 2 - 1;
                normalTex.xy *= _NormalStrength;              // 法線のXYを強調
                normalTex = normalize(normalTex);             // 正規化

                // TBN行列を作成してワールド空間へ変換
                float3 normalWS = normalize(mul(normalTex, tbn));

                // ディレクショナルライト対応
                // このときノーマルマップから計算した法線の補間ベクトル(normalTex)が適用され
                // 平らなポリゴンでも凹凸の影ができる
                float3 lightDir = normalize(_WorldSpaceLightPos0.xyz);
                float NdotL = saturate(dot(normalWS, lightDir));
                fixed3 diffuse = baseCol.rgb * NdotL;

                // ハイトマップ補正して出力
                fixed4 resultColor;
                resultColor.rgb = diffuse + height * _HeightStrength;
                resultColor.a = baseCol.a;

                return resultColor;
            }
            ENDCG
        }
    }
}
