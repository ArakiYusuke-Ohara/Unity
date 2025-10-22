using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

[ExecuteAlways]
public class StageObject : MonoBehaviour
{
    public enum StageObjectID
    {
        // Floor
        FLOOR_00,
        // Player
        PLAYER,
        // Block
        BLOCK_00,
        // Enemy
        ENEMY_00,
        ENEMY_01,

    }

    [SerializeField]
    private StageObjectID m_ID;
    public StageObjectID ID {get { return m_ID; }}

    public static List<StageObject> m_AllObject = new List<StageObject>();

    // オブジェクトを配置したらリストに追加
    private void Awake()
    {
        if(!m_AllObject.Contains(this)) m_AllObject.Add(this);
    }

    private void OnEnable()
    {
        if (!m_AllObject.Contains(this)) m_AllObject.Add(this);
    }

    // シーンから削除したらリストからも削除
    private void OnDisable()
    {
        m_AllObject.Remove(this);
    }
}
