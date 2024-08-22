using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEntry : MonoBehaviour
{
    // Ȱ��ȭ/��Ȱ��ȭ�� ������Ʈ�� �����մϴ�.
    [SerializeField] private Transform SpawnPoint;
    // Ʈ���ſ� �浹���� �� ȣ��˴ϴ�.

    private void OnTriggerEnter2D(Collider2D other)
    {
        // �÷��̾� �±׸� ���� ������Ʈ�� �浹�ߴ��� Ȯ���մϴ�.
        if (other.CompareTag("Player"))
        {
            other.gameObject.transform.position = SpawnPoint.position;
        }
    }
}
