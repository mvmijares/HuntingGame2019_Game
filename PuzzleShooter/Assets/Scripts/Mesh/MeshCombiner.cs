using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshCombiner : MonoBehaviour
{

    MeshCollider meshCollider;
    Mesh newMesh;

    private void Awake()
    {
        MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();
        CombineInstance[] combine = new CombineInstance[meshFilters.Length];

        for (int i = 0; i < meshFilters.Length; i++) {
            if (meshFilters[i].transform == transform)
                continue;

            combine[i].mesh = meshFilters[i].sharedMesh;
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
            meshFilters[i].gameObject.SetActive(false);
        }

        newMesh = new Mesh();
        newMesh.CombineMeshes(combine);
        transform.GetComponent<MeshFilter>().mesh = newMesh;
        transform.GetComponent<MeshCollider>().sharedMesh = newMesh;
        transform.gameObject.SetActive(true);
    }
}
