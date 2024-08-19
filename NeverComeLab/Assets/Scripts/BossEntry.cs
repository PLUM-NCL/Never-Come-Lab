using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEntry : MonoBehaviour
{
    // Ȱ��ȭ/��Ȱ��ȭ�� ������Ʈ�� �����մϴ�.
    public GameObject targetObject1;
    public GameObject targetObject2;
    // Ʈ���ſ� �浹���� �� ȣ��˴ϴ�.

    private void OnTriggerEnter2D(Collider2D other)
    {
        // �÷��̾� �±׸� ���� ������Ʈ�� �浹�ߴ��� Ȯ���մϴ�.
        if (other.CompareTag("Player"))
        {
            // ������Ʈ�� ���� Ȱ��ȭ ���¸� ������ŵ�ϴ�.
            targetObject1.SetActive(!targetObject1.activeSelf);
            targetObject2.SetActive(!targetObject2.activeSelf);
        }
    }
}
