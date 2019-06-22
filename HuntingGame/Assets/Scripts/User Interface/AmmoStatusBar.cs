using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmmoStatusBar : MonoBehaviour
{
    GameManager _gameManager;
    public Text text;
    Weapon playerWeapon;
    public void InitializeStatusBar(GameManager manager)
    {
        _gameManager = manager;
        playerWeapon = _gameManager.player.weapon;
    }

    private void Update()
    {
        if (_gameManager)
        {
            text.text = playerWeapon.clip.ToString();
        }
    }
}
