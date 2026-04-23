using Fusion;
using UnityEngine;
using UnityEngine.Rendering;
using static UnityEditor.PlayerSettings;

public class Player : NetworkBehaviour
{
    [SerializeField]
    float m_Speed = 5.0f;

    [SerializeField]
    float m_JumpPower = 10.0f;

    Vector3 m_Move = Vector3.zero;
    Rigidbody m_RigidBody = null;
    private void Start()
    {
        m_RigidBody = GetComponent<Rigidbody>();
    }

    public override void FixedUpdateNetwork()
    {
        // NW状態管理者（StateAuthority）がなければ何もしない
        if (!Object.HasStateAuthority) return;

        Vector3 pos = transform.position;

        // NW入力管理者（InputAuthority）から入力情報を受け取る
        // 入力情報はdataの中に格納される
        if (GetInput(out NetworkManager.PlayerInputData data))
        {
            // 移動ベクトル設定
            m_Move.x = data.horizontal * m_Speed * Runner.DeltaTime;
            m_Move.y = m_RigidBody.linearVelocity.y;
            m_Move.z = data.vertical * m_Speed * Runner.DeltaTime;

            // 移動ベクトル設定（こうしないと重力効かない）
            m_RigidBody.linearVelocity = m_Move;

            // ジャンプ
            if (data.jump)
            {
                // 移動ベクトル入れ直し
                m_Move.y = m_JumpPower;
                m_RigidBody.linearVelocity = m_Move;
            }

        }

    }
}
