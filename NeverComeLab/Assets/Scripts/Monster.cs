using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class Monster : MonoBehaviour
{
    private enum State { Patrol, Chase, Return }
    private State currentState = State.Patrol;

    private Vector3 initialPosition;
    NavMeshAgent agent;

    public TextMeshPro mark;


    [SerializeField]
    private MonsterData enemyData;

    [SerializeField]
    private GameObject projectile;

    [SerializeField]
    Transform pos;

    private float monsterHp = 100;
    public float Hp
    {
        get { return monsterHp; }
        set {
            monsterHp = value;
            if (monsterHp > 100)
            {
                monsterHp = 100;
                Debug.Log("Hp 100 범위 초과");
            }
            else if(monsterHp < 0)
            {
                monsterHp = 0;
                Debug.Log("Hp 0 범위 초과");
            }
            
        }

    }
    private float monsterDamage;
    private float monsterAttackSpeed;
    private int monsterType;

    private bool isHit = false;
    private bool isShooting = false;
    private bool isDie = false;
    private bool isPlayerDetected = false;
    private bool isPatrol = false;

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

        
        Patrol();
        monsterAnimator.SetBool("isForward", true);
    }

    private void Update()
    {
        if (isDie) return;

        distanceToPlayer = Vector2.Distance(transform.position, player.position); // 프레임마다 플레이어와 몬스터 사이 거리 계산
        
        switch (currentState) // 현재 상태가
        {
            case State.Patrol: // 순찰 중이면
                Patrol();
                break;

            case State.Chase: // 추적 중이면
                Chase();
                break;

            case State.Return: // 놓쳐서 돌아가는 중이면
                Return();
                break;
        }
    }

    private void Patrol()
    {
        if (!isPatrol)
        {
            StartCoroutine(PatrolRoutine());
        }

        if (isPlayerDetected && distanceToPlayer <= stopChasingDistance) // 몬스터가 플레이어를 감지하면 Chase
        {
            monsterAnimator.SetBool("isForward", true);
            currentState = State.Chase;
            StopCoroutine(PatrolRoutine());
            mark.text = "!";
        }
    }

    private void Chase()
    {

        if (!isShooting && !isHit)
        {
            StartCoroutine(Shoot());
        }
        else
        {
            agent.SetDestination(player.position);
        }

        if (distanceToPlayer > stopChasingDistance) // 플레이어와의 거리가 멀어지면 Return
        {
            SetPlayerDetected(false);

            mark.text = "?";
            StartCoroutine(StopAndResume(3f));
            currentState = State.Return;
        }
    }

    private void Return()
    {
        agent.SetDestination(initialPosition);

        if (Vector2.Distance(transform.position, initialPosition) < 0.1f) // 제자리로 돌아가면 Patrol
        {
            mark.text = "";
            currentState = State.Patrol;
            StartCoroutine(PatrolRoutine());
        }
    }

    IEnumerator PatrolRoutine()
    {
        isPatrol = true;

        // 왼쪽으로 이동
        agent.SetDestination(new Vector3(initialPosition.x - 2, initialPosition.y, initialPosition.z));
        yield return new WaitUntil(() => !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance);

        StartCoroutine(StopAndResume(1f)); // 1초 동안 멈춤

        // 오른쪽으로 이동
        agent.SetDestination(new Vector3(initialPosition.x + 2, initialPosition.y, initialPosition.z));
        yield return new WaitUntil(() => !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance);

        StartCoroutine(StopAndResume(1f)); // 1초 동안 멈춤

        isPatrol = false;

    }

    IEnumerator Shoot() // projectile 발사
    {
        isShooting = true;

        Vector2 direction = (player.position - pos.position).normalized;
        GameObject newProjectile = Instantiate(projectile, pos.position, Quaternion.identity);
        newProjectile.GetComponent<Rigidbody2D>().velocity = direction * projectileSpeed;
        Debug.Log(rigid.velocity);
        yield return new WaitForSeconds(monsterAttackSpeed);

        isShooting = false;
    }

    IEnumerator StopAndResume(float delay) // delay초 동안 멈췄다 다시 움직임
    {
        agent.isStopped = true;

        monsterAnimator.SetBool("isForward", false);
        yield return new WaitForSeconds(delay);
        monsterAnimator.SetBool("isForward", true);

        agent.isStopped = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //float knockBackForce = 0.5f;
        Vector2 knockBack = transform.position - collision.transform.position;
        if (collision.CompareTag("Bullet"))
        {
            isHit = true;
            if(currentState == State.Patrol || currentState == State.Return)
            {
                monsterAnimator.SetBool("isForward", true);
                currentState = State.Chase;
                StopCoroutine(PatrolRoutine());
                mark.text = "!";
            }
            
            StartCoroutine(StopAndResume(1f));
            isHit = false;
            //rigid.AddForce(knockBackForce * knockBack, ForceMode2D.Impulse); // 넉백 시 문제가 좀 있음..
        }
    }
}
