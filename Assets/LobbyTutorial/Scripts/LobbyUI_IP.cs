using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using Unity.Services.Authentication;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyUI_IP : MonoBehaviour {

    public static LobbyUI_IP Instance { get; private set; }

    [SerializeField] private Transform playerSingleTemplate;
    [SerializeField] private Transform container;
    [SerializeField] private TextMeshProUGUI lobbyNameText;
    [SerializeField] private TextMeshProUGUI playerCountText;
    [SerializeField] private Button leaveLobbyButton;
    [SerializeField] private Button startGameButton;

    public Transform player;


    private void Awake() {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;

        playerSingleTemplate.gameObject.SetActive(false);

        leaveLobbyButton.onClick.AddListener(() => {
            KitchenGameLobby.Instance.LeaveLobby();
        });

        /*startGameButton.onClick.AddListener(() =>
        {
            StartGame();
        });*/
    }

    private void Start() {
        KitchenGameLobby.Instance.OnCreateLobbyStarted += UpdateLobby_Event;
        KitchenGameLobby.Instance.OnJoinedLobby += UpdateLobby_Event;
        KitchenGameLobby.Instance.OnJoinedLobbyUpdate += UpdateLobby_Event;
        KitchenGameLobby.Instance.OnLobbyGameModeChanged += UpdateLobby_Event;
        KitchenGameLobby.Instance.OnLeftLobby += LobbyManager_OnLeftLobby;
        KitchenGameLobby.Instance.OnKickedFromLobby += LobbyManager_OnLeftLobby;
        
        Hide();
    }

    private void LobbyManager_OnLeftLobby(object sender, System.EventArgs e) {
        ClearLobby();
        Hide();
    }

    private void UpdateLobby_Event(object sender, System.EventArgs e) {
        UpdateLobby();
    }

    private void UpdateLobby() {
        KitchenGameLobby.Instance.UpdatePlayerName(KitchenGameLobby.Instance.playerName);
    }

    [ContextMenu("UpdatePlayerList")]
    private void UpdatePlayerList() {
        ClearLobby();

        for (int i = 0; i < NetworkManager.Singleton.ConnectedClientsIds.Count; i++)
        {
            Transform playerSingleTransform = Instantiate(playerSingleTemplate, container);
            playerSingleTransform.gameObject.SetActive(true);
            LobbyPlayerSingleUI lobbyPlayerSingleUI = playerSingleTransform.GetComponent<LobbyPlayerSingleUI>();
        }
        Show();
    }

    public bool IsLobbyHost()
    {
        return KitchenGameLobby.Instance.joinedLobby != null && KitchenGameLobby.Instance.joinedLobby.HostId == AuthenticationService.Instance.PlayerId;
    }

    private void ClearLobby() {
        foreach (Transform child in container) {
            if (child == playerSingleTemplate) continue;
            Destroy(child.gameObject);
        }
    }

    public void StartGame()
    {
        NetworkManager.Singleton.SceneManager.LoadScene("level_1_Multiplayer", LoadSceneMode.Single);
    }

    private void Hide() {
        gameObject.SetActive(false);
    }

    public void Show() {
        gameObject.SetActive(true);
    }

}