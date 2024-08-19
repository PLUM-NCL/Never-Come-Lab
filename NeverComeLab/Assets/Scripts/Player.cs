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

    private GameObject nearbyBlock = null; // 플레이어가 근접한 블럭을 추적하기 위한 변수
    public KeyCode pushKey = KeyCode.Space; // 블럭을 미는 키 설정

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
            // 블럭 밀기 처리
            if (nearbyBlock != null && Input.GetKeyDown(pushKey))
            {
                PushBlock();
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

    private void PushBlock()
    {
        // 현재 위치를 미리 저장
        Vector2 originalPosition = transform.position;

        Vector2 direction = inputVec.normalized; // 현재 플레이어의 입력 방향 사용
        if (direction != Vector2.zero)
        {
            Vector2 targetPosition = (Vector2)nearbyBlock.transform.position + direction; // 블럭의 목표 위치 계산

            RaycastHit2D hit = Physics2D.Raycast(nearbyBlock.transform.position, direction, 1f, LayerMask.GetMask("Obstacle"));

            if (hit.collider == null) // 목표 위치에 장애물이 없으면
            {
                nearbyBlock.transform.position = targetPosition; // 블럭을 한 칸 이동시킴
            }
        }

        // 블럭을 밀 때 플레이어의 위치를 고정
        transform.position = originalPosition;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Block"))
        {
            nearbyBlock = collision.gameObject; // 블럭과 충돌 시 블럭을 추적
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Block"))
        {
            nearbyBlock = null; // 블럭과의 충돌이 끝나면 추적을 해제
        }
    }
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
