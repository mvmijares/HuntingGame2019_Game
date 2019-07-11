using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class RoundStatusBar : MonoBehaviour
{
    GameManager _gameManager;

    public Text text;
    public void InitializeStatusBar(GameManager manager)
    {
        _gameManager = manager;
        text.text = _gameManager.roundNum.ToString();
    }

    private void Update()
    {
        if (_gameManager)
        { 
            text.text = _gameManager.roundNum.ToString();
        }
    }
}
