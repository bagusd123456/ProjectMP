using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using TMPro;
using Unity.Networking.Transport;
using UnityEngine.UI;

public class UIButtonManager : MonoBehaviour
{
    public NetworkManager networkManager;
    public GameObject[] playerPrefab;
    GameObject instancePlayerPrefab;
    NetworkObject m_playerPrefab;
    public Button btnHost;
    public Button btnServer;
    public Button btnClient;

    [SerializeField] int index = 1;
    private void Awake()
    {
        btnHost.onClick.AddListener(() => StartHost());
        btnServer.onClick.AddListener(() => networkManager.StartServer());
        btnClient.onClick.AddListener(() => StartClient());
    }

    public void StartHost()
    {
        RandomChar();
        networkManager.StartHost();
    }

    public void StartClient()
    {
        RandomChar();
        networkManager.StartClient();
    }

    public void RandomChar()
    {
        if (index != 0)
            index = Random.Range(0, playerPrefab.Length);

        networkManager.NetworkConfig.PlayerPrefab = playerPrefab[index];
    }
    
    public void Destroy(NetworkObject networkObject)
    {
        Destroy(instancePlayerPrefab);
    }
}
