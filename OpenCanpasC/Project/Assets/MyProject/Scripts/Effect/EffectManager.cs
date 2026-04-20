using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    // ID順に設定するエフェクトの元データ
    [SerializeField]
    private GameObject[] m_EffectMasterObject;

    // プレイされたエフェクトリスト
    List<Effect> m_Effects = new List<Effect>();

    public static EffectManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void PlayEffect(int id, Vector3 pos)
    {
        // IDチェック
        if (id < 0 || id >= m_EffectMasterObject.Length) return;

        // 発射リストを見て同じ弾丸かつ未使用のものがあれば再利用
        foreach (Effect effect in m_Effects)
        {
            if (effect.ID == id && !effect.gameObject.activeInHierarchy)
            {
                effect.Play(pos);
                return;
            }
        }

        // 再利用できなければ生成して再生
        GameObject obj = Instantiate(m_EffectMasterObject[id]);
        Effect effectComponent = obj.GetComponent<Effect>();
        effectComponent.Play(pos);
        m_Effects.Add(effectComponent);
    }
}
