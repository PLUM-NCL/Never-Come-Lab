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
    }

    private void Update()
    {
        transform.Translate(Vector2.right * 5f * Time.deltaTime);

        Dead();
    }

    public void Init(float damage)
    {
        this.damage = damage;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Enemy"))
            return;

        rigid.velocity = Vector2.zero;
        gameObject.SetActive(false);
    }

    //몬스터쪽에서 Bullet과 몬스터 충돌 부분 필요함.  

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
