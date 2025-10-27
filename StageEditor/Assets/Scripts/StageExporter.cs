using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System;
using System.Linq;

// ファイル出力するデータ
[Serializable]
public class StageObjectData
{
    public int id;              // ID
    public Vector3 position;    // 座標
    public Vector3 rotation;    // 回転
    public Vector3 scale;       // スケール
    public string name;         // ヒエラルキー名（確認用）
}

// jsonファイルにするデータのラッパー
[Serializable]
public class Wrapper
{
    public StageObjectData[] items;
}

public class StageExporter
{
    // Unityの上部メニューに追加する
    // クリックすると下の関数が呼ばれる
    [MenuItem("Tools/Export Stage JSON")]
    static void ExportStage()
    {
        // 開いているシーンを取得
        Scene scene = SceneManager.GetActiveScene();
        // 登録されたオブジェクトたちを取得
        List<StageObject> objects = StageObject.m_AllObject;

        // 登録されたオブジェクトたちを出力用のデータに格納
        StageObjectData[] exportList = objects
            .Select(obj => new StageObjectData
            {
                id = (int)obj.ID,
                position = obj.transform.position,
                rotation = obj.transform.localEulerAngles,
                scale = obj.transform.localScale,
                name = obj.name,
            })
            .ToArray();

        // 出力するデータをラップする
        var wrapper = new Wrapper { items = exportList };

        // ラップしたデータをjsonで出力する
        string json = JsonUtility.ToJson(wrapper, true);
        File.WriteAllText(Application.dataPath + "/../json/" + scene.name + ".json", json);

        // 完了メッセージ
        Debug.Log("Exported 3D stage.json");
    }
}
