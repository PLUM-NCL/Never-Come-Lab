using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Monster : MonoBehaviour
{
    private Vector3 target;
    NavMeshAgent agent;

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

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();

        // 플레이어가 표면에서 움직이는 것을 방지
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }
    private void Start()
    {
        monsterHp = enemyData.monsterHp;
        monsterDamage = enemyData.monsterDamage;
        monsterAttackSpeed = enemyData.monsterAttackSpeed;

        agent.speed = enemyData.monsterSpeed;

        monsterAnimator = GetComponent<Animator>();
        monsterAudio = GetComponent<AudioSource>();
        rigid = GetComponent<Rigidbody2D>();
    }
    void SetAgentPosition()
    {
        agent.SetDestination(player.position);
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
                    agent.isStopped = true; // NavMeshAgent 멈추기
                    agent.ResetPath(); // 현재 경로 초기화
                    monsterAnimator.SetBool("isMove", false);
                }
                else
                {
                    // 플레이어 방향으로 몬스터 이동
                    //Vector2 direction = (player.position - transform.position).normalized;
                    //rigid.velocity = direction * monsterSpeed;
                    SetAgentPosition();
                    agent.isStopped = false;
                    monsterAnimator.SetBool("isMove", true);
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
        newProjectile.GetComponent<Rigidbody2D>().velocity = direction * projectileSpeed;
        Debug.Log(rigid.velocity);

        isShooting = false;
    }
}
