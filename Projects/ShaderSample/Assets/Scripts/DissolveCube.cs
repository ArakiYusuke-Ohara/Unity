using UnityEngine;

public class DissolveCube : MonoBehaviour
{
    [SerializeField]
    private float m_DissolveSpeed = 0.5f;

    private Dissolver m_Dissolver = null;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_Dissolver = GetComponent<Dissolver>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!m_Dissolver) return;

        if (Input.GetKey(KeyCode.Z))
        {
            m_Dissolver.Dissolve += m_DissolveSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.X))
        {
            m_Dissolver.Dissolve -= m_DissolveSpeed * Time.deltaTime;
        }
    }
}
