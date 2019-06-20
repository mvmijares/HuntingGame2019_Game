using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KilledEnemyStatusBar : MonoBehaviour
{
    GameManager _gameManager;
    [Tooltip("Image for displaying how many enemies left.")]
    public Sprite enemySprite;
    public Vector2 imageSize;
    public Vector2 offset;
    [SerializeField] private GameObject[] imageObjects;
    [SerializeField]private int lastIndex;
    private float xPos;
    /// <summary>
    /// Initialization function
    /// </summary>
    /// <param name="manager"></param>
    public void InitializeStatusBar(GameManager manager)
    {
        _gameManager = manager;
        xPos = (-imageSize.x - offset.x) * 5; // 5 units to the left
        imageObjects = new GameObject[_gameManager.numOfEnemies];
        lastIndex = _gameManager.numOfEnemies - 1;
        for (int i = 0; i < _gameManager.numOfEnemies; i++)
        {
            GameObject clone = new GameObject("Enemy Image " + i.ToString());
            clone.transform.SetParent(transform);
            clone.AddComponent<Image>().sprite = enemySprite;
            clone.GetComponent<RectTransform>().anchoredPosition = new Vector2(xPos, 0);
            imageObjects[i] = clone;
            xPos += imageSize.x + offset.x;
        }
    }
    /// <summary>
    /// Turns the images back to active and resets the last index of the array
    /// </summary>
    public void ResetStatusBar()
    {
        lastIndex = _gameManager.numOfEnemies - 1;
        for(int i = 0; i < _gameManager.numOfEnemies; i++)
        {
            imageObjects[i].SetActive(true);
        }
    }
    /// <summary>
    /// Deactivates the last image of the image array and updates the last index.
    /// </summary>
    public void EnemyWasKilled()
    {
        if (lastIndex >= 0) //Also called when we destroy every enemy at end of each round
        {
            imageObjects[lastIndex].SetActive(false);
            lastIndex--;
        }
    }
   
}
