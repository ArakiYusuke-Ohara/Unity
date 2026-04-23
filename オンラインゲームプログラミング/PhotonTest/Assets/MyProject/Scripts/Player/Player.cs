using Fusion;
using UnityEngine;

public class Player : NetworkBehaviour
{
    [SerializeField]
    float m_Speed = 5.0f;

    public override void FixedUpdateNetwork()
    {

        if (!Object.HasStateAuthority) return;

        if (GetInput(out NetworkManager.PlayerInputData data))
        {
            transform.position += Vector3.right * data.horizontal * m_Speed * Runner.DeltaTime;
        }
    }
}
