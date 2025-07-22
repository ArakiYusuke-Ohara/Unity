Shader "Custom/03_UVScroll"
{
    Properties
    {
        // �e�N�X�`���̓C���X�y�N�^�[�Őݒ�
        _MainTex ("Texture", 2D) = "white" {}

        // �X�N���[�������Ƒ��x�̓C���X�y�N�^�[�Őݒ�
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

                // ���Ԍo�߂�UV���W�����炵�Ă���
                // _Time��Unity���ŗp�ӂ���Ă���o�ߎ��Ԃ��i�[����Ă���ϐ�
                o.uv = v.uv + _ScrollSpeed.xy * _Time.x;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // UV���W�����Ƀe�N�X�`���̂ǂ̕����̐F���g�������߂ĕԋp
                return tex2D(_MainTex, i.uv);
            }
            ENDCG
        }
    }
}
