using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage;

    Rigidbody2D rigid;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        rigid.velocity = Vector2.zero;
    }

    private void Update()
    {
        transform.Translate(Vector2.right * 5f * Time.deltaTime);

        Dead();
    }

    public void Init(float damage)
    {
        this.damage = damage;
        rigid.velocity = Vector2.zero;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("TileMap"))
            return;
        if (collision.CompareTag("Enemy") || collision.CompareTag("Wall"))
        {
            rigid.velocity = Vector2.zero;
            gameObject.SetActive(false);
        }
    }

    //�����ʿ��� Bullet�� ���� �浹 �κ� �ʿ���.  

    void Dead()
    {
        Transform target = GameManager.Instance.player.transform;
        Vector3 targetPos = target.position;
        float dir = Vector3.Distance(targetPos, transform.position);
        if (dir > 5f)
        {
            this.gameObject.SetActive(false);
            rigid.velocity = Vector2.zero;
        }
    }
}
