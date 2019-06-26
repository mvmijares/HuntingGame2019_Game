using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
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

    private string treeAssetPath = "Assets/Prefabs/Environment/Trees/";
    private string treeAssetName = "Trees Prefab";
 
 
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

        string savePath = treeAssetPath + treeAssetName + ".asset";

        bool exists = AssetDatabase.GetMainAssetTypeAtPath(savePath) != null;
        
        if (!exists)
        {
            if (treePrefabs.Count > 0)
            {
                for (int i = 0; i < numOfTrees; i++)
                {
                    GameObject newTree = SpawnNewTree();
                    if (prefabContainer)
                        newTree.transform.SetParent(prefabContainer.transform);
                    
                }

                CombineMeshes();
                LoadMesh(savePath);
            }
        }
        else
        {
            LoadMesh(savePath);
        }
  
    }

    private void LoadMesh(string path)
    {
        Mesh treeMesh = (Mesh)AssetDatabase.LoadAssetAtPath(path, typeof(Mesh));
        if (treeMesh)
        {
            foreach (GameObject t in spawnLocations)
            {
                Debug.Log(t.name);
                t.GetComponent<MeshFilter>().mesh = treeMesh;
                //Add Physics Collider here
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
    /// <summary>
    /// Combines all the tree meshes together.
    /// Reduces draw calls and verts used
    /// </summary>
    private void CombineMeshes()
    {
        MeshCollider meshCollider = prefabContainer.GetComponent<MeshCollider>();
        MeshFilter meshFilter = prefabContainer.GetComponent<MeshFilter>();
        
        MeshFilter[] meshFilters = prefabContainer.GetComponentsInChildren<MeshFilter>();
        CombineInstance[] combine = new CombineInstance[meshFilters.Length];

        for (int i = 0; i < meshFilters.Length; i++)
        {
            if (meshFilters[i].transform == prefabContainer.transform)
                continue;

            combine[i].mesh = meshFilters[i].sharedMesh;
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;

        }

        Mesh mesh = new Mesh();
        mesh.CombineMeshes(combine);

        meshFilter.sharedMesh = mesh;
        meshCollider.sharedMesh = mesh;

        string savePath = treeAssetPath + treeAssetName + ".asset";
        AssetDatabase.CreateAsset(mesh, savePath);

        prefabContainer.SetActive(false);
        
    }


}
