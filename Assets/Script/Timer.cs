using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Netcode;

public class Timer : NetworkBehaviour
{
    public static Timer Instance;

    public int durationInSeconds;
    [SerializeField] int remainingTime;

    [SerializeField] TMP_Text timerText;
    [SerializeField] GameObject hiderWinUI;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        hiderWinUI.SetActive(false);
    }

    public override void OnNetworkSpawn()
    {
        
    }

    public void StartTimer()
    {
        if (IsHost)
        {
            Debug.Log("Start Timer Check Player");
            if (GameSetupController.Singleton.playerSpawned == KitchenGameLobby.Instance.playerCount)
            {
                Debug.Log("Timer Started");
                Debug.Log(NetworkManager.Singleton.ConnectedClients.Count);
                StartTimerServerRpc();
            }
        }
    }

    [ServerRpc]
    public void StartTimerServerRpc(ServerRpcParams serverRpcParams = default)
    {
        Debug.Log("Start Timer Server Rpc");
        StartTimerClientRpc();
    }

    [ClientRpc]
    public void StartTimerClientRpc()
    {
        Debug.Log("Start Timer Client Rpc");
        durationInSeconds = remainingTime;
        StartCoroutine(UpdateTimer());
    }

    IEnumerator UpdateTimer()
    {
        while(durationInSeconds >= 0)
        {
            timerText.text = $"{durationInSeconds / 60:D2} : {durationInSeconds % 60:D2}";
            durationInSeconds--;
            if (durationInSeconds <= 0)
            {
                durationInSeconds = 0;
                Time.timeScale = 0;
                hiderWinUI.SetActive(true);
            }
            yield return new WaitForSeconds(1);
        }
        EndTimer();
    }

    void EndTimer()
    {
        Debug.Log("Time's Up");
    }
}
