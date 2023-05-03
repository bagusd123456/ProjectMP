using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class MainMenuCleanUp : MonoBehaviour
{
    NetworkManager networkManager;
    GameSetupController gameSetupController;
    private void Awake()
    {
        if (NetworkManager.Singleton != null)
            Destroy(NetworkManager.Singleton.gameObject);
        if (GameSetupController.Singleton != null)
            Destroy(GameSetupController.Singleton.gameObject);
    }
}
