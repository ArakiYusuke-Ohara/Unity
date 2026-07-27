using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    public enum EffectID
    {
        NONE = -1,
        HIT_BULLET,
    }

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

    public void PlayEffect(EffectID id, Vector3 pos)
    {
        // IDチェック
        if (id < 0 || (int)id >= m_EffectMasterObject.Length) return;

        // エフェクトリストを見て同じエフェクトかつ未使用のものがあれば再利用
        foreach (Effect effect in m_Effects)
        {
            if (effect.ID == id && !effect.gameObject.activeInHierarchy)
            {
                effect.Play(pos);
                return;
            }
        }

        // 再利用できなければ生成して再生
        GameObject obj = Instantiate(m_EffectMasterObject[(int)id]);
        Effect effectComponent = obj.GetComponent<Effect>();
        effectComponent.Play(pos);
        m_Effects.Add(effectComponent);
    }
}
