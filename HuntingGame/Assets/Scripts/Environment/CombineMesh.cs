using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
public class CombineMesh : MonoBehaviour
{

    //public GameObject prefabContainer;
    //private string treeAssetPath = "Assets/Resources/Terrain/";
    //private string treeAssetName = "Trees Prefab";
    ///// <summary>
    ///// Combines all the tree meshes together.
    ///// Reduces draw calls and verts used
    ///// </summary>
    //private void CombineMeshes()
    //{
    //    MeshCollider meshCollider = prefabContainer.GetComponent<MeshCollider>();
    //    MeshFilter meshFilter = prefabContainer.GetComponent<MeshFilter>();

    //    MeshFilter[] meshFilters = prefabContainer.GetComponentsInChildren<MeshFilter>();
    //    CombineInstance[] combine = new CombineInstance[meshFilters.Length];

    //    for (int i = 0; i < meshFilters.Length; i++)
    //    {
    //        if (meshFilters[i].transform == prefabContainer.transform)
    //            continue;

    //        combine[i].mesh = meshFilters[i].sharedMesh;
    //        combine[i].transform = meshFilters[i].transform.localToWorldMatrix;

    //    }

    //    Mesh mesh = new Mesh();
    //    mesh.CombineMeshes(combine);

    //    meshFilter.sharedMesh = mesh;
    //    meshCollider.sharedMesh = mesh;

    //    string savePath = treeAssetPath + treeAssetName + ".asset";

    //    AssetDatabase.CreateAsset(mesh, savePath);

    //    prefabContainer.SetActive(false);
    //}


}
#endif