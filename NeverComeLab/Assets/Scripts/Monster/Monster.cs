using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class Monster : MonoBehaviour
{
    private enum State { Patrol, Chase, Return }
    private State currentState = State.Patrol;

    private Vector3 initialPosition;
    public NavMeshAgent agent;

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
            if (monsterHp > 10000000) // Hp는 100 이상 못올라감
            {
                monsterHp = 100;
                Debug.Log("Hp 100 한도 초과");
            }
            else if(monsterHp <= 0) // Hp가 0 이하로 내려가면 죽음
            {
                Death();
                mark.text = "꾸웱";
                Debug.Log("꾸웱");
            }
            
        }

    }
    private float monsterDamage;
    private float monsterAttackSpeed;
    private int monsterType;

    private bool isHit = false; // 공격받음?
    private bool isShooting = false; // 발사 중?
    private bool isDie = false; // 죽음?
    private bool isPlayerDetected = false; // 플레이어 발견?
    private bool isPatrol = false; // 순찰 중?
    private bool isBlink = false; // 무적?

    private Animator monsterAnimator;
    private AudioSource monsterAudio;

    private float distanceToPlayer;
    private float stopChasingDistance = 5f;

    public Transform player;
    public float projectileSpeed = 5f;

    Rigidbody2D rigid;

    private Coroutine patrolCoroutine;
    private Coroutine stopAndResume;

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

        spriteRenderer = GetComponent<SpriteRenderer>();
        monsterAnimator = GetComponent<Animator>();
        monsterAudio = GetComponent<AudioSource>();
        rigid = GetComponent<Rigidbody2D>();

        Patrol();
    }

    private void Update()
    {
        if (isDie) 
            return; // 죽으면 동작 안하도록

        AnimationSet(); // 상하좌우 애니메이션

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

    private void AnimationSet() // 몬스터 상하좌우 애니메이션 적용
    {
        Vector3 velocity = agent.velocity;
        float inputX = 0f;
        float inputY = 0f;

        if (velocity.x > 0f && Mathf.Abs(velocity.y) < Mathf.Abs(velocity.x)) // right 이동
        {
            inputX = 1f;
            inputY = 0f;
        }
        else if (velocity.x < 0f && Mathf.Abs(velocity.y) < Mathf.Abs(velocity.x)) // left 이동
        {
            inputX = -1f;
            inputY = 0f;
        }
        else if (velocity.y > 0f && Mathf.Abs(velocity.y) > Mathf.Abs(velocity.x)) // back 이동
        {
            inputX = 0f;
            inputY = 1f;
        }
        else if (velocity.y < 0f && Mathf.Abs(velocity.y) > Mathf.Abs(velocity.x)) // forward 이동
        {
            inputX = 0f;
            inputY = -1f;
        }

        monsterAnimator.SetFloat("inputX", inputX);
        monsterAnimator.SetFloat("inputY", inputY);
    }

    private void Patrol()
    {
        if (!agent.enabled) return;

        if (!isPatrol)
        {
            patrolCoroutine = StartCoroutine(PatrolRoutine());
        }

        if (isPlayerDetected && !GameManager.Instance.player.isHide) // 몬스터가 플레이어를 감지하면 Chase
        {
            
            if (patrolCoroutine != null)
            {
                StopCoroutine(patrolCoroutine);
                patrolCoroutine = null;
            }
            currentState = State.Chase;
            mark.text = "!";
        }
    }

    private void Chase()
    {
        if (!agent.enabled) return;

        if (GameManager.Instance.player.isHide == true)
        {
            Miss();
            return;
        }

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
            Miss();
            //SetPlayerDetected(false);

            //mark.text = "?";
            //stopAndResume = StartCoroutine(StopAndResume(3f));
            //currentState = State.Return;
        }

        
    }

    private void Miss()
    {
        SetPlayerDetected(false);

        mark.text = "?";
        stopAndResume = StartCoroutine(StopAndResume(3f));
        currentState = State.Return;
    }

    private void Return()
    {
        if (!agent.enabled) return;

        agent.SetDestination(initialPosition);

        if (Vector2.Distance(transform.position, initialPosition) < 0.01f) // 제자리로 돌아가면 Patrol
        {
            mark.text = "";
            currentState = State.Patrol;
            patrolCoroutine = StartCoroutine(PatrolRoutine());
        }
    }

    private void Death() // 죽음
    {
        if (!agent.enabled) return;

        isDie = true;
        if (patrolCoroutine != null)
        {
            StopCoroutine(patrolCoroutine);
            patrolCoroutine = null;
        }
        if (stopAndResume != null)
        {
            StopCoroutine(stopAndResume);
            stopAndResume = null;
        }
        agent.enabled = false; // 에이전트 비활성화

        //rigid.velocity = Vector2.zero; // 속도 멈춰

        monsterAnimator.SetBool("isDeath", true); // Death 애니메이션 가동

        Destroy(gameObject, 3f); // 3초 후 오브젝트 할당 해제
    }

    IEnumerator PatrolRoutine()
    {
        if (!agent.enabled) yield break;
        
        isPatrol = true;

        // 왼쪽으로 이동
        agent.SetDestination(new Vector3(initialPosition.x - 2, initialPosition.y, initialPosition.z));
        yield return new WaitUntil(() => !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance);

        stopAndResume = StartCoroutine(StopAndResume(1f)); // 1초 동안 멈춤

        // 오른쪽으로 이동
        agent.SetDestination(new Vector3(initialPosition.x + 2, initialPosition.y, initialPosition.z));
        yield return new WaitUntil(() => !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance);

        stopAndResume = StartCoroutine(StopAndResume(1f)); // 1초 동안 멈춤

        isPatrol = false;

    }

    IEnumerator Shoot() // projectile 발사
    {
        if (agent.enabled)
        {
            isShooting = true;

            Vector2 direction = (player.position - pos.position).normalized;
            GameObject newProjectile = Instantiate(projectile, pos.position, Quaternion.identity);
            newProjectile.GetComponent<Rigidbody2D>().velocity = direction * projectileSpeed;
            AudioManager.instance.PlaySfx(AudioManager.Sfx.MonsterBullet);
            Debug.Log(rigid.velocity);
            yield return new WaitForSeconds(monsterAttackSpeed);

            isShooting = false;
        }
    }

    IEnumerator StopAndResume(float delay) // delay초 동안 멈췄다 다시 움직임
    {
        if (!agent.enabled || agent.isStopped) yield break;

        agent.isStopped = true;

        yield return new WaitForSeconds(delay);

        agent.isStopped = false;

    }

    // // 빨간색으로 blink
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

    // 투명+빨강하게 blink
    private IEnumerator BlinkEffect()
    {
        if (!agent.enabled) yield break;

        Color originalColor = spriteRenderer.color;
        Color blinkColor = new Color(1f, 0f, 0f, 0.5f); // 빨간색(1, 0, 0)과 반투명(알파값 0.5)
        float duration = 1.0f;
        float blinkInterval = 0.1f;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            // 빨간색 반투명으로 변경
            spriteRenderer.color = blinkColor;
            yield return new WaitForSeconds(blinkInterval); //여기

            // 원래 색상으로 변경
            spriteRenderer.color = originalColor;
            yield return new WaitForSeconds(blinkInterval);

            elapsedTime += blinkInterval * 2;
        }

        // Blink 효과가 끝난 후 원래 색상으로 되돌리기
        spriteRenderer.color = originalColor;
        isBlink = false;
    }

    public void TakeDamage()
    {
        Hp -= 10; // 데미지 입음
        Debug.Log("남은 몬스터 체력: " + Hp);
       
        if (isBlink) return;
        isBlink = true;
        StartCoroutine(BlinkEffect());
        isHit = true;
        if (currentState == State.Return)
        {
            currentState = State.Chase;
            if (patrolCoroutine != null)
            {
                StopCoroutine(PatrolRoutine());
                patrolCoroutine = null;
            }
            mark.text = "!";
        }
        stopAndResume = StartCoroutine(StopAndResume(1f));
        AudioManager.instance.PlaySfx(AudioManager.Sfx.MonsterDamage);
        isHit = false;
    }

    public void TakeSleep()
    {
        if (isBlink) return;
        isBlink = true;
        StartCoroutine(BlinkEffect());
        isHit = true;
        if (currentState == State.Return)
        {
            currentState = State.Chase;
            if (patrolCoroutine != null)
            {
                StopCoroutine(PatrolRoutine());
                patrolCoroutine = null;
            }
            mark.text = "!";
        }
        stopAndResume = StartCoroutine(StopAndResume(3f));

        isHit = false;
    }

}
