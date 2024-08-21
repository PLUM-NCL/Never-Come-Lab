using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using NavMeshPlus.Components;
using Unity.VisualScripting;

public class Baker : MonoBehaviour
{
    private NavMeshSurface navSurface;
    private NavMeshModifier navModifier;
    void Start()
    {
        navSurface = FindObjectOfType<NavMeshSurface>();
        navModifier = GetComponent<NavMeshModifier>();
        DynamicBake("Not Walkable");
    }

    private void DynamicBake(string areaName)
    {
        navModifier.area = NavMesh.GetAreaFromName(areaName);
        navSurface.BuildNavMesh();
    }
}
