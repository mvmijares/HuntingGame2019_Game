using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioStatusBar : MonoBehaviour
{
    GameManager _gameManager;
    AudioHandler _audioHandler;
    public Text volumeText;
    private float volume;

    public void InitializeStatusBar(GameManager manager)
    {
        _gameManager = manager;
        _audioHandler = _gameManager.audioHandler;   
    }

    private void Update()
    {
        volume = _audioHandler.GetVolume();
        volume = volume * 100f;
        int volumeToInt = Mathf.RoundToInt(volume);

        if (volumeText)
            volumeText.text = volumeToInt.ToString();
    }
}
