using UnityEngine;

public class Dissolver : MonoBehaviour
{
    [SerializeField]
    private Material m_Material;

    private float m_Dissolve = 0.0f;

    public float Dissolve { get { return m_Dissolve; } set { m_Dissolve = value; } }

    void Update()
    {
        m_Material.SetFloat("_DissolveAmount", m_Dissolve);
    }
}
