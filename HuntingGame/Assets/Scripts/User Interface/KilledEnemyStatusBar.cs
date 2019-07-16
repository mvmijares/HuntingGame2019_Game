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
    [SerializeField] private Sprite[] enemySprites;
    [SerializeField] private GameObject[] imageObjects;
    [SerializeField] private int lastIndex; // last index of enemy image
    private float xPos;
    EnemyType type = EnemyType.None; 
    /// <summary>
    /// Initialization function
    /// </summary>
    /// <param name="manager"></param>
    public void InitializeStatusBar(GameManager manager)
    {
        _gameManager = manager;
        LoadArtAssets();
        CreateImagePrefabs();
    }
    /// <summary>
    /// Loads all art assets for the enemy bar
    /// </summary>
    private void LoadArtAssets()
    {
        //Maybe use structs for data type?
        enemySprites = new Sprite[4]; // Change this to a list if we plan to expand this
        enemySprites[0] = Resources.Load<Sprite>("Art/Chicken");
        enemySprites[1] = Resources.Load<Sprite>("Art/Rooster");
        enemySprites[2] = Resources.Load<Sprite>("Art/Pig");
        enemySprites[3] = Resources.Load<Sprite>("Art/Goat");
    }
    /// <summary>
    /// Instantiates image prefabs for the enemy count
    /// </summary>
    private void CreateImagePrefabs()
    {
        xPos = (-imageSize.x - offset.x) * 5; // 5 units to the left
        imageObjects = new GameObject[_gameManager.numOfEnemies];
        lastIndex = _gameManager.numOfEnemies - 1;
        for (int i = 0; i < _gameManager.numOfEnemies; i++)
        {
            GameObject clone = new GameObject("Enemy Image " + i.ToString());
            clone.transform.SetParent(transform);
            clone.AddComponent<Image>();
            clone.GetComponent<RectTransform>().anchoredPosition = new Vector2(xPos, 0);
            imageObjects[i] = clone;
            clone.SetActive(false);
            xPos += imageSize.x + offset.x;
        }
    }
    /// <summary>
    /// Grabs the enemy type from game manager. 
    /// Switches the image to the correct image type
    /// TODO : Instead of public functions, use events to handle image calls
    /// </summary>
    /// <param name="type"></param>
    public void SetEnemyType(EnemyType type)
    {
        this.type = type;

        switch (this.type)
        {
            case EnemyType.None:
                {
                    Debug.Log("Enemy type has not been assigned");
                    break;
                }
            case EnemyType.Chicken:
                {
                    SetImages(enemySprites[0]); 
                    break;
                }
            case EnemyType.Rooster:
                {
                    SetImages(enemySprites[1]);
                    break;
                }
            case EnemyType.Pig:
                {
                    SetImages(enemySprites[2]);
                    break;
                }
            case EnemyType.Goat:
                {
                    SetImages(enemySprites[3]);
                    break;
                }
        }
    }
    /// <summary>
    /// Sets all the sprites for each image object
    /// </summary>
    /// <param name="sprite"></param>
    private void SetImages(Sprite sprite)
    {
        foreach(GameObject i in imageObjects)
        {
            i.GetComponent<Image>().sprite = sprite;
            i.SetActive(true);
        }
    }
    /// <summary>
    /// Hides the image bar until we set the enemy types again
    /// </summary>
    public void HideImages()
    {
        foreach(GameObject i in imageObjects)
        {
            i.SetActive(false);
        }
    }
    /// <summary>
    /// Deactivates the last image of the image array and updates the last index.
    /// </summary>
    public void EnemyWasKilled()
    {
        if (lastIndex >= 0) //Also called when we destroy every enemy at end of each round
        {
            imageObjects[lastIndex].GetComponent<Image>().sprite = null;
            imageObjects[lastIndex].SetActive(false);
            lastIndex--;
        }
    }
   
}
