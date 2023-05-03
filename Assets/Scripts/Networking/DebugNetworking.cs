using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DebugNetworking : MonoBehaviour
{
    public TMP_Text textPing;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        textPing.text = "Ping: " + PhotonNetwork.GetPing() + "ms";
    }
}
