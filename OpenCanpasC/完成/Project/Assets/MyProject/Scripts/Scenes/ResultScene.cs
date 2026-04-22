using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ResultScene : MonoBehaviour
{
    [SerializeField]
    Text m_KillEnemyText = null;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (m_KillEnemyText)
        {
            m_KillEnemyText.text = "ì|ÇµÇΩìGÇÃêîÅF" + PlayScene.Instance.KillEnemy + "ëÃ";
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            SceneManager.LoadScene("Play");
        }
    }
}
