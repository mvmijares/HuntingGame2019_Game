using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/* What is our game?
 * - It is our version of Duck Hunt
 * - Users can move around, aim, and shoot at targets/
 * - Objective of the game
 *      - Given specific targets to find
 * -    - Must kill targets to aquire points.
 * - What makes the game difficult?
 *      - Limited ammo.
 *      - Animals move fast over the time.
 * - Win Condition?
 *      - Users have a total amount of targets to kill.
 *      - When the round is done, users get a grading (Kills / Total)
 *      - If they get a high enough grade, they proceed to next round.
 * - Gameplay Experience
 *      - Rounds should be 1 min each
 *      - Targets should "disappear" shortly after being spawned
 * 
 * 
 * 
 */
public class GameManager : MonoBehaviour
{
    [SerializeField] Player _player;
    public Player player { get { return _player; } }
    [SerializeField] EnemyManager _eManager;
    public EnemyManager eManager { get { return _eManager; } }
    [SerializeField] KilledEnemyStatusBar _enemyStatusBar;
    public KilledEnemyStatusBar enemyStatusBar { get { return _enemyStatusBar; } }
    //Testing Purposes

    private int roundNum;
    private bool pass; // if user passes to next round.

    private bool initRound; // Handles round initialization
    private bool roundStart;
    private float startTimer = 0.0f;
    public float startTime;
    //in case the user takes too long
    [SerializeField] private float currRoundTime = 0.0f;
    public float roundTime = 60.0f;
    private bool checkRoundStatus;
    private bool start = false; //main start condition.

    private bool isSpawnRunning; // spawn coroutine condition
    public bool prototype; //Prototyping rounds. TODO : Delete later
    private float score; // score is calculated at end of each round
    private float requiredScore; //required score to pass the round.
    private int killedEnemies; //Enemies that were killed
    public int numOfEnemies; //Enemies that you need to kill

    private void Awake()
    {
        initRound = false;
        numOfEnemies = 10; // Prototype variable

        _player = FindObjectOfType<Player>();
        if (_player)
            _player.ObjectInitialize(this);

        _eManager = FindObjectOfType<EnemyManager>();
        if (_eManager)
            _eManager.Initialize(this);

        _enemyStatusBar = FindObjectOfType<KilledEnemyStatusBar>();
        if (_enemyStatusBar)
            _enemyStatusBar.InitializeStatusBar(this);

    
    }
    //Methods to handle order of execution for Update / LateUpdate
    private void Update()
    {
        ProcessGameTasks();
        _player.CustomUpdate();
    }
    private void LateUpdate()
    {
        _player.CustomLateUpdate();
    }
    /// <summary>
    /// function to handle game loop
    /// </summary>
    private void ProcessGameTasks()
    {
        if (!start)
        {
            startTimer += Time.deltaTime;
            if (startTimer >= startTime)
            {
                startTimer = 0.0f;
                start = true;
            }
        }
        else
        {
            SpawnEnemyHandler();
            HandleRoundLogic();
            CheckObjectiveStatus();
        }
    }
    /// <summary>
    /// Function that contains the taasks for each round
    /// Rounds are completed by :
    ///     - Running out of time 
    ///     - Completing the task
    ///     
    /// </summary>
    private void HandleRoundLogic()
    {
        currRoundTime += Time.deltaTime;
        if (currRoundTime >= roundTime)
        {
            checkRoundStatus = true;
            enemyStatusBar.ResetStatusBar();
        }else if (numOfEnemies == killedEnemies)
        {
            checkRoundStatus = true;
            roundNum++;
            enemyStatusBar.ResetStatusBar();
        }
    }
    /// <summary>
    /// Method to check the status after each round has been processed
    /// </summary>
    private void CheckObjectiveStatus()
    {
        if (checkRoundStatus)
        {
            if (isSpawnRunning)
            {
                isSpawnRunning = false;
                StopCoroutine(SpawnEnemyCoroutine());
            }
            start = false;
            currRoundTime = 0.0f;
            killedEnemies = 0;

            _eManager.DeleteAllEnemies();
            CalculateScore();
        }
    }
    /// <summary>
    /// Function to spawn enemies.
    /// Checks for coroutine
    /// </summary>
    private void SpawnEnemyHandler()
    {
        if (!isSpawnRunning)
        {
            StartCoroutine(SpawnEnemyCoroutine());
            isSpawnRunning = true;
        }
    }
    /// <summary>
    /// Coroutine for spawning enemies
    /// </summary>
    /// <returns></returns>
    IEnumerator SpawnEnemyCoroutine()
    {
        SpawnEnemies(1);
        yield return new WaitForSeconds(1.0f);
        isSpawnRunning = false;
    }
    /// <summary>
    /// Method for updating our killed enemy score.
    /// Opted out of using events because of scope of game.
    /// </summary>
    public void EnemyWasKilled()
    {
        killedEnemies++;
        enemyStatusBar.EnemyWasKilled();
    }
    /// <summary>
    /// Function for calculating score after each round
    /// </summary>
    private void CalculateScore()
    {
        score = killedEnemies / numOfEnemies * 100.0f;
        pass = (score < requiredScore) ? false : true;
    }
    /// <summary>
    /// Function for spawning enemies
    /// </summary>
    /// <param name="num"></param>
    private void SpawnEnemies(int num)
    {
        if (num < 1) return; //error check

        for (int i = 0; i < num; i++)
        {
            _eManager.CreateNewEnemy();
            // TODO :   Setup a way to add Enemy Definitions to Enemy Manager 
            //          based on round number.
        }
    }
}
