using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using Unity.Services.Authentication;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyUI : MonoBehaviour {
    public static LobbyUI Instance { get; private set; }


    [SerializeField] private Transform playerSingleTemplate;
    [SerializeField] private Transform container;
    [SerializeField] private TextMeshProUGUI lobbyNameText;
    [SerializeField] private TextMeshProUGUI playerCountText;
    //[SerializeField] private TextMeshProUGUI gameModeText;
    //[SerializeField] private Button changeMarineButton;
    //[SerializeField] private Button changeNinjaButton;
    //[SerializeField] private Button changeZombieButton;
    [SerializeField] private Button leaveLobbyButton;
    [SerializeField] private Button startGameButton;

    [SerializeField] public GameObject lobbyPanel;

    private void Awake() {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;

        playerSingleTemplate.gameObject.SetActive(false);

        /*changeMarineButton.onClick.AddListener(() => {
            LobbyManager.Instance.UpdatePlayerCharacter(LobbyManager.PlayerCharacter.Marine);
        });
        changeNinjaButton.onClick.AddListener(() => {
            LobbyManager.Instance.UpdatePlayerCharacter(LobbyManager.PlayerCharacter.Ninja);
        });
        changeZombieButton.onClick.AddListener(() => {
            LobbyManager.Instance.UpdatePlayerCharacter(LobbyManager.PlayerCharacter.Zombie);
        });*/

        leaveLobbyButton.onClick.AddListener(() => {
            KitchenGameLobby.Instance.LeaveLobby();
        });
        
    }

    public void UpdateStartButton()
    {
        if (IsLobbyHost())
        {
            startGameButton.onClick.AddListener(() =>
            {
                KitchenGameLobby.Instance.StartGame();
            });
        }

        else
        {
            startGameButton.GetComponentInChildren<TMP_Text>().text = "READY";
            /*startGameButton.onClick.AddListener(() =>
            {
                KitchenGameLobby.Instance.UpdatePlayerReady("1");
            });*/
        }
    }

    private void Start() {
        KitchenGameLobby.Instance.OnCreateLobbyStarted += UpdateLobby_Event;
        KitchenGameLobby.Instance.OnJoinedLobby += LobbyManager_OnJoinLobby;
        KitchenGameLobby.Instance.OnJoinedLobbyUpdate += UpdateLobby_Event;
        KitchenGameLobby.Instance.OnLobbyGameModeChanged += UpdateLobby_Event;
        KitchenGameLobby.Instance.OnLeftLobby += LobbyManager_OnLeftLobby;
        KitchenGameLobby.Instance.OnKickedFromLobby += LobbyManager_OnLeftLobby;
        KitchenGameLobby.Instance.OnGameStarted += Instance_OnGameStarted;
        Hide();
    }

    private void Instance_OnGameStarted(object sender, System.EventArgs e)
    {
        KitchenGameLobby.Instance.OnCreateLobbyStarted -= UpdateLobby_Event;
        KitchenGameLobby.Instance.OnJoinedLobby -= LobbyManager_OnJoinLobby;
        KitchenGameLobby.Instance.OnJoinedLobbyUpdate -= UpdateLobby_Event;
        KitchenGameLobby.Instance.OnLobbyGameModeChanged -= UpdateLobby_Event;
        KitchenGameLobby.Instance.OnLeftLobby -= LobbyManager_OnLeftLobby;
        KitchenGameLobby.Instance.OnKickedFromLobby -= LobbyManager_OnLeftLobby;
    }

    private void LobbyManager_OnLeftLobby(object sender, System.EventArgs e) {
        ClearLobby();
        Hide();
    }

    private void LobbyManager_OnJoinLobby(object sender, System.EventArgs e){
        Show();
        UpdateLobby_Event(this,System.EventArgs.Empty);
    }

    private void UpdateLobby_Event(object sender, System.EventArgs e) {
        UpdateLobby();
    }

    private void UpdateLobby() {
        if(SceneManager.GetActiveScene().name == "Scene_LobbyMP")
        {
            KitchenGameLobby.Instance.UpdatePlayerName(KitchenGameLobby.Instance.playerName);
            UpdateLobby(KitchenGameLobby.Instance.GetLobby());
        }
    }

    private void UpdateLobbyList(List<Lobby> lobbyList)
    {
        foreach (Transform child in container)
        {
            if (child == playerSingleTemplate) continue;
            Destroy(child.gameObject);
        }

        foreach (Lobby lobby in lobbyList)
        {
            Transform lobbyTransform = Instantiate(playerSingleTemplate, container);
            lobbyTransform.gameObject.SetActive(true);
            LobbyPlayerSingleUI lobbyPlayerSingleUI = lobbyTransform.GetComponent<LobbyPlayerSingleUI>();

            lobbyPlayerSingleUI.SetKickPlayerButtonVisible(
                LobbyManager.Instance.IsLobbyHost() &&
                lobby.Id != AuthenticationService.Instance.PlayerId // Don't allow kick self
            );
            //lobbyTransform.GetComponent<LobbyListSingleUI>().SetLobby(lobby);
        }
    }

    private void UpdateLobby(Lobby lobby) {
        //ClearLobby();
        if(container != null)
        {
            foreach (Transform child in container)
            {
                if (child == playerSingleTemplate) continue;
                Destroy(child.gameObject);
            }
        }

        foreach (Player player in lobby.Players) {
            Transform playerSingleTransform = Instantiate(playerSingleTemplate, container);
            playerSingleTransform.gameObject.SetActive(true);
            LobbyPlayerSingleUI lobbyPlayerSingleUI = playerSingleTransform.GetComponent<LobbyPlayerSingleUI>();

            lobbyPlayerSingleUI.SetKickPlayerButtonVisible(
                IsLobbyHost() &&
                player.Id != AuthenticationService.Instance.PlayerId // Don't allow kick self
            );
            lobbyPlayerSingleUI.UpdatePlayer(player);
        }
        UpdateStartButton();
        lobbyNameText.text = lobby.Name;
        //playerCountText.text = lobby.Players.Count + "/" + lobby.MaxPlayers;
        //gameModeText.text = lobby.Data[LobbyManager.KEY_GAME_MODE].Value;
        Show();
    }

    public bool IsLobbyHost()
    {
        return KitchenGameLobby.Instance.joinedLobby != null && KitchenGameLobby.Instance.joinedLobby.HostId == AuthenticationService.Instance.PlayerId;
    }

    private void ClearLobby() {
        if(SceneManager.GetActiveScene().name == "Scene_LobbyMP Fix")
        {
            foreach (Transform child in container)
            {
                if (child == playerSingleTemplate) continue;
                Destroy(child.gameObject);
            }
        }
    }

    private void Hide() {
        gameObject.SetActive(false);
    }

    public void Show() {
        gameObject.SetActive(true);
    }

}