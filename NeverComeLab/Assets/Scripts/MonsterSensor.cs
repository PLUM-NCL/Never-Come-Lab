using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSensor : MonoBehaviour
{
    private Monster monster;

    void Start()
    {
        // �θ� ������Ʈ���� Monster ��ũ��Ʈ�� ������
        monster = GetComponentInParent<Monster>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            monster.SetPlayerDetected(true);
        }
    }
}