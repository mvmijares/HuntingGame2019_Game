using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerStatusBar : MonoBehaviour
{
    GameManager _gameManager;
    public Text text;
    public void InitializeStatusBar(GameManager manager)
    {
        _gameManager = manager;
        text.text = FormatTime(_gameManager.GetRoundTime());
    }

    private void Update()
    {
        if (_gameManager) {
            text.text = FormatTime(_gameManager.GetRoundTime());
        }
    }

    /// <summary>
    /// Function that returns time that is formated from a float
    /// </summary>
    /// <param name="time">time based on delta time</param>
    /// <returns></returns>
    private string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time - minutes * 60f);

        return string.Format("{0:0}:{1:00}", minutes, seconds);
    }
}
