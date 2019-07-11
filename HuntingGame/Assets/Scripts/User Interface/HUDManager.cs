using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Created By: Michael-Vincent Mijares
 * Creation Date : 06-26-2019
 * 
 */
 /// <summary>
 /// Handles HUD related functionality.
 /// TODO : Create event management system (observer pattern / Model-View-Viewmodel)
 /// </summary>
public class HUDManager : MonoBehaviour
{
    private GameManager _gameManager;

    [SerializeField] KilledEnemyStatusBar _enemyStatusBar;
    public KilledEnemyStatusBar enemyStatusBar { get { return _enemyStatusBar; } }
    [SerializeField] AmmoStatusBar _ammoStatusBar;
    public AmmoStatusBar ammoStatusBar { get { return _ammoStatusBar; } }
    [SerializeField] RoundStatusBar _roundStatusBar;
    public RoundStatusBar roundStatusBar { get { return _roundStatusBar; } }
    [SerializeField] TimerStatusBar _timerStatusBar;
    public TimerStatusBar timerStatusBar { get { return _timerStatusBar; } }
    [SerializeField] FadeScreen _fadeScreen;
    public FadeScreen fadeScreen { get { return _fadeScreen; } }
    public void InitializeHUD(GameManager manager)
    {
        _gameManager = manager;
        InitializeUserInterface();
    }
    /// <summary>
    /// Intiialization for User Interface scripts
    /// TODO : Make a User Interface Manager
    /// </summary>
    private void InitializeUserInterface()
    {
        _enemyStatusBar = FindObjectOfType<KilledEnemyStatusBar>();
        if (_enemyStatusBar)
            _enemyStatusBar.InitializeStatusBar(_gameManager);

        _ammoStatusBar = FindObjectOfType<AmmoStatusBar>();
        if (_ammoStatusBar)
            _ammoStatusBar.InitializeStatusBar(_gameManager);

        _roundStatusBar = FindObjectOfType<RoundStatusBar>();
        if (_roundStatusBar)
            _roundStatusBar.InitializeStatusBar(_gameManager);

        _timerStatusBar = FindObjectOfType<TimerStatusBar>();
        if (_timerStatusBar)
            _timerStatusBar.InitializeStatusBar(_gameManager);

        _fadeScreen = FindObjectOfType<FadeScreen>();
        if (_fadeScreen)
        {
            _fadeScreen.InitializeFadeScreenBar(_gameManager);
            _fadeScreen.FadeEvent(FadeType.Out);
        }
    }
}
