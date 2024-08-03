using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("맞음!");
            Player player = collision.GetComponent<Player>();
            if (player != null)
            {
                player.TakeDamage(5); // 데미지 값을 설정
            }

            Destroy(gameObject);
        }
        else if(collision.CompareTag("TileMap"))
        {
            Destroy(gameObject);
        }
    }
}
