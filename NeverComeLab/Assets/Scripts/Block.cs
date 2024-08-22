using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{    
    public LayerMask obstacleLayer;

    public bool Move(Vector2 direction)
    {
        Vector2 targetPosition = (Vector2)transform.position + direction;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, 1f, obstacleLayer);

        if (hit.collider == null)
        {
            transform.position = targetPosition; // 블럭을 한 칸 이동시킴
            return true;
        }

        return false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // 플레이어와 충돌 시 필요한 동작을 처리
        }
    }
}
