using Fusion;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class Player : NetworkBehaviour
{
    [SerializeField]
    float m_YawSpeed = 360.0f;

    [SerializeField]
    float m_MaxSpeed = 10.0f;

    [SerializeField]
    float m_Accel = 5.0f;

    [SerializeField]
    float m_Decel = -20.0f;

    float m_Speed = 0.0f;
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
            // 入力からヨー回転
            transform.Rotate(0.0f, data.horizontal * m_YawSpeed * Runner.DeltaTime, 0.0f);

            // アクセル入力
            if (data.accel)
            {
                m_Speed += m_Accel * Runner.DeltaTime;
            }
            else 
            {
                m_Speed += m_Decel * Runner.DeltaTime;
            }

            // 速度制限
            m_Speed = Mathf.Clamp(m_Speed, 0.0f, m_MaxSpeed);
            // 移動
            m_Controller.Move(transform.forward * m_Speed * Runner.DeltaTime);

            // 弾丸発射
            if (data.fire)
            {
                // 発射
                Bullet bullet = BulletManager.Instance.FireBullet(Runner, transform.position, transform.rotation);
                // 誰が撃ったかはInputAuthorityで覚えておくと確実
                bullet.Owner = Object.InputAuthority;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // 当たり判定はサーバーだけでやる
        if (!HasStateAuthority) return;

        // タグではなくコンポーネントチェック
        Bullet bullet = other.GetComponent<Bullet>();
        if (bullet)
        {
            // 相手が撃った弾かどうか
            if (bullet.Owner != Object.InputAuthority)
            {
                // 死亡
                Runner.Despawn(Object);
            }
        }
    }
}
