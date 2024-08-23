using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class Monster : MonoBehaviour
{
    private enum State { Patrol, Chase, Return, Sleep}
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
            if (monsterHp > 100) // Hp는 100 이상 못올라감
            {
                monsterHp = 100;
                Debug.Log("Hp 100 한도 초과");
            }
            else if(monsterHp <= 0) // Hp가 0 이하로 내려가면 죽음
            {
                Death();
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
    private bool isAsleep = false;  // 잠들었는가?

    private Animator monsterAnimator;
    private AudioSource monsterAudio;

    private float distanceToPlayer;
    [SerializeField] private float stopChasingDistance = 7f;

    public Transform player;
    public float projectileSpeed = 5f;

    Rigidbody2D rigid;

    private Coroutine patrolCoroutine;
    private Coroutine stopAndResume;
    private Coroutine shoot;
    private Coroutine sleep;
    private Coroutine blink;

    public delegate void MonsterStateChange(Monster monster);
    public event MonsterStateChange OnDeath;
    public event MonsterStateChange OnSleep;
    public event MonsterStateChange OnWake;

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
                if(isPlayerDetected) currentState= State.Chase;
                Return();
                break;
            case State.Sleep:
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
        mark.text = "";
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
            
        }
    }

    private void Chase()
    {
        if (!agent.enabled) return;
        mark.text = "!";
        if (GameManager.Instance.player.isHide == true)
        {
            Miss();
            return;
        }

        if (!isShooting && !isHit && !GameManager.Instance.player.isDie)
        {
            shoot = StartCoroutine(Shoot());
            
        }
            agent.SetDestination(player.position);
        if (distanceToPlayer > stopChasingDistance) // 플레이어와의 거리가 멀어지면 Return
        {
            Miss();
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
            currentState = State.Patrol;
            patrolCoroutine = StartCoroutine(PatrolRoutine());
        }
    }
    private void StopAllCoroutine()
    {
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
        //if(shoot != null)
        //{
        //    StopCoroutine(shoot);
        //    stopAndResume = null;
        //}
        if(blink != null)
        {
            StopCoroutine(blink);
            stopAndResume = null;
        }
        if (sleep != null)
        {
            StopCoroutine(sleep);
            stopAndResume = null;
        }
        
    }
    private void Death() // 죽음
    {
        if (!agent.enabled) return;

        isDie = true;

        // 순찰 및 기타 코루틴 종료
        StopAllCoroutine();

        // 에이전트 비활성화
        agent.enabled = false;

        // 죽음 애니메이션 시작
        monsterAnimator.SetBool("isDeath", true);

        // 텍스트 표시 (텍스트 갱신을 먼저 처리)
        mark.text = "꾸엑";
        Debug.Log("몬스터 죽음: " + gameObject.name);

        if (isAsleep)// 몬스터가 죽을 때 수면 상태를 먼저 해제합니다.
        {
            isAsleep = false;
            OnWake?.Invoke(this); // 몬스터가 죽으면 수면 상태를 해제하고 깨어남 처리
        }

        // 스테이지 매니저에 죽음을 알림
        OnDeath?.Invoke(this);
        
        // 일정 시간 후 오브젝트 삭제
        Destroy(gameObject, 3f);
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
        if (agent.enabled && !isHit)
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

        if(!isDie)
            agent.isStopped = false;

        WakeUp();
    }

    IEnumerator Sleep(float delay) // delay초 동안 멈췄다 다시 움직임
    {
        if (!agent.enabled || agent.isStopped) yield break;
        mark.text = "Zzz";
        StopAllCoroutine();
        agent.isStopped = true;
        yield return new WaitForSeconds(delay);
        isHit = false;
        agent.isStopped = false;
        if (distanceToPlayer > stopChasingDistance) // 플레이어와의 거리가 멀어지면 Return
        {
            Miss();
        }
        else
        {
            currentState = State.Chase;
        }

        WakeUp();
    }
    
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

        if(blink == null)
            blink = StartCoroutine(BlinkEffect());
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
        currentState = State.Sleep;
        if (isDie || isAsleep) return;   // 이미 죽었거나 잠들어 있으면 아무 작업도 하지 않음

        isAsleep = true;

        OnSleep?.Invoke(this); // 스테이지 매니저에 잠들었음을 알림

        isHit = true;
        
        sleep = StartCoroutine(Sleep(10f));
    }

    public void WakeUp()
    {
        if (isDie || !isAsleep) return; // 죽었거나 잠들어 있지 않으면 아무 작업도 하지 않음
        isAsleep = false;
        isHit = false;
        OnWake?.Invoke(this); // 스테이지 매니저에 깨어났음을 알림
    }

    public bool IsAsleep()
    {
        return isAsleep;
    }


}
