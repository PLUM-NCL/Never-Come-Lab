using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Vector2 inputVec;
    public float speed;
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
        inputVec.x = Input.GetAxis("Horizontal");
        inputVec.y = Input.GetAxis("Vertical");
    }

    private void FixedUpdate()
    {
        Vector2 nextVec = inputVec.normalized * speed * Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + nextVec);
    }

    //private void LateUpdate()
    //{
    //    //anim.SetFloat("Speed", inputVec.magnitude); //�ִϸ��̼� Float�� ����, ������ ������ ũ�� ��

    //    //anim.ResetTrigger("Forward");
    //    //anim.ResetTrigger("Back");
    //    //anim.ResetTrigger("Right");

    //    if (inputVec.y > 0)
    //    {
    //        anim.SetTrigger("Forward");
    //    }
    //    else if (inputVec.y < 0)
    //    {
    //        anim.SetTrigger("Back");
    //    }

    //    else if (inputVec.x > 0)
    //    {
    //        anim.SetTrigger("Right");
    //        spriter.flipX = false;
    //    }
    //    else if (inputVec.x < 0)
    //    {
    //        anim.SetTrigger("Right");
    //        spriter.flipX = true;
    //    }
    //}
}
