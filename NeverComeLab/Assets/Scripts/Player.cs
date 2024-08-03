using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Vector2 inputVec;
    public float speed = 4f;
    //public float playerHp = 100;
    private bool isDie = false;
    private bool isStopped = false;
    public bool isHit = false;


    public Animator anim;
    Rigidbody2D rigid;
    public SpriteRenderer spriter;

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
            inputVec.x = Input.GetAxisRaw("Horizontal");
            inputVec.y = Input.GetAxisRaw("Vertical");

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
        if (inputVec.magnitude > 0)
        {
            isStopped = false;
            anim.speed = 1; // 애니메이션 재생 속도 정상화
            anim.SetFloat("Speed", inputVec.magnitude); // 애니메이션 Float값 수정, 벡터의 순수한 크기 값
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
        else if (!isStopped)    //움직임 멈추면 애니메이션 정지 시키기 
        {
            StopAnimation();
            isStopped = true;
        }
    }

    private void AnimReset()
    {
        anim.ResetTrigger("Forward");
        anim.ResetTrigger("Back");
        anim.ResetTrigger("Right");
    }

    private void StopAnimation()
    {
        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);  //현재 재생중인 애니메이션 정보 가져옴
        anim.Play(stateInfo.fullPathHash, 0, stateInfo.normalizedTime); //현재 애니 해시값, 애니메이션 시작부분, 현재 상태값
        anim.speed = 0; // 애니메이션 멈춤
    }

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (!collision.CompareTag("MonsterBullet"))  
    //        return;

    //    collision.gameObject.SetActive(false);
    //    playerHp -= collision.GetComponent<Bullet>().damage;
    //    Debug.Log("남은 몬스터 체력: " + playerHp);

    //    Debug.Log("남은 플레이어 체력: " + playerHp);

    //    if (playerHp > 0)
    //    {
    //        //Hit 애니메이션 관련 코드 추가 필요
    //    }
    //    else
    //    {
    //        Debug.Log("으앙 플레이어 죽음");
    //    }
    //}

    public void TakeDamage(int damage)
    {
        if (isHit == true)
            return;

        gameObject.layer = 9;
        spriter.color = new Color(1, 1, 1, 0.4f);

        GameManager.Instance.health -= damage;

        Debug.Log("남은 플레이어 체력: " + GameManager.Instance.health);

        if (GameManager.Instance.health <= 0)
        {
            isDie = true;
            Debug.Log("으앙 플레이어 죽음");
            //Destroy(gameObject);
        }

        Invoke("OffDamaged", 0.2f);
        isHit = false;
    }

    void OffDamaged()
    {
        gameObject.layer = 3;
        spriter.color = new Color(1, 1, 1, 1);
    }
}
