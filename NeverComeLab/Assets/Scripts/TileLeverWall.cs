using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileLeverWall : MonoBehaviour
{
    public bool isOpen;
    //Collider2D spriteCollider;
    TilemapCollider2D tilemapCollider;
    Renderer spriteRenderer;
    void Start()
    {
        //spriteCollider = GetComponent<Collider2D>();
        tilemapCollider = GetComponent<TilemapCollider2D>();
        spriteRenderer = GetComponent<Renderer>();
        Color color = spriteRenderer.material.color;
        color.a = 1f;
        spriteRenderer.material.color = color;
    }

    public void Open()
    {
        print("�̰� �� ������?");
        if (!isOpen)
        {
            SetState(true);
            Color color = spriteRenderer.material.color;
            color.a = 0.0f;
            spriteRenderer.material.color = color;  //����ȭ

        }
    }

    public void Close()
    {
        if (isOpen)
        {
            SetState(false);
            Color color = spriteRenderer.material.color;
            color.a = 1f;
            spriteRenderer.material.color = color;  //������� 
        }
    }

    public void Toggle()
    {
        if (isOpen)
            Close();
        else
            Open();
    }

    void SetState(bool open)
    {
        isOpen = open;
        tilemapCollider.isTrigger = open;
    }
}
