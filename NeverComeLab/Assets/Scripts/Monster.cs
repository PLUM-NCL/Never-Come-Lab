using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class Monster : MonoBehaviour
{
    private enum State { Patrol, Chase, Return }
    private State currentState = State.Patrol;

    private Vector3 initialPosition;
    private Vector3 target;
    NavMeshAgent agent;
    public TextMeshPro mark;


    [SerializeField]
    private MonsterData enemyData;

    [SerializeField]
    private GameObject projectile;

    [SerializeField]
    Transform pos;

    private float monsterHp;
    private float monsterDamage;
    private float monsterAttackSpeed;
    private int monsterType;

    private bool isHit = false;
    private bool isShooting = false;
    private bool isDie = false;
    private bool isPlayerDetected = false;

    private Animator monsterAnimator;
    private AudioSource monsterAudio;

    private float distanceToPlayer;
    private float stopChasingDistance = 5f;

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
        initialPosition = transform.position;
        monsterHp = enemyData.monsterHp;
        monsterDamage = enemyData.monsterDamage;
        monsterAttackSpeed = enemyData.monsterAttackSpeed;

        agent.speed = enemyData.monsterSpeed;

        monsterAnimator = GetComponent<Animator>();
        monsterAudio = GetComponent<AudioSource>();
        rigid = GetComponent<Rigidbody2D>();

        StartCoroutine(PatrolRoutine());
    }

    private void Update()
    {
        distanceToPlayer = Vector2.Distance(transform.position, player.position);
        
        switch (currentState)
        {
            case State.Patrol:
                if (isPlayerDetected && distanceToPlayer <= stopChasingDistance || isHit)
                {
                    monsterAnimator.SetBool("isMove", true);
                    currentState = State.Chase;
                    StopCoroutine(PatrolRoutine());
                    mark.text = "!";
                }
                break;
            case State.Chase:
               Chase();
                if (distanceToPlayer > stopChasingDistance)
                {
                    SetPlayerDetected(false);
                    currentState = State.Return;
                    mark.text = "?";
                    StartCoroutine(StopAndResume(3f));
                }
                break;
            case State.Return:
                Return();
                if (Vector2.Distance(transform.position, initialPosition) < 0.1f)
                {
                    mark.text = "";
                    currentState = State.Patrol;
                    StartCoroutine(PatrolRoutine());
                }
                break;
        }
    }

    private void Chase()
    {
        if ((!isDie && isPlayerDetected) || (!isDie && isHit))
        {
            if (!isShooting && !isHit)
            {
                StartCoroutine(Shoot());
            }
            else
            {
                agent.SetDestination(player.position);
                
            }
        }
    }

    private void Return()
    {
        agent.SetDestination(initialPosition);
        monsterAnimator.SetBool("isMove", true);
    }

    IEnumerator PatrolRoutine()
    {
        while (currentState == State.Patrol)
        {
            // 왼쪽으로 이동
            agent.SetDestination(new Vector3(initialPosition.x - 2, initialPosition.y, initialPosition.z));
            yield return new WaitUntil(() => !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance); // 2초 동안 이동

            StartCoroutine(StopAndResume(1f));

            // 오른쪽으로 이동
            agent.SetDestination(new Vector3(initialPosition.x + 2, initialPosition.y, initialPosition.z));
            yield return new WaitUntil(() => !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance); // 2초 동안 이동

            StartCoroutine(StopAndResume(1f));
        }
    }

    IEnumerator Shoot()
    {
        isShooting = true;

        
        Vector2 direction = (player.position - pos.position).normalized;
        GameObject newProjectile = Instantiate(projectile, pos.position, Quaternion.identity);
        newProjectile.GetComponent<Rigidbody2D>().velocity = direction * projectileSpeed;
        Debug.Log(rigid.velocity);
        yield return new WaitForSeconds(monsterAttackSpeed);
        isShooting = false;
    }
    IEnumerator StopAndResume(float delay)
    {
        agent.isStopped = true;
        monsterAnimator.SetBool("isMove", false);
        yield return new WaitForSeconds(delay);
        monsterAnimator.SetBool("isMove", true);
        agent.isStopped = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //float knockBackForce = 0.5f;
        Vector2 knockBack = transform.position - collision.transform.position;
        if (collision.CompareTag("Bullet"))
        {
            isHit = true;
            if(currentState == State.Patrol)
            {
                currentState = State.Chase;
            }
            
            StartCoroutine(StopAndResume(1f));
            isHit = false;
            //rigid.AddForce(knockBackForce * knockBack, ForceMode2D.Impulse); // 넉백 시 문제가 좀 있음..
        }
    }
}
