Shader "Custom/08_SimpleToon"
{
    Properties
    {
        // �I�u�W�F�N�g�̊�{�F
        _Color ("Main Color", Color) = (1,1,1,1)

        // �e�ɂȂ邩�ǂ��������߂���̓������̂������l
        _ShadeThreshold ("Shade Threshold", Range(0,1)) = 0.5

        // �e���̐F�̊����i_Color�ɏ�Z�����j
        _ShadeColor ("Shade Color", Color) = (0.5, 0.5, 0.5, 1)
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        Pass
        {
            HLSLPROGRAM
            // ���_/�s�N�Z���V�F�[�_�[�w��
            #pragma vertex vert
            #pragma fragment frag

            // URP�̋��ʃ��C�u�������C���N���[�h
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            // ���_�V�F�[�_�[�ɓn�����\����
            struct Attributes
            {
                float4 positionOS : POSITION; // ���f�����W�iObject Space�j
                float3 normalOS   : NORMAL;   // �@���x�N�g���iObject Space�j
            };

            // ���_���s�N�Z���ɓn�����\����
            struct Varyings
            {
                float4 positionHCS : SV_POSITION; // ��ʍ��W�iHomogeneous Clip Space�j
                float3 normalWS    : TEXCOORD0;   // �@���x�N�g���iWorld Space�j
            };

            // �v���p�e�B�i�}�e���A���Őݒ肳���l�j
            float4 _Color;
            float4 _ShadeColor;
            float  _ShadeThreshold;

            // ���_�V�F�[�_�[�F���_���Ƃ̏����v�Z���ăs�N�Z���V�F�[�_�[�ɓn��
            Varyings vert (Attributes IN)
            {
                Varyings OUT;

                // Object Space �� Clip Space�i��ʍ��W�j�ɕϊ�
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);

                // �@����World Space�ɕϊ�
                OUT.normalWS = TransformObjectToWorldNormal(IN.normalOS);

                return OUT;
            }

            // �s�N�Z���V�F�[�_�[�F�e�s�N�Z���̐F������
            half4 frag (Varyings IN) : SV_Target
            {
                // �@���Ǝ�����̕����𐳋K��
                float3 normal = normalize(IN.normalWS);
                float3 lightDir = normalize(_MainLightPosition.xyz); // ������̕����i�t�����j

                // �@���ƌ��̓��ς���� �� �����o�[�g���˂̊�{
                float NdotL = dot(normal, lightDir);

                // �������l�ŉA�e��񕪂���i�g�D�[�����j
                if (NdotL > _ShadeThreshold)
                {
                    // ���邢���i���C�g���悭������j
                    return _Color;
                }
                else
                {
                    // �e���i���C�g��������Ȃ��j�� �F���Â�����
                    return _Color * _ShadeColor;
                }
            }
            ENDHLSL
        }
    }
}
