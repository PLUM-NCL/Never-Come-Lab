using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/MonsterData", fileName = "MonsterData")]
public class MonsterData : ScriptableObject
{
    public float monsterHp; // 체력
    public float monsterDamage; // 공격력
    public float monsterSpeed; // 이속
    public float monsterAttackSpeed; // 공속

    public AudioClip monsterDieClip; // 죽는 소리
    public AudioClip monsterAttackClip; // 공격 소리
    public AudioClip monsterHitClip; // 피격 소리
}
