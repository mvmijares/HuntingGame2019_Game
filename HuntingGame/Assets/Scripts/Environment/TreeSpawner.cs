using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
/// <summary>
/// TODO : Make an editor script for automating tree spawning
/// </summary>
public class TreeSpawner : MonoBehaviour
{
    public List<GameObject> treePrefabs;
    
    [Range(1,100), Tooltip("Number of trees per section.")]
    public int numOfTrees;
    public Transform spawnCenter;
    public float spawnRadius;
    public float raycastHeight;
    public float raycastDistance;
    public float treeOffsetFromGround;
    [Tooltip("Containers for each section of trees")]
    public GameObject treeContainer;
    private List<GameObject> spawnLocations;
    public Material treeMaterial;
    public GameObject prefabContainer;
    [SerializeField] private int groundLayerMask;


 
 
    /// <summary>
    /// Initialization
    /// </summary>
    private void Awake()
    {
        groundLayerMask = 1 << LayerMask.NameToLayer("Ground");
        spawnLocations = new List<GameObject>();

        Transform[] locations = treeContainer.GetComponentsInChildren<Transform>();
        foreach(Transform t in locations)
        {
            if (t == treeContainer.transform)
                continue;

            spawnLocations.Add(t.gameObject);
        }
        if (numOfTrees > 100)
            numOfTrees = 100;

        LoadMesh();
    }
    /// <summary>
    /// Loads mesh and sets them for each tree spawn location.
    /// </summary>
    /// <param name="path"></param>
    private void LoadMesh()
    {
        Mesh treeMesh = Resources.Load<Mesh>("Terrain/Trees Prefab");

        if (treeMesh)
        {
            foreach (GameObject t in spawnLocations)
            {
                t.GetComponent<MeshFilter>().mesh = treeMesh;
                //Add Physics Collider here
                t.GetComponent<MeshCollider>().sharedMesh = treeMesh;
                t.GetComponent<MeshRenderer>().sharedMaterial = treeMaterial;
               
            }

            prefabContainer.SetActive(false);
        }
        else
        {
            Debug.Log("Could not load tree mesh correctly");
        }
    
    }

    /// <summary>
    /// Handles new tree creation
    /// </summary>
    /// <returns></returns>
    private GameObject SpawnNewTree()
    {
        GameObject tree = null;
        int choice = UnityEngine.Random.Range(0, treePrefabs.Count - 1);

        tree = Instantiate(treePrefabs[choice], GetTreeSpawnLocation(), treePrefabs[choice].transform.rotation) as GameObject;

        return tree;
    }
    /// <summary>
    /// Creates a location for the new tree.
    /// </summary>
    /// <returns></returns>
    private Vector3 GetTreeSpawnLocation()
    {
        Vector3 position, raycastPosition;
        bool groundCheck = false;
   
        do
        {
            position = UnityEngine.Random.insideUnitSphere * spawnRadius + spawnCenter.position;
            raycastPosition = new Vector3(position.x, raycastHeight, position.z);

            RaycastHit hit;

            if (Physics.Raycast(raycastPosition, Vector3.down, out hit, raycastDistance, groundLayerMask))
            {
                groundCheck = true;
                position = new Vector3(position.x, hit.point.y + treeOffsetFromGround, position.z);
            }
            else
            {
                groundCheck = false;
            }
        } while (!groundCheck);

        return position;
    }
   
}
