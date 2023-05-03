using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class LobbyManager2 : NetworkBehaviour
{
    string KEY_START_GAME;

    private Lobby hostLobby;
    private Lobby joinedLobby;
    private float heartBeatTimer;
    private float lobbyPollTimer;
    private string playerName;
    private bool IsPlayerInLobby;
    private bool IsLobbyHost;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    /*
    private async void HandleLobbyPolling()
    {
        if(joinedLobby != null)
        {
            lobbyPollTimer -= Time.deltaTime;
            if(lobbyPollTimer < 0f)
            {
                float lobbyPollTimerMax = 1.1f;
                lobbyPollTimer = lobbyPollTimerMax;

                joinedLobby = await LobbyService.Instance.GetLobbyAsync(joinedLobby.Id);

               // OnJoinedLobbyUpdate?.Invoke(this, new LobbyEventArgs { Lobby = joinedLobby });

                if (!IsPlayerInLobby())
                {
                    //player was kicked out of this lobby
                    Debug.Log("Kicked from Lobby!");

                    OnKickedFromLobby?.Invoke(this, new LobbyEventArgs { Lobby = joinedLobby });

                    joinedLobby = null;
                }

                if (joinedLobby.Data[KEY_START_GAME].Value != "0")
                {
                    // Start Game!
                    if (!IsLobbyHost())
                    {
                        RelayHandler.instance.JoinRelay(joinedLobby.Data[KEY_START_GAME].value);
                    }

                    joinedLobby = null;

                    onGameStarted?.Invoke(this, EventArgs.Empty);
                }
            }
        }
    }*/

    /*public async void CreateLobby(string lobbyName, int maxPlayers, bool isPrivate)
    {
        Player player = GetPlayer();

        CreateLobbyOptions options = new CreateLobbyOptions
        {
            Player = player,
            IsPrivate = isPrivate,
            Data = new Dictionary<string, DataObject>
            {
                {KEY_START_GAME, new DataObject(DataObject.VisibilityOptions.Member, "0") }
            }
        };
    }*/
    /*
    public async void StartGame()
    {
        if (IsHost)
        {
            try
            {
                Debug.Log("Start Game");

                string relayCode = await RelayHandler.instance.CreateRelay();

                Lobby lobby = await Lobbies.Instance.UpdateLobbyAsync(joinedLobby.Id, new UpdateLobbyOptions
                {
                    Data = new Dictionary<string, DataObject>
                    {
                        KEY_START_GAME, new DataObject(DataObject.VisibilityOptions.Member, relayCode)
                    }
                });
            }
            catch(LobbyServiceException e)
            {
                Debug.Log(e);
            }
        }
    }*/
}
