using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEntry : MonoBehaviour
{
    // 활성화/비활성화할 오브젝트를 참조합니다.
    [SerializeField] private Transform SpawnPoint;
    // 트리거에 충돌했을 때 호출됩니다.

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 플레이어 태그를 가진 오브젝트와 충돌했는지 확인합니다.
        if (other.CompareTag("Player"))
        {
            other.gameObject.transform.position = SpawnPoint.position;
        }
    }
}
