using Fusion;
using UnityEngine;

public class Player : NetworkBehaviour
{
    [SerializeField]
    float m_Speed = 5.0f;

    [SerializeField]
    float m_JumpPower = 5.0f;

    [SerializeField]
    float m_Gravity = -9.8f;

    Vector3 m_Move = Vector3.zero;
    CharacterController m_Controller = null;

    private void Awake()
    {
        // オンラインの移動はCharacterControllerが一番無難
        m_Controller = GetComponent<CharacterController>();
    }

    public override void FixedUpdateNetwork()
    {
        // 移動処理はサーバーのみ行う
        // Authorityはサーバーのみ持つのでそれで判定する
        if (!Object.HasStateAuthority) return;

        // 設置判定
        if (m_Controller.isGrounded && m_Move.y < 0.0f)
        {
            // 地面にいるときは軽く地面に押し付ける
            m_Move.y = -1.0f;
        }

        // NetworkManagerのOnInputで設定された入力データを受け取る
        // 入力情報はdataの中に格納される
        if (GetInput(out NetworkManager.PlayerInputData data))
        {
            // 入力から水平移動量を設定
            m_Move.x = data.horizontal * m_Speed;
            m_Move.z = data.vertical * m_Speed;

            // ジャンプ
            if (data.jump && m_Controller.isGrounded)
            {
                m_Move.y = m_JumpPower;
            }

            // 重力
            m_Move.y += m_Gravity * Runner.DeltaTime;

            // 移動
            m_Controller.Move(m_Move * Runner.DeltaTime);
        }
    }
}
