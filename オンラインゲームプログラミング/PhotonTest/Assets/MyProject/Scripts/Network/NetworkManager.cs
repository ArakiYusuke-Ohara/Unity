using Fusion;
using Fusion.Sockets;
using System;
using System.Collections.Generic;
using UnityEngine;

public class NetworkManager : MonoBehaviour, INetworkRunnerCallbacks
{
    public struct PlayerInputData : INetworkInput
    {
        public float horizontal;
        public float vertical;
        public bool jump;
    }

    private NetworkRunner m_Runner;

    [SerializeField]
    NetworkPrefabRef m_PlayerPrefab;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    async void Start()
    {
        m_Runner = gameObject.AddComponent<NetworkRunner>();
        m_Runner.ProvideInput = true;

        m_Runner.AddCallbacks(this);

        await m_Runner.StartGame(new StartGameArgs()
        {
            GameMode = GameMode.AutoHostOrClient,
            SessionName = "FusionLesson",
            SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
        });
    }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player) 
    {
        if (runner.IsServer)
        {
            Vector3 spawnPos = new Vector3(0.0f, 3.0f, 0.0f);
            NetworkObject obj = runner.Spawn(m_PlayerPrefab, spawnPos, Quaternion.identity, player);
            runner.SetPlayerObject(player, obj);
        }
    }
    public void OnInput(NetworkRunner runner, NetworkInput input) 
    {
        PlayerInputData data = new PlayerInputData();
        data.horizontal = Input.GetAxis("Horizontal");
        data.vertical = Input.GetAxis("Vertical");
        data.jump = Input.GetKeyDown(KeyCode.Z);

        input.Set(data);
    }


    // Å´é¿ëïÇµÇ»Ç¢Ç∆Ç¢ÇØÇ»Ç¢èÉêàâºëzä÷êîÇΩÇø
    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
        
    }

    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
        
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        
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
