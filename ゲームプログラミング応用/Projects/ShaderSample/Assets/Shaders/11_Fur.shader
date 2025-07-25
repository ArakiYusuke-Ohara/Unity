Shader "Custom/11_Fur"
{
    Properties
    {
        _MainTex ("Base Texture", 2D) = "white" {}
        _FurNoise ("Fur Noise", 2D) = "white" {}
        _FurLength ("Fur Length", Range(0.0, 0.5)) = 0.1
        _ShellCount ("Shell Count", Range(1, 30)) = 10
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Back
        ZWrite Off

        Pass
        {
            CGPROGRAM
            #pragma target 4.0
            #pragma vertex vert
            #pragma geometry geom
            #pragma fragment frag

            #include "UnityCG.cginc"

            sampler2D _MainTex;
            sampler2D _FurNoise;
            float _FurLength;
            float _ShellCount;

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
                float layer : TEXCOORD1;
            };

            // 頂点シェーダー
            appdata vert(appdata v)
            {
                return v;
            }

            [maxvertexcount(90)] // 最大出力頂点数（3頂点 × 30層）
            void geom(triangle appdata input[3], inout TriangleStream<v2f> triStream)
            {
                for (int i = 0; i < _ShellCount; ++i)
                {
                    float layerRatio = i / _ShellCount;
                    float offset = layerRatio * _FurLength;

                    v2f o[3];
                    for (int j = 0; j < 3; ++j)
                    {
                        float3 offsetPos = input[j].vertex.xyz + input[j].normal * offset;
                        o[j].pos = UnityObjectToClipPos(float4(offsetPos, 1.0));
                        o[j].uv = input[j].uv;
                        o[j].layer = layerRatio;
                    }

                    triStream.Append(o[0]);
                    triStream.Append(o[1]);
                    triStream.Append(o[2]);
                    triStream.RestartStrip();
                }
            }

            // フラグメントシェーダー
            fixed4 frag(v2f i) : SV_Target
            {
                float noise = tex2D(_FurNoise, i.uv).r;
                if (noise < i.layer)
                    discard;

                fixed4 col = tex2D(_MainTex, i.uv);
                col.a *= (1.0 - i.layer); // 毛先ほど透明に
                return col;
            }

            ENDCG
        }
    }
}
