using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkController : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    public override void OnEnable()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnected()
    {
        Debug.Log("Connected to: " + PhotonNetwork.CloudRegion + " server.");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
