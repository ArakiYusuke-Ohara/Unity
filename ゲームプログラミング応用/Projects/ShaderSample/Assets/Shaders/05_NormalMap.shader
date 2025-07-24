Shader "Custom/05_NormalMap"
{
    Properties
    {
        _MainTex ("Water Texture", 2D) = "white" {}            // ���ʂ̊�{�e�N�X�`��
        _HeightMap ("Height Map", 2D) = "gray" {}              // �������i�g�j�p�̃n�C�g�}�b�v
        _NormalMap ("Normal Map", 2D) = "bump" {}                // ���ʂ̉��ʊ����o���@���}�b�v
        _ScrollSpeed ("Scroll Speed", Vector) = (0.05, 0.02, 0, 0) // UV�X�N���[�����x (x, y)
        _HeightStrength ("Height Strength", Range(0,1)) = 0.1  // �n�C�g�}�b�v�ɂ�閾�邳�ω��̋���
        _NormalStrength ("Normal Strength", Range(0,2)) = 1.0  // �@���}�b�v�̋����i�ʉ����j
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

            // �e�N�X�`����p�����[�^
            sampler2D _MainTex;            // ���ʃe�N�X�`��
            sampler2D _HeightMap;          // �n�C�g�}�b�v
            sampler2D _NormalMap;          // �m�[�}���}�b�v
            float4 _ScrollSpeed;           // UV�X�N���[�����x�ix, y �����̂ݎg�p�j
            float _HeightStrength;         // �����ɂ��F�̉e��
            float _NormalStrength;         // �m�[�}���}�b�v�̉e���x

            // ���_�V�F�[�_�[����
            struct appdata
            {
                float4 vertex : POSITION;   // ���_�ʒu�i���[�J����ԁj
                float3 normal : NORMAL;     // ���_�@��
                float4 tangent : TANGENT;   // �ڐ��i�m�[�}���}�b�v�ϊ��Ɏg�p�j
                float2 uv : TEXCOORD0;      // UV���W
            };

            // ���_�V�F�[�_�[�o��
            struct v2f
            {
                float4 vertex : SV_POSITION;    // �N���b�v��Ԉʒu
                float2 uv : TEXCOORD0;          // ���ʃe�N�X�`���pUV
                float3 worldPos : TEXCOORD1;    // ���[���h��Ԉʒu
                float3 viewDir : TEXCOORD2;     // �J��������̎����x�N�g��
                float3 tangent : TEXCOORD3;     // �ڐ�
                float3 binormal : TEXCOORD4;    // �]�@��
                float3 normal : TEXCOORD5;      // �@��
            };

            // ���_�V�F�[�_�[
            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);  // �N���b�v��Ԃ֕ϊ�

                // UV���X�N���[�������ăA�j���[�V����
                float2 scrollUV = v.uv + _ScrollSpeed.xy * _Time.x;
                o.uv = scrollUV;

                // ���_�̃��[���h���W
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;

                // �J���������i�����x�N�g���j
                o.viewDir = normalize(_WorldSpaceCameraPos - o.worldPos);

                // TBN�s��i�m�[�}���}�b�v�̃x�N�g���ϊ��p�j
                o.normal = UnityObjectToWorldNormal(v.normal);
                o.tangent = UnityObjectToWorldDir(v.tangent.xyz);
                o.binormal = cross(o.normal, o.tangent) * v.tangent.w;

                return o;
            }

            // �t���O�����g�V�F�[�_�[
            fixed4 frag(v2f i) : SV_Target
            {
                float3x3 tbn = float3x3(i.tangent, i.binormal, i.normal);

                // ���ʃe�N�X�`���擾
                fixed4 baseCol = tex2D(_MainTex, i.uv);

                // �n�C�g�}�b�v���獂���擾
                fixed height = tex2D(_HeightMap, i.uv).r;

                // �m�[�}���}�b�v���T���v�����O����
                // RGB�̒l��XYZ�ɕϊ����Ė@����␳����x�N�g���Ƃ���
                // �������ARGB��0�`1�̒l�Ȃ̂Ńx�N�g���ɂ���ꍇ��-1�`1�͈̔͂ɕϊ�����K�v������
                fixed3 normalTex = tex2D(_NormalMap, i.uv).xyz * 2 - 1;
                normalTex.xy *= _NormalStrength;              // �@����XY������
                normalTex = normalize(normalTex);             // ���K��

                // TBN�s����쐬���ă��[���h��Ԃ֕ϊ�
                float3 normalWS = normalize(mul(normalTex, tbn));

                // �f�B���N�V���i�����C�g�Ή�
                // ���̂Ƃ��m�[�}���}�b�v����v�Z�����@���̕�ԃx�N�g��(normalTex)���K�p����
                // ����ȃ|���S���ł����ʂ̉e���ł���
                float3 lightDir = normalize(_WorldSpaceLightPos0.xyz);
                float NdotL = saturate(dot(normalWS, lightDir));
                fixed3 diffuse = baseCol.rgb * NdotL;

                // �n�C�g�}�b�v�␳���ďo��
                fixed4 resultColor;
                resultColor.rgb = diffuse + height * _HeightStrength;
                resultColor.a = baseCol.a;

                return resultColor;
            }
            ENDCG
        }
    }
}
