using Fusion;
using UnityEngine;

public class Bullet : NetworkBehaviour
{
    [Networked]
    public PlayerRef Owner { get; set; }

    [SerializeField]
    float m_Life = 2.0f;

    [SerializeField]
    float m_Speed = 5.0f;

    public override void FixedUpdateNetwork()
    {
        // 移動処理はサーバーのみ行う
        if (!Object.HasStateAuthority) return;

        // 前進するのみ
        transform.position += transform.forward * m_Speed * Runner.DeltaTime;

        // 寿命処理
        if (m_Life <= 0.0f)
        {
            // 削除
            Runner.Despawn(Object);
        }
        m_Life -= Runner.DeltaTime;
    }

    public void Fire(Vector3 pos, Quaternion rot)
    {
        transform.position = pos;
        transform.rotation = rot;
    }

    private void OnTriggerEnter(Collider other)
    {
        // 当たり判定はサーバーだけでやる
        if (!HasStateAuthority) return;

        // タグではなくコンポーネントでチェック
        Player otherPlayer = other.GetComponent<Player>();
        if (otherPlayer)
        {
            // 当たったのが撃ったプレイヤーでないか
            if (Owner != otherPlayer.Object.InputAuthority)
            {
                // 削除
                Runner.Despawn(Object);
            }
        }
    }
}
