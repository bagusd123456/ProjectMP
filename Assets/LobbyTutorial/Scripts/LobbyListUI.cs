using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyListUI : MonoBehaviour {

    public static LobbyListUI Instance { get; private set; }

    [SerializeField] private Transform lobbySingleTemplate;
    [SerializeField] private Transform container;
    [SerializeField] private Button refreshButton;
    [SerializeField] private Button createLobbyButton;

    private void Awake() {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;

        lobbySingleTemplate.gameObject.SetActive(false);

        refreshButton.onClick.AddListener(RefreshButtonClick);
        createLobbyButton.onClick.AddListener(CreateLobbyButtonClick);
    }

    private void Start() {
        KitchenGameLobby.Instance.OnCreateLobbyStarted += LobbyManager_OnJoinedLobby;
        KitchenGameLobby.Instance.OnLobbyListChanged += LobbyManager_OnLobbyListChanged;
        KitchenGameLobby.Instance.OnJoinedLobby += LobbyManager_OnJoinedLobby;
        KitchenGameLobby.Instance.OnLeftLobby += LobbyManager_OnLeftLobby;
        KitchenGameLobby.Instance.OnKickedFromLobby += LobbyManager_OnKickedFromLobby;
    }

    private void LobbyManager_OnKickedFromLobby(object sender, KitchenGameLobby.LobbyEventArgs e) {
        Show();
    }

    private void LobbyManager_OnLeftLobby(object sender, EventArgs e) {
        Show();
    }

    private void LobbyManager_OnJoinedLobby(object sender, EventArgs e) {
        Hide();
    }

    private void LobbyManager_OnLobbyListChanged(object sender, KitchenGameLobby.OnLobbyListChangedEventArgs e) {
        UpdateLobbyList(e.lobbyList);
    }

    private void UpdateLobbyList(List<Lobby> lobbyList) {
        if(SceneManager.GetActiveScene().buildIndex == 0)
        {
            foreach (Transform child in container)
            {
                if (child == lobbySingleTemplate) continue;

                Destroy(child.gameObject);
            }

            foreach (Lobby lobby in lobbyList)
            {
                Transform lobbySingleTransform = Instantiate(lobbySingleTemplate, container);
                lobbySingleTransform.gameObject.SetActive(true);
                LobbyListSingleUI lobbyListSingleUI = lobbySingleTransform.GetComponent<LobbyListSingleUI>();
                lobbyListSingleUI.UpdateLobby(lobby);
            }
        }
        
    }

    private void RefreshButtonClick() {
        KitchenGameLobby.Instance.RefreshLobbyList();
    }

    private void CreateLobbyButtonClick() {
        //LobbyCreateUI.Instance.Show();
        LobbyCreateUI.Instance.CreateLobbyDefault();

    }

    private void Hide() {
        LobbyCreateUI.Instance.lobbyPanel.SetActive(true);
        gameObject.SetActive(false);
    }

    private void Show() {
        LobbyCreateUI.Instance.lobbyPanel.SetActive(false);
        gameObject.SetActive(true);
    }

}