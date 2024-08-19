using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEntry : MonoBehaviour
{
    // 활성화/비활성화할 오브젝트를 참조합니다.
    public GameObject targetObject1;
    public GameObject targetObject2;
    // 트리거에 충돌했을 때 호출됩니다.

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 플레이어 태그를 가진 오브젝트와 충돌했는지 확인합니다.
        if (other.CompareTag("Player"))
        {
            // 오브젝트의 현재 활성화 상태를 반전시킵니다.
            targetObject1.SetActive(!targetObject1.activeSelf);
            targetObject2.SetActive(!targetObject2.activeSelf);
        }
    }
}
