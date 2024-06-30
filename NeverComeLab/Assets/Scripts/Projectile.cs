using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("맞음!");
            Player player = collision.GetComponent<Player>();
            if (player != null)
            {
                player.takeDamage(1f); // 데미지 값을 설정
            }

            Destroy(gameObject);
        }
    }
}
