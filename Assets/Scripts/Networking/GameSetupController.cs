using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using System.Linq;

public class GameSetupController : NetworkBehaviour
{
    public delegate void OnFinishedSpawn();
    public static OnFinishedSpawn onFinishedSpawn;
    public static GameSetupController Singleton { get; private set; }

    [SerializeField] Vector3 spawnPos;
    [HideInInspector] public GameObject player;
    public List<GameObject> characterList = new List<GameObject>();

    int playerCount;
    [HideInInspector] 
    public int playerSpawned = 0;

    [Header("Hider Setup")]
    public Material[] hiderMaterialList = new Material[4];
    public Material[] additionalMaterial;
    private void Awake()
    {
        Singleton = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        playerSpawned = 0;
        //NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
        //NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnected;
    }

    private void OnClientConnected(ulong id)
    {
        Debug.Log("Client Connected");
        id = OwnerClientId;
        float offset = Random.Range(0, 10f);

        //player = Instantiate(characterList[Random.Range(0, 1)], spawnPos + Vector3.right * offset, Quaternion.identity);
        //player.GetComponent<NetworkObject>().SpawnAsPlayerObject(id, true);
    }

    public override void OnNetworkSpawn()
    {
        //SpawnPlayerServerRpc();

        if (IsServer)
        {
            //NetworkManager.Singleton.OnClientDisconnectCallback += NetworkManager_OnClientDisconnectCallback;
            NetworkManager.Singleton.SceneManager.OnLoadEventCompleted += SceneManager_OnLoadEventCompleted;
        }
    }

    private void SceneManager_OnLoadEventCompleted(string sceneName, LoadSceneMode loadSceneMode, List<ulong> clientsCompleted, List<ulong> clientsTimedOut)
    {
        bool seekerSpawned = false;
        foreach (ulong clientId in NetworkManager.Singleton.ConnectedClientsIds)
        {
            playerCount = NetworkManager.Singleton.ConnectedClients.Count;
            Transform playerTransform;
            
            if(playerSpawned < playerCount)
            {
                if (!seekerSpawned)
                {
                    playerTransform = Instantiate(characterList[Random.Range(0, characterList.Count)].transform);
                    if (playerTransform.CompareTag("Player"))
                        seekerSpawned = true;
                }
                else
                    playerTransform = Instantiate(characterList[0].transform);
            }
            else if(playerSpawned >= playerCount && !seekerSpawned)
            {
                playerTransform = Instantiate(characterList[1].transform);
            }
            else
            {
                playerTransform = Instantiate(characterList[0].transform);
            }

            playerTransform.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId, true);
            if (playerTransform.CompareTag("Hider"))
            {
                GameManager.Instance.hiderList.Add(playerTransform);
            }
            else if (playerTransform.CompareTag("Player"))
            {
                GameManager.Instance.hunterList.Add(playerTransform);
            }
        }

        onFinishedSpawn?.Invoke();
    }

    [ContextMenu("SetMaterial")]
    public void SetMaterial()
    {
        for (int i = 0; i < GameManager.Instance.hiderList.Count; i++)
        {
            PlayerMoveNetwork targetPlayer = GameManager.Instance.hiderList[i].GetComponent<PlayerMoveNetwork>();
            Material[] mats = targetPlayer.meshRenderer.materials;

            for (int j = 0; j < mats.Length; j++)
            {
                mats[j] = hiderMaterialList[j];
            }

            targetPlayer.meshRenderer.materials = mats;
            targetPlayer.DisableView();
            if(targetPlayer.additionalMaterial != null)
            {
                Material[] addMats = targetPlayer.additionalMaterial.materials;
                addMats = additionalMaterial;
                targetPlayer.additionalMaterial.materials = addMats;
            }
        }
        
        Debug.Log("Material Changed");
    }

    public void CreatePlayer()
    {
        Debug.Log("Creating Player");
        float offset = Random.Range(0, 10f);

        if (!IsOwner) return;
        foreach (ulong id in NetworkManager.Singleton.ConnectedClientsIds)
        {
            player = Instantiate(characterList[Random.Range(0, 1)], spawnPos + Vector3.right * offset, Quaternion.identity);
            player.GetComponent<NetworkObject>().SpawnAsPlayerObject(id, true);
        }
    }

    [ContextMenu("Assign Camera")]
    public void AssignCamera()
    {
        gameObject.GetComponent<CameraFollow>().player = player.transform;
    }

    private void OnApplicationQuit()
    {
        //PhotonNetwork.Disconnect();
        //NetworkManager.Singleton.Shutdown();
    }

}
