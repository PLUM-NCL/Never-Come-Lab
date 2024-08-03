using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("����!");
            Player player = collision.GetComponent<Player>();
            if (player != null)
            {
                player.TakeDamage(5); // ������ ���� ����
            }

            Destroy(gameObject);
        }
        else if(collision.CompareTag("TileMap"))
        {
            Destroy(gameObject);
        }
    }
}
