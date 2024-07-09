using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Vector2 inputVec;
    public float speed = 4f;
    public float playerHp = 10;
    private bool isDie = false;

    Animator anim;
    Rigidbody2D rigid;
    SpriteRenderer spriter;

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (!isDie)
        {
            inputVec.x = Input.GetAxis("Horizontal");
            inputVec.y = Input.GetAxis("Vertical");

            rigid.velocity = inputVec.normalized * speed;
            if (inputVec.magnitude == 0)
            {
                AnimReset();
            }
        }
    }

    private void FixedUpdate()
    {
        //Jeong's 방법 : 물리적 이동 고려 x -> FixedUpdate에 써야함
        //if (inputVec != Vector2.zero)
        //{
        //    Vector2 nextVec = inputVec.normalized * speed * Time.fixedDeltaTime;
        //    rigid.MovePosition(rigid.position + nextVec);
        //}
        //지섭쿤 방법 : 물리적 이동 고려시.. -> Update에 써야함 
        //rigid.velocity = inputVec.normalized * speed; 
    }

    private void LateUpdate()
    {
        anim.SetFloat("Speed", inputVec.magnitude); //애니메이션 Float값 수정, 벡터의 순수한 크기 값

        AnimReset();

        if (inputVec.y > 0)
        {
            anim.SetTrigger("Forward");
        }
        else if (inputVec.y < 0)
        {
            anim.SetTrigger("Back");
        }

        else if (inputVec.x > 0)
        {
            anim.SetTrigger("Right");
            spriter.flipX = false;
        }
        else if (inputVec.x < 0)
        {
            anim.SetTrigger("Right");
            spriter.flipX = true;
        }
    }

    private void AnimReset()
    {
        anim.ResetTrigger("Forward");
        anim.ResetTrigger("Back");
        anim.ResetTrigger("Right");
    }

    public void TakeDamage(int damage)
    {
        playerHp -= damage;

        Debug.Log("남은 플레이어 체력: " + playerHp);

        if (playerHp <= 0)
        {
            isDie = true;
            Debug.Log("으앙 플레이어 죽음");
        }
    }
}
