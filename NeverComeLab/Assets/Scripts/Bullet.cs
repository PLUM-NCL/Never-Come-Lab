using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage;
    public int per;     //불릿 관통수

    Rigidbody2D rigid;

    private void Awake()
    {
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
        //if (!collision.CompareTag("Enemy"))
        //    return;
        
        //사거리에 따른 SetActive(false) 추가 필요 
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
