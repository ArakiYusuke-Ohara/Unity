using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System;
using System.Linq;

[Serializable]
public class StageObjectData
{
    public int id;
    public Vector3 position;
    public Vector3 rotation;
    public Vector3 scale;
    public string name;
}

// 配列ラッパー
[Serializable]
public class Wrapper
{
    public StageObjectData[] items;
}

public class StageExporter
{
    [MenuItem("Tools/Export Stage JSON")]
    static void ExportStage()
    {
        var scene = SceneManager.GetActiveScene();
        var objects = StageObject.m_AllObject;

        // StageObject の情報を StageObjectData に変換
        var exportList = StageObject.m_AllObject
            .Select(obj => new StageObjectData
            {
                id = (int)obj.ID,
                position = obj.transform.position,
                rotation = obj.transform.localEulerAngles,
                scale = obj.transform.localScale,
                name = obj.name,
            })
            .ToArray();

        var wrapper = new Wrapper { items = exportList };
        string json = JsonUtility.ToJson(wrapper, true);

        File.WriteAllText(Application.dataPath + "/../json/" + scene.name + ".json", json);

        Debug.Log("Exported 3D stage.json");
    }
}