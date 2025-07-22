Shader "Custom/04_HeightMap"
{
    Properties
    {
        // ���C���e�N�X�`��
        _MainTex ("Water Texture", 2D) = "white" {}

        // �n�C�g�}�b�v
        _HeightMap ("Height Map", 2D) = "gray" {}

        // UV�X�N���[�����x
        _ScrollSpeed ("Scroll Speed", Vector) = (0.05, 0.02, 0, 0)

        // �n�C�g�}�b�v�̋����i�K�p�x�j
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

                // �X�N���[��UV�v�Z
                float2 scrollUV = v.uv + _ScrollSpeed.xy * _Time.x;
                o.uv = scrollUV;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                // ���ʃe�N�X�`���擾
                fixed4 baseCol = tex2D(_MainTex, i.uv);

                // �n�C�g�}�b�v�i�����j�擾
                // ����͐Ԑ����݂̂��擾����
                // ���������قǐF�̒l���傫���̂ō����l���擾�ł���
                fixed height = tex2D(_HeightMap, i.uv).r;

                // �����ŐF���������邭�i�g�̉��ʕ\���j
                fixed4 col = baseCol + height * _HeightStrength;

                return col;
            }
            ENDCG
        }
    }
}
