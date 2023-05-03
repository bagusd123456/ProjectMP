using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    //Hider count
    //Hider Amountnya bisa dipakein list trus jumlah hidernya di hiderAmount.Add(), nanti dari script HunterBehaviour nya di hiderAmount.Remove() cuma aku lupa caranya h3h3
    public List<Transform> hiderList = new List<Transform>();
    public List<Transform> hunterList = new List<Transform>();
    [SerializeField] Transform hider;
    public int hiderAmount;
    [SerializeField] GameObject hunterWinUI;

    //Teleport coolDown
    public float InitialTpCoolDown;
    public float tpCoolDown;
    public bool isCoolDown;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;
    }

    private void Start()
    {
        //hiderList.Where(obj => obj.name == "Hider").SingleOrDefault();
        for(int i = 0; i < hiderList.Count; i++)
        {
            if (hiderList[i].CompareTag("Hider"))
            {
                hider = hiderList[i];
                hiderList.Add(hider);
            }
        }
        hiderAmount = hiderList.Count;
        tpCoolDown = InitialTpCoolDown;
    }

    private void Update()
    {
        if (tpCoolDown > 0)
        {
            tpCoolDown -= Time.deltaTime;
            if (tpCoolDown <= 0)
            {
                tpCoolDown = 0;
                isCoolDown = false;
            }
        }

        /*for (int i = 0; i < hiderList.Count; i++)
        {
            if (hiderList[i].GetComponent<PlayerMoveNetwork>().isDead.Value)
                hiderList.Remove(hiderList[i]);
        }*/

        foreach (var item in hunterList)
        {
            if (item == null)
                hunterList.Remove(item);
        }
    }

    public void HunterWin()
    {
        hunterWinUI.SetActive(true);
    }
}
