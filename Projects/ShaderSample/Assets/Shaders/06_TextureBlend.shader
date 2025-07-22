Shader "Custom/06_TextureBlend"
{
    Properties
    {
        _TexA("Texture A", 2D) = "white" {}              // �u�����h���̃e�N�X�`��
        _TexB("Texture B", 2D) = "black" {}              // �u�����h��̃e�N�X�`��
        _NoiseTex("Noise Texture", 2D) = "gray" {}       // �u�����h���Ɏg���m�C�Y�e�N�X�`���i�O���[�X�P�[���j
        _BlendStrength("Blend Strength", Range(0,1)) = 1.0 // �u�����h���x�i0=���STexA�A1=�m�C�Y�ɏ]���ău�����h�j
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" } // �s�����Ƃ��Ĉ���
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            sampler2D _TexA;         // �e�N�X�`��A
            sampler2D _TexB;         // �e�N�X�`��B
            sampler2D _NoiseTex;     // �m�C�Y�e�N�X�`��
            float _BlendStrength;    // �u�����h�̋���

            struct appdata
            {
                float4 vertex : POSITION;  // ���_���W
                float2 uv : TEXCOORD0;     // UV���W
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;      // �t���O�����g�pUV���W
                float4 vertex : SV_POSITION; // �N���b�v��ԍ��W
            };

            // ���_�V�F�[�_�[
            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex); // ��ʕ`��p�ϊ�
                o.uv = v.uv;                               // UV�����̂܂ܓn��
                return o;
            }

            // �t���O�����g�V�F�[�_�[
            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 colA = tex2D(_TexA, i.uv);               // �e�N�X�`��A�̐F�擾
                fixed4 colB = tex2D(_TexB, i.uv);               // �e�N�X�`��B�̐F�擾
                fixed noise = tex2D(_NoiseTex, i.uv).r;         // �m�C�Y�̖��邳�iR�`�����l���j

                fixed blendFactor = saturate(noise * _BlendStrength); // �u�����h�W����0�`1�ɃN�����v
                return lerp(colA, colB, blendFactor);                 // �m�C�Y�Ɋ�Â����`��Ԃō���
            }
            ENDCG
        }
    }
}
