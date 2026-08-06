using Fusion;
using Fusion.Sockets;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Unity.Collections.Unicode;

public class NetworkManager : MonoBehaviour, INetworkRunnerCallbacks
{
    // 入力データ
    public struct PlayerInputData : INetworkInput
    {
        public float horizontal;
        public bool accel;
        public bool fire;
    }

    // プレイヤーが操作するプレハブ
    [SerializeField]
    NetworkPrefabRef m_PlayerPrefab;

    // ネットワークシステムの根幹であるランナー
    private NetworkRunner m_Runner;

    // ジャンプを押したかフラグ
    bool m_IsFire = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    async void Start()
    {
        // ランナーの設定
        m_Runner = gameObject.AddComponent<NetworkRunner>();
        m_Runner.ProvideInput = true;
        m_Runner.AddCallbacks(this);
        BulletManager.Instance.Init(m_Runner);

        // サーバーかクライアントか
        GameMode gameMode = GameMode.Server;
        // 実行時のコマンドライン引数でサーバーかどうか判定する
        string[] args = System.Environment.GetCommandLineArgs();
        if (args.Contains("-client"))
        {
            gameMode = GameMode.Client;
        }

        // ゲームを設定して通信開始
        await m_Runner.StartGame(new StartGameArgs()
        {
            GameMode = gameMode,    // サーバーかクライアントか
            SessionName = "FusionLesson",   // セッション名（同じ名前の人たちでつながる）
            SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()    // ネットワーク用のシーン管理
        });
    }

    private void Update()
    {
        // トリガーな入力は通信ラグで検知できない場合がある
        // いつものUpdateでフラグとして入力があったことを覚えておく
        if (Input.GetKeyDown(KeyCode.X))
        {
            m_IsFire = true;
        }
    }

    // ↓↓↓実装しないといけない純粋仮想関数たち↓↓↓

    /// <summary>
    /// プレイヤーが参加してきたら呼ばれる
    /// </summary>
    /// <param name="runner">ランナー</param>
    /// <param name="player">参加してきたプレイヤー参照</param>
    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player) 
    {
        // サーバーがプレイヤーをスポーンさせる
        if (runner.IsServer)
        {
            Vector3 spawnPos = new Vector3(0.0f, 3.0f, 0.0f);
            NetworkObject obj = runner.Spawn(m_PlayerPrefab, spawnPos, Quaternion.identity, player);
            runner.SetPlayerObject(player, obj);
        }
    }

    /// <summary>
    /// プレイヤーから入力があるたびに呼ばれる
    /// </summary>
    /// <param name="runner">ランナー</param>
    /// <param name="input">入力システム</param>
    public void OnInput(NetworkRunner runner, NetworkInput input) 
    {
        // 入力データに入力状況を記録
        PlayerInputData data = new PlayerInputData();
        data.horizontal = Input.GetAxis("Horizontal");
        data.accel = Input.GetKey(KeyCode.Z);

        // トリガーなものはUpdateで記憶した情報を設定
        data.fire = m_IsFire;

        // 入力システムにデータを設定
        input.Set(data);

        // トリガーを設定出来たらフラグを折る
        m_IsFire = false;

    }


    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
        
    }

    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
        
    }

    /// <summary>
    /// プレイヤーが通信からいなくなったら呼ばれる
    /// </summary>
    /// <param name="runner">ランナー</param>
    /// <param name="player">いなくなったプレイヤー参照</param>
    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        // オブジェクトを取得して退場
        if (runner.TryGetPlayerObject(player, out NetworkObject obj))
        {
            runner.Despawn(obj);
        }
    }

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {
        
    }

    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason)
    {
        
    }

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
    {
        
    }

    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    {
        
    }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
    {
        
    }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data)
    {
        
    }

    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress)
    {
        
    }

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
    {
        
    }

    public void OnConnectedToServer(NetworkRunner runner)
    {
        
    }

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {
        
    }

    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
    {
        
    }

    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
    {
        
    }

    public void OnSceneLoadDone(NetworkRunner runner)
    {
        
    }

    public void OnSceneLoadStart(NetworkRunner runner)
    {
        
    }
}
