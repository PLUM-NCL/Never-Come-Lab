using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSensor : MonoBehaviour
{
    private Monster monster;

    void Start()
    {
        // 부모 오브젝트에서 Monster 스크립트를 가져옴
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