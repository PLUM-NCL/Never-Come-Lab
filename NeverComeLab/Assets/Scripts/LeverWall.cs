using NavMeshPlus.Components;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Tilemaps;

public class LeverWall : MonoBehaviour
{
    public bool isOpen;
    Collider2D spriteCollider;
    Renderer spriteRenderer;

    private NavMeshSurface navSurface;
    private NavMeshModifier navModifier;
    void Start()
    {
        spriteCollider = GetComponent<Collider2D>();
        if (spriteCollider == null)
            spriteCollider = GetComponent<TilemapCollider2D>();

        spriteRenderer = GetComponent<Renderer>();

        navModifier = GetComponent<NavMeshModifier>();
        navSurface = FindObjectOfType<NavMeshSurface>();

        if (isOpen)
            Invisibility();
        else if (!isOpen)
            Visibility();
    }

    public void Visibility()
    {
        Color color = spriteRenderer.material.color;
        color.a = 1f;
        spriteRenderer.material.color = color;  //원래대로 
        
    }
    public void Invisibility()
    {
        Color color = spriteRenderer.material.color;
        color.a = 0.0f;
        spriteRenderer.material.color = color;  //투명화
        
    }

    public void Open()
    {
        isOpen = !isOpen;
        Invisibility();
        spriteCollider.isTrigger = !spriteCollider.isTrigger;
        DynamicBake("Walkable");
    }

    public void Close()
    {
        isOpen = !isOpen;
        Visibility();
        spriteCollider.isTrigger = !spriteCollider.isTrigger;
        DynamicBake("Not Walkable");
    }

    public void Toggle()
    {
        if (isOpen)
            Close();
        else if(!isOpen)
            Open();
    }

    private void DynamicBake(string areaName)
    {
        navModifier.area = NavMesh.GetAreaFromName(areaName);
        navSurface.BuildNavMesh();
    }
}
