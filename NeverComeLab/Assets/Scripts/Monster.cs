using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    [SerializeField]
    private MonsterData enemyData;

    [SerializeField]
    private GameObject projectile;

    [SerializeField]
    Transform pos;

    private float monsterHp; // 체력
    private float monsterDamage; // 공격력
    private float monsterSpeed; // 이속
    private float monsterAttackSpeed; // 공속 (낮을 수록 빠름) 딜레이 시간
    private int monsterType; // 몬스터 유형

    private bool isHit = false;
    private bool isShooting = false;
    private bool isDie = false; // 죽었는지 안죽었는지
    private bool isPlayerDetected = false;

    private Animator monsterAnimator;
    private AudioSource monsterAudio;

    private float distanceToPlayer; // 플레이어와의 거리
    private bool isAttack = false; // 공격 구분
    private float attackSpeed = 1f; // 공속 (공격 간격)
    private float stopChasingDistance = 5f; // 추적을 멈출 거리

    public Transform player;
    public float projectileSpeed = 5f;

    Rigidbody2D rigid;

    public void SetPlayerDetected(bool detected)
    {
        isPlayerDetected = detected;
    }

    private void Start()
    {
        monsterHp = enemyData.monsterHp;
        monsterDamage = enemyData.monsterDamage;
        monsterSpeed = enemyData.monsterSpeed;
        monsterAttackSpeed = enemyData.monsterAttackSpeed;

        monsterAnimator = GetComponent<Animator>();
        monsterAudio = GetComponent<AudioSource>();
        
        rigid = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {


        if (!isDie && isPlayerDetected)
        {
            if (!isShooting)
            {
                StartCoroutine(Shoot());
            }
            else
            {
                // 플레이어와 몬스터 사이의 거리 계산
                distanceToPlayer = Vector2.Distance(transform.position, player.position);
                Debug.Log(distanceToPlayer + "m");

                if (distanceToPlayer > stopChasingDistance)
                {
                    // 플레이어와의 거리가 stopChasingDistance보다 크면 추적 중지
                    SetPlayerDetected(false);
                    rigid.velocity = Vector3.zero;
                }
                else
                {
                    // 플레이어 방향으로 몬스터 이동
                    Vector2 direction = (player.position - transform.position).normalized;
                    rigid.velocity = direction * monsterSpeed;
                }
            }
        }
    }
    

    IEnumerator Shoot()
    {
        isShooting = true;
        yield return new WaitForSeconds(monsterAttackSpeed);
        Vector2 direction = (player.position - pos.position).normalized;
        GameObject newProjectile = Instantiate(projectile, pos.position, Quaternion.identity);
        Rigidbody2D rigid = newProjectile.GetComponent<Rigidbody2D>();
        rigid.velocity = direction * projectileSpeed;
        Debug.Log(rigid.velocity);
        isShooting = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Bullet") || isHit)   //피격후 0.5초간은 무적판정
            return;

        collision.gameObject.SetActive(false);
        monsterHp -= collision.GetComponent<Bullet>().damage;
        Debug.Log("남은 몬스터 체력: " + monsterHp);


        if (monsterHp > 0)
        {
            //Hit 애니메이션 관련 코드 추가 필요
            isHit = true;
            StartCoroutine(ResetHit());
        }
        else
        {
            Dead();
        }
    }

    IEnumerator ResetHit()
    {
        // 0.1초 대기
        yield return new WaitForSeconds(0.5f);
        isHit = false;
    }

    void Dead()
    {
        gameObject.SetActive(false);
        Debug.Log("으앙 몬스터 죽음");
    }
}
