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
    private float monsterAttackSpeed; // ����
    private int monsterType; // ���� ����

    private bool isHit = false;
    private bool isShooting = false;
    private bool isDie = false; // �׾����� ���׾�����

    private Animator monsterAnimator;
    private AudioSource monsterAudio;

    private float distance; // �÷��̾���� �Ÿ�
    private bool isAttack = false; // ���� ����
    private float attackSpeed = 2f; // ���� (���� ����)

    public Transform player;
    public float projectileSpeed = 5f;

    private void Start()
    {
        monsterHp = enemyData.monsterHp;
        monsterDamage = enemyData.monsterDamage;
        monsterSpeed = enemyData.monsterSpeed;
        monsterAttackSpeed = enemyData.monsterAttackSpeed;

        monsterAnimator = GetComponent<Animator>();
        monsterAudio = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (!isDie) // ���Ÿ�
        {
            if(!isShooting) StartCoroutine(Shoot());
        }

    }

    IEnumerator Shoot()
    {
        isShooting = true;
        yield return new WaitForSeconds(1f);
        Vector2 direction = (player.position - pos.position).normalized;
        GameObject newProjectile = Instantiate(projectile, pos.position, Quaternion.identity);
        Rigidbody2D rigid = newProjectile.GetComponent<Rigidbody2D>();
        rigid.velocity = direction * projectileSpeed;
        Debug.Log(rigid.velocity);
        isShooting = false;
    }
}
