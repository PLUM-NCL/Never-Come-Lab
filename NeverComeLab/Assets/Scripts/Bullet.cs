using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage;
    public int per;     //�Ҹ� �����

    Rigidbody2D rigid;
    private Monster monster;
    private void Awake()
    {
        monster = FindObjectOfType<Monster>();
        rigid = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Dead();
    }

    public void Init(float damage, Vector3 dir)
    {
        this.damage = damage;
        
        rigid.velocity = dir * 15f;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            monster.SetPlayerDetected(true);
            monster.TakeDamage();
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
        if (dir > 5f)
        {
            this.gameObject.SetActive(false);
        }
    }
}
