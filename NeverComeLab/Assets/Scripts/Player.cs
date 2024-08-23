using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public Vector2 inputVec;
    public float speed = 4f;
    //public float playerHp = 100;
    public bool isDie = false;
    public bool isHit = false;
    public bool isHide = false;
    public static bool isStop = false;
    public bool isObstacleHit = false;


    public Animator anim;
    Rigidbody2D rigid;
    public SpriteRenderer spriter;
    List<Collider2D> colliders = new List<Collider2D>();

//블럭 밀기 관련
    private GameObject nearbyBlock = null; // 플레이어가 근접한 블럭을 추적하기 위한 변수
    public KeyCode pushKey = KeyCode.Space; // 블럭을 미는 키 설정

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            colliders.ForEach(n =>
            {
                if (n.CompareTag("Lever"))
                    n.SendMessage("Use", SendMessageOptions.DontRequireReceiver);
            });
        }
    }

    private void FixedUpdate()
    {
        if (isDie || isStop == true)
        {
            AnimReset();
            inputVec = Vector2.zero;
            rigid.velocity = Vector2.zero;
            anim.speed = 0;
            return;
        }
            

        if (!isDie && !isObstacleHit)
        {
            // 블럭 밀기 처리
            if (nearbyBlock != null && Input.GetKey(pushKey))
            {
                PushBlock();
            }
            
            inputVec.x = Input.GetAxisRaw("Horizontal");
            inputVec.y = Input.GetAxisRaw("Vertical");

            rigid.velocity = inputVec.normalized * speed;
            if (inputVec.magnitude == 0)
            {
                AnimReset();
            }
            
        }
    }

    private void LateUpdate()
    {
        if (isDie)
            return; 

        if (inputVec.magnitude > 0)
        {
            anim.speed = 1; // 애니메이션 재생 속도 정상화
            anim.SetFloat("Speed", inputVec.magnitude); // 애니메이션 Float값 수정, 벡터의 순수한 크기 값
            AnimReset();

            //음향 관련(일반 걸음소리, 모래 걸음 소리) 
            if (!isHide && AudioManager.instance.isPlaying(AudioManager.Sfx.Run))   
            {
                AudioManager.instance.PlaySfx(AudioManager.Sfx.Run);
                AudioManager.instance.StopSfx(AudioManager.Sfx.Leave);
            }
            else if(isHide && AudioManager.instance.isPlaying(AudioManager.Sfx.Leave))
            {
                AudioManager.instance.PlaySfx(AudioManager.Sfx.Leave);
                AudioManager.instance.StopSfx(AudioManager.Sfx.Run);
            }

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
        else if(inputVec.magnitude == 0)    //움직임 멈추면 애니메이션 정지 시키기 
        {
            AudioManager.instance.StopSfx(AudioManager.Sfx.Run);
            AudioManager.instance.StopSfx(AudioManager.Sfx.Leave);
            StopAnimation();            
        }
    }

    private void AnimReset()
    {
        anim.ResetTrigger("Forward");
        anim.ResetTrigger("Back");
        anim.ResetTrigger("Right");
        anim.StopPlayback();
    }

    private void StopAnimation()
    {
        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);  //현재 재생중인 애니메이션 정보 가져옴
        anim.Play(stateInfo.fullPathHash, 0, 0.25f); //현재 애니 해시값, 애니메이션 시작부분, 현재 상태값(노말화됌) 
        anim.speed = 0; // 애니메이션 멈춤
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("HideObject")){
            isHide = true;
        }

        //충돌한 물체(lever)를 colliders 리스트에 추가
        if (collision.CompareTag("Lever"))
        {
            colliders.Add(collision);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("HideObject"))
        {
            isHide = false;
        }

        if (collision.CompareTag("Lever"))
        {
            colliders.Remove(collision);
        }
    }


//블럭 밀기 구현
    private void PushBlock()
    {
        // 현재 위치를 미리 저장
        Vector2 originalPosition = transform.position;
        Vector2 direction = inputVec.normalized; // 현재 플레이어의 입력 방향 사용

        // 대각선 입력 방지
        if (Mathf.Abs(direction.x) > 0.1f && Mathf.Abs(direction.y) > 0.1f)
        {
            if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
            {
                direction.y = 0f; // 가로 방향 우선
            }
            else
            {
                direction.x = 0f; // 세로 방향 우선
            }
        }

        direction = direction.normalized; // 수정된 방향을 정규화

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
//블럭 밀기 구현 (여기 위까지)


    public void TakeDamage(int damage)
    {
        if (isDie)
            return;

        if (isHit == true)
            return;

        gameObject.layer = 9;
        spriter.color = new Color(1, 1, 1, 0.4f);

        GameManager.Instance.health -= damage;
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Damage);
        Debug.Log("남은 플레이어 체력: " + GameManager.Instance.health);

        if (GameManager.Instance.health <= 0)
        {
            isDie = true;
            inputVec = Vector2.zero;
            rigid.velocity = Vector2.zero;

            anim.speed = 1;
            anim.SetTrigger("Dead");

            AudioManager.instance.StopSfx(AudioManager.Sfx.Run);
            AudioManager.instance.StopSfx(AudioManager.Sfx.Leave);
            AudioManager.instance.PlaySfx(AudioManager.Sfx.PlayerDie);

            PlayerPrefs.SetString("CurrentScene", SceneManager.GetActiveScene().name);
            PlayerPrefs.Save();

            GameManager.Instance.fade.FadeOut();
            GameManager.Instance.Invoke("GameOver", 2f);

                    
        }

        Invoke("OffDamaged", 0.2f);
        isHit = false;
    }

    void OffDamaged()
    {
        gameObject.layer = 3;
        spriter.color = new Color(1, 1, 1, 1);
    }
    

    public void UseLever()
    {
        //플레이어와 충돌이 일어난 리스트들 각각에게 해당 메세지 전송 
        colliders.ForEach(n =>
        {
            if (n.CompareTag("Lever"))
                n.SendMessage("Use", SendMessageOptions.DontRequireReceiver);
        });
    }
}
