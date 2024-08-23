using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage;

    Rigidbody2D rigid;
    private Monster monster;
    private void Awake()
    {
        //monster = FindObjectOfType<Monster>();
        rigid = GetComponent<Rigidbody2D>();
        rigid.velocity = Vector2.zero;
    }

    private void Update()
    {
        // 탄환 별로 발사속도가 다르도록 설정
        if (gameObject.CompareTag("Bullet"))
        {
            transform.Translate(Vector2.right * 5f * Time.deltaTime);
        }
        else if (gameObject.CompareTag("BindBullet"))
        {
            transform.Translate(Vector2.right * 15f * Time.deltaTime);
        }

        Dead();
    }

    public void Init(float damage)
    {
        this.damage = damage;
        rigid.velocity = Vector2.zero;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        monster = collision.GetComponent<Monster>();
        if (collision.CompareTag("TileMap"))
            return;
        else if(collision.CompareTag("Wall"))
        {
            rigid.velocity = Vector2.zero;
            gameObject.SetActive(false);
        }
        else if (collision.CompareTag("Enemy"))
        {
            monster.SetPlayerDetected(true);
            if (gameObject.CompareTag("Bullet"))
            {
                monster.TakeDamage();
            }
            else if (gameObject.CompareTag("BindBullet"))
            {
                monster.TakeSleep();
            }
            rigid.velocity = Vector2.zero;
            gameObject.SetActive(false);
            //rigid.AddForce(knockBackForce * knockBack, ForceMode2D.Impulse); // �˹� �� ������ �� ����..
        }
    }

    //�����ʿ��� Bullet�� ���� �浹 �κ� �ʿ���.  

    void Dead()
    {
        Transform target = GameManager.Instance.player.transform;
        Vector3 targetPos = target.position;
        float dir = Vector3.Distance(targetPos, transform.position);
        
        // 탄환 별로 발사속도가 다르도록 설정 // 으엥 리펙토링 해줘
        if (gameObject.CompareTag("Bullet"))
        {
            if (dir > 5f)
            {
                this.gameObject.SetActive(false);
                rigid.velocity = Vector2.zero;
            }
        }
        else if (gameObject.CompareTag("BindBullet"))
        {
            if (dir > 10f)
            {
                this.gameObject.SetActive(false);
                rigid.velocity = Vector2.zero;
            }
        }
    }
}
