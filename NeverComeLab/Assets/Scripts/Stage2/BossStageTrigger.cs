using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStageTrigger : MonoBehaviour
{
    public BossStage bossStage; // ���� �������� ���� ��ũ��Ʈ ����

    private void OnTriggerEnter2D(Collider2D other)
    {
        // �÷��̾ Ʈ���� ������ ���Դ��� Ȯ��
        if (other.CompareTag("Player"))
        {
            // ���� �̺�Ʈ ����
            bossStage.StartBossEvent();
        }
    }
}
