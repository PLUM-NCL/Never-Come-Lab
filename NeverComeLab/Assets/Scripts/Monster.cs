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

    private float monsterHp; // ü��
    private float monsterDamage; // ���ݷ�
    private float monsterSpeed; // �̼�
    private float monsterAttackSpeed; // ���� (���� ���� ����) ������ �ð�
    private int monsterType; // ���� ����

    private bool isHit = false;
    private bool isShooting = false;
    private bool isDie = false; // �׾����� ���׾�����
    private bool isPlayerDetected = false;

    private Animator monsterAnimator;
    private AudioSource monsterAudio;

    private float distanceToPlayer; // �÷��̾���� �Ÿ�
    private bool isAttack = false; // ���� ����
    private float attackSpeed = 1f; // ���� (���� ����)
    private float stopChasingDistance = 5f; // ������ ���� �Ÿ�

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
                // �÷��̾�� ���� ������ �Ÿ� ���
                distanceToPlayer = Vector2.Distance(transform.position, player.position);
                Debug.Log(distanceToPlayer + "m");

                if (distanceToPlayer > stopChasingDistance)
                {
                    // �÷��̾���� �Ÿ��� stopChasingDistance���� ũ�� ���� ����
                    SetPlayerDetected(false);
                    rigid.velocity = Vector3.zero;
                }
                else
                {
                    // �÷��̾� �������� ���� �̵�
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
}
