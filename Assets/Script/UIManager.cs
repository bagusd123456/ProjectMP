using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    [SerializeField] PlayerMovement playerMovement;
    private void Awake()
    {
        if (instance != null) instance = this;
    }

    [SerializeField] GameObject arrow1UI, arrow2UI;

    public void EnableMovement()
    {
        playerMovement.isCanMove = true;
    }
}
