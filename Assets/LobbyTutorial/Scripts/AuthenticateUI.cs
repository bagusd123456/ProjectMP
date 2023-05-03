using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AuthenticateUI : MonoBehaviour {

    public static AuthenticateUI Instance;


    [SerializeField] private Button authenticateButton;

    private void Awake() {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;

        authenticateButton.onClick.AddListener(() => {
            string playerName = EditPlayerName.Instance.GetPlayerName();
            PlayerPrefs.SetString("_playerName", playerName);
            KitchenGameLobby.Instance.Authenticate(playerName);
            Hide();
        });
    }

    public void Show(){
        gameObject.SetActive(true);
    }

    public void Hide() {
        gameObject.SetActive(false);
    }

}