using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/MonsterData", fileName = "MonsterData")]
public class MonsterData : ScriptableObject
{
    public float monsterHp; // ü��
    public float monsterDamage; // ���ݷ�
    public float monsterSpeed; // �̼�
    public float monsterAttackSpeed; // ����

    public AudioClip monsterDieClip; // �״� �Ҹ�
    public AudioClip monsterAttackClip; // ���� �Ҹ�
    public AudioClip monsterHitClip; // �ǰ� �Ҹ�
}
