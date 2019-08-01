using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuInterface : MonoBehaviour
{
    public Button helpButton;
    /// <summary>
    /// Start button pressed method
    /// Loads game scene
    /// </summary>
    public void OnStartButtonPressed()
    {
        SceneManager.LoadScene("Game");
    }
    /// <summary>
    /// Exit button pressed method
    /// Quits the application
    /// </summary>
    public void OnExitButtonPressed()
    {
        Application.Quit();
    }

   

}
