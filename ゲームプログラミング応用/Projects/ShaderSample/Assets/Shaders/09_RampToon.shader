Shader "Custom/09_RampToon"
{
    Properties
    {
        // �e�N�X�`���Ȃ��ł��g����P�F�J���[
        _Color ("Main Color", Color) = (1,1,1,1)

        // Ramp�e�N�X�`���i�e�̃g�[�������߂�1D�摜�j
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

            // ���_�V�F�[�_�[���́F���f�����W�Ɩ@��
            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS   : NORMAL;
            };

            // ���_���s�N�Z���ɓn�����
            struct Varyings
            {
                float4 positionHCS : SV_POSITION;  // ��ʍ��W
                float3 normalWS    : TEXCOORD0;    // �@���iWorld Space�j
            };

            // �v���p�e�B�ϐ��iMaterial Inspector����ݒ�\�j
            float4 _Color;

            sampler2D _RampTex;

            // ���_�V�F�[�_�[
            Varyings vert (Attributes IN)
            {
                Varyings OUT;

                // ���f�����W �� ��ʍ��W�ɕϊ�
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);

                // �@����World Space�ɕϊ�
                OUT.normalWS = TransformObjectToWorldNormal(IN.normalOS);

                return OUT;
            }

            // �s�N�Z���V�F�[�_�[�i1�s�N�Z�����Ƃ̐F�v�Z�j
            half4 frag (Varyings IN) : SV_Target
            {
                // �@���ƌ��̕����̓��ρi�ǂꂾ�������������Ă��邩�j
                float3 normal = normalize(IN.normalWS);
                float3 lightDir = normalize(_MainLightPosition.xyz); // ������̕���
                float NdotL = dot(normal, lightDir); // ���l��0�ɃN���b�v
                float rampU = saturate(NdotL * 0.5 + 0.5); // -1~1 �� 0~1
                
                // Ramp�e�N�X�`���ŉA�e�̐F������
                // ��1�s�̃e�N�X�`���Ȃ̂ŁAY=0.5�ŌŒ肵�ēǂݎ��
                float3 rampColor = tex2D(_RampTex, float2(rampU, 0.5)).rgb;

                // �x�[�X�J���[�ƍ������čŏI�F��
                return float4(_Color.rgb * rampColor, 1.0);
            }
            ENDHLSL
        }
    }
}
