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

    private SpriteRenderer spriteRenderer;


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
            if (monsterHp > 10000000) // Hp�� 100 �̻� ���ö�
            {
                monsterHp = 100;
                Debug.Log("Hp 100 �ѵ� �ʰ�");
            }
            else if(monsterHp <= 0) // Hp�� 0 ���Ϸ� �������� ����
            {
                Death();
                mark.text = "�ٟy";
                Debug.Log("�ٟy");
            }
            
        }

    }
    private float monsterDamage;
    private float monsterAttackSpeed;
    private int monsterType;

    private bool isHit = false; // ���ݹ���?
    private bool isShooting = false; // �߻� ��?
    private bool isDie = false; // ����?
    private bool isPlayerDetected = false; // �÷��̾� �߰�?
    private bool isPatrol = false; // ���� ��?
    private bool isBlink = false; // ����?

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

        spriteRenderer = GetComponent<SpriteRenderer>();
        monsterAnimator = GetComponent<Animator>();
        monsterAudio = GetComponent<AudioSource>();
        rigid = GetComponent<Rigidbody2D>();

        Patrol();
    }

    private void Update()
    {
        if (isDie) return; // ������ ���� ���ϵ���

        AnimationSet(); // �����¿� �ִϸ��̼�

        distanceToPlayer = Vector2.Distance(transform.position, player.position); // �����Ӹ��� �÷��̾�� ���� ���� �Ÿ� ���

        switch (currentState) // ���� ���°�
        {
            case State.Patrol: // ���� ���̸�
                Patrol();
                break;

            case State.Chase: // ���� ���̸�
                Chase();
                break;

            case State.Return: // ���ļ� ���ư��� ���̸�
                Return();
                break;
        }
    }

    private void AnimationSet() // ���� �����¿� �ִϸ��̼� ����
    {
        Vector3 velocity = agent.velocity;
        float inputX = 0f;
        float inputY = 0f;

        if (velocity.x > 0f && Mathf.Abs(velocity.y) < Mathf.Abs(velocity.x)) // right �̵�
        {
            inputX = 1f;
            inputY = 0f;
        }
        else if (velocity.x < 0f && Mathf.Abs(velocity.y) < Mathf.Abs(velocity.x)) // left �̵�
        {
            inputX = -1f;
            inputY = 0f;
        }
        else if (velocity.y > 0f && Mathf.Abs(velocity.y) > Mathf.Abs(velocity.x)) // back �̵�
        {
            inputX = 0f;
            inputY = 1f;
        }
        else if (velocity.y < 0f && Mathf.Abs(velocity.y) > Mathf.Abs(velocity.x)) // forward �̵�
        {
            inputX = 0f;
            inputY = -1f;
        }

        monsterAnimator.SetFloat("inputX", inputX);
        monsterAnimator.SetFloat("inputY", inputY);
    }

    private void Patrol()
    {
        //if (!agent.enabled) return;

        if (!isPatrol)
        {
            StartCoroutine(PatrolRoutine());
        }

        if (isPlayerDetected && distanceToPlayer <= stopChasingDistance) // ���Ͱ� �÷��̾ �����ϸ� Chase
        {
            currentState = State.Chase;
            StopCoroutine(PatrolRoutine());
            mark.text = "!";
        }
    }

    private void Chase()
    {
        //if (!agent.enabled) return;

        if (!isShooting && !isHit)
        {
            StartCoroutine(Shoot());
        }
        else
        {
            agent.SetDestination(player.position);
        }

        if (distanceToPlayer > stopChasingDistance) // �÷��̾���� �Ÿ��� �־����� Return
        {
            SetPlayerDetected(false);

            mark.text = "?";
            StartCoroutine(StopAndResume(3f));
            currentState = State.Return;
        }
    }

    private void Return()
    {
        //if (!agent.enabled) return;

        agent.SetDestination(initialPosition);

        if (Vector2.Distance(transform.position, initialPosition) < 0.01f) // ���ڸ��� ���ư��� Patrol
        {
            mark.text = "";
            currentState = State.Patrol;
            StartCoroutine(PatrolRoutine());
        }
    }

    private void Death() // ����
    {
        isDie = true;
        agent.enabled = false; // NavMesh ����!
        rigid.velocity = Vector2.zero; // �ӵ� ����!
        monsterAnimator.SetBool("isDeath", true); // Death �ִϸ��̼� ����
        Destroy(gameObject, 3f); // 3�� �� ������Ʈ �Ҵ� ����
        return;
    }

    IEnumerator PatrolRoutine()
    {
        isPatrol = true;

        // �������� �̵�
        agent.SetDestination(new Vector3(initialPosition.x - 2, initialPosition.y, initialPosition.z));
        yield return new WaitUntil(() => !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance);

        StartCoroutine(StopAndResume(1f)); // 1�� ���� ����

        // ���������� �̵�
        agent.SetDestination(new Vector3(initialPosition.x + 2, initialPosition.y, initialPosition.z));
        yield return new WaitUntil(() => !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance);

        StartCoroutine(StopAndResume(1f)); // 1�� ���� ����

        isPatrol = false;
    }

    IEnumerator Shoot() // projectile �߻�
    {
        isShooting = true;

        Vector2 direction = (player.position - pos.position).normalized;
        GameObject newProjectile = Instantiate(projectile, pos.position, Quaternion.identity);
        newProjectile.GetComponent<Rigidbody2D>().velocity = direction * projectileSpeed;
        Debug.Log(rigid.velocity);
        yield return new WaitForSeconds(monsterAttackSpeed);

        isShooting = false;
    }

    IEnumerator StopAndResume(float delay) // delay�� ���� ����� �ٽ� ������
    {
        if (!agent.isStopped)
        {
            agent.isStopped = true;

            yield return new WaitForSeconds(delay);

            agent.isStopped = false;
        }
    }

    // // ���������� blink
    //private IEnumerator BlinkEffect()
    //{
    //    Color originalColor = spriteRenderer.color;
    //    Color blinkColor = Color.red;
    //    float duration = 1.0f;
    //    float blinkInterval = 0.1f;
    //    float elapsedTime = 0f;

    //    while (elapsedTime < duration)
    //    {
    //        spriteRenderer.color = blinkColor;
    //        yield return new WaitForSeconds(blinkInterval);
    //        spriteRenderer.color = originalColor;
    //        yield return new WaitForSeconds(blinkInterval);
    //        elapsedTime += blinkInterval * 2;
    //    }

    //    spriteRenderer.color = originalColor;
    //}

    // ����+�����ϰ� blink
    private IEnumerator BlinkEffect()
    {
        Color originalColor = spriteRenderer.color;
        Color blinkColor = new Color(1f, 0f, 0f, 0.5f); // ������(1, 0, 0)�� ������(���İ� 0.5)
        float duration = 1.0f;
        float blinkInterval = 0.1f;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            // ������ ���������� ����
            spriteRenderer.color = blinkColor;
            yield return new WaitForSeconds(blinkInterval);

            // ���� �������� ����
            spriteRenderer.color = originalColor;
            yield return new WaitForSeconds(blinkInterval);

            elapsedTime += blinkInterval * 2;
        }

        // Blink ȿ���� ���� �� ���� �������� �ǵ�����
        spriteRenderer.color = originalColor;
        isBlink = false;
    }

    private void TakeDamage()
    {
        Hp = Hp - 50; // ������ ����
        if (isBlink) return;
        isBlink = true;
        StartCoroutine(BlinkEffect());
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        //float knockBackForce = 0.5f;
        // Vector2 knockBack = transform.position - collision.transform.position;

        if (collision.CompareTag("Bullet"))
        {
            TakeDamage();

            isHit = true;
            if (currentState == State.Patrol || currentState == State.Return)
            {
                currentState = State.Chase;
                StopCoroutine(PatrolRoutine());
                mark.text = "!";
            }
            StartCoroutine(StopAndResume(1f));

            isHit = false;
            //rigid.AddForce(knockBackForce * knockBack, ForceMode2D.Impulse); // �˹� �� ������ �� ����..
        }
    }
}
