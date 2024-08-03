using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SortingLayerInMeshRenderer : MonoBehaviour
{

    public string sortingLayerName;
    public int sortingOrder;

    void Start()
    {
        MeshRenderer mesh = GetComponent<MeshRenderer>();
        mesh.sortingLayerName = sortingLayerName;
        mesh.sortingOrder = sortingOrder;
    }
}
// [출처] 유니티 텍스트메쉬 Text Mesh|작성자 고도
