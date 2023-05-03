using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HunterBehaviour : MonoBehaviour
{
    GameManager _gameManager;
    private void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Hider"))
        {
            _gameManager.hiderAmount--;
            if (_gameManager.hiderAmount == 0)
            {
                _gameManager.HunterWin();
            }
        }
    }
}
