using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverWall : MonoBehaviour
{
    public bool isOpen;
    Collider2D spriteCollider;
    Renderer spriteRenderer;
    void Start()
    {
        spriteCollider = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<Renderer>();
    }

    public void Open()
    {
        if (!isOpen)
        {
            SetState(true);
            Color color = spriteRenderer.material.color; 
            color.a = 0.0f;
            spriteRenderer.material.color = color; 

        }
    }

    public void Close()
    {
        if (isOpen)
        {
            SetState(false);
            Color color = spriteRenderer.material.color;
            color.a = 1f;
            spriteRenderer.material.color = color;
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
        spriteCollider.isTrigger = open;
    }
}
