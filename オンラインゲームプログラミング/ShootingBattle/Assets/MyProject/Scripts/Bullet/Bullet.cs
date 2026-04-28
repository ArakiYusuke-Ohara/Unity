using Fusion;
using UnityEngine;

public class Bullet : NetworkBehaviour
{
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
            gameObject.SetActive(false);
        }
        m_Life -= Runner.DeltaTime;
    }

    public void Fire(Vector3 pos, Quaternion rot)
    {
        transform.position = pos;
        transform.rotation = rot;
        gameObject.SetActive(true);
    }
}
