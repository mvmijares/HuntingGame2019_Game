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
 *  
 * 
 * 
 * 
 */
public class GameManager : MonoBehaviour
{
    [SerializeField] Player _player;
    [SerializeField] EnemyManager _eManager;
    //Testing Purposes
    private int numOfEnemies; //Enemies that you need to kill
    private int totalEnemies; //Total number of enemies that need to spawn
    private int killedEnemies; //Enemies that were killed
    private float grade; // Grade calculated at end of each round
    private int roundNum; 
    private bool pass; // if user passes to next round.
    private bool lose; // if user has to reset game.
    private bool initRound; // Handles round initialization
    private bool roundStart;
    private float startTimer = 0.0f;
    public float startTime;
    //in case the user takes too long
    private float currTime = 0.0f; 
    private float maxTime = 60.0f;
    //Initialization
    private void Awake()
    {
        totalEnemies = 10;
        _player = FindObjectOfType<Player>();
        if (_player)
            _player.ObjectInitialize(this);

        _eManager = FindObjectOfType<EnemyManager>();
        if (_eManager)
            _eManager.Initialize(this);

        initRound = false;
    }
    //Methods to handle order of execution for Update / LateUpdate
    private void Update()
    {
        _player.CustomUpdate();
        HandleGameLogic();
    }
    private void LateUpdate()
    {
        _player.CustomLateUpdate();
    }
    private void HandleGameLogic()
    {
        startTimer += Time.deltaTime;
        if (startTimer >= startTime)
        {
            startTimer = 0.0f;
            roundStart = true;
        }

        if (roundStart)
        {
            HandleRoundLogic();
        }
    }
    private void HandleRoundLogic()
    {
        if (!initRound)
        {
            SpawnEnemies(totalEnemies); // prototype value;
            initRound = !initRound;
        }
    }
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
    private void WinCondition()
    {

    }
    private void LoseCondition()
    {

    }
}
