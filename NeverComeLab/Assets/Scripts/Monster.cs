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
    private float monsterAttackSpeed; // 공속
    private int monsterType; // 몬스터 유형

    private bool isHit = false;
    private bool isShooting = false;
    private bool isDie = false; // 죽었는지 안죽었는지

    private Animator monsterAnimator;
    private AudioSource monsterAudio;

    private float distance; // 플레이어와의 거리
    private bool isAttack = false; // 공격 구분
    private float attackSpeed = 2f; // 공속 (공격 간격)

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
        if (!isDie) // 원거리
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
