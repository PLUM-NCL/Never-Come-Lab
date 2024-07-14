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
    private float monsterSpeed;
    private float monsterAttackSpeed;
    private int monsterType;

    private bool isHit = false;
    private bool isShooting = false;
    private bool isDie = false;
    private bool isPlayerDetected = false;

    private Animator monsterAnimator;
    private AudioSource monsterAudio;

    private float distanceToPlayer;
    private float attackSpeed = 1f;
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

        // �÷��̾ ǥ�鿡�� �����̴� ���� ����
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
                if (isPlayerDetected && distanceToPlayer <= stopChasingDistance)
                {
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
                    agent.velocity = Vector2.zero;
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

    private void Patrol()
    {
        // ���⿡ ���� �������� �����̴� ������ ����
        monsterAnimator.SetBool("isMove", true);
    }

    private void Chase()
    {
        if (!isDie && isPlayerDetected)
        {
            if (!isShooting)
            {
                StartCoroutine(Shoot());
            }
            else
            {
                agent.SetDestination(player.position);
                agent.isStopped = false;
                monsterAnimator.SetBool("isMove", true);
            }
        }
    }

    private void Return()
    {
        
        agent.SetDestination(initialPosition);
        agent.isStopped = false;
        monsterAnimator.SetBool("isMove", true);
    }

    IEnumerator PatrolRoutine()
    {
        while (currentState == State.Patrol)
        {
            // �������� �̵�
            agent.SetDestination(new Vector3(initialPosition.x - 2, initialPosition.y, initialPosition.z));
            yield return new WaitForSeconds(2f); // 2�� ���� �̵�

            // ���������� �̵�
            agent.SetDestination(new Vector3(initialPosition.x + 2, initialPosition.y, initialPosition.z));
            yield return new WaitForSeconds(2f); // 2�� ���� �̵�
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Bullet") || isHit)   //�ǰ��� 0.5�ʰ��� ��������
            return;

        collision.gameObject.SetActive(false);
        monsterHp -= collision.GetComponent<Bullet>().damage;
        Debug.Log("���� ���� ü��: " + monsterHp);


        if (monsterHp > 0)
        {
            //Hit �ִϸ��̼� ���� �ڵ� �߰� �ʿ�
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
        // 0.1�� ���
        yield return new WaitForSeconds(0.5f);
        isHit = false;
    }

    void Dead()
    {
        gameObject.SetActive(false);
        Debug.Log("���� ���� ����");
    }
}
