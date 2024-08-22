using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStageTrigger : MonoBehaviour
{
    public BossStage bossStage; // ���� �������� ���� ��ũ��Ʈ ����
    [SerializeField] private CinemachineVirtualCamera BossCamera;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // �÷��̾ Ʈ���� ������ ���Դ��� Ȯ��
        if (other.CompareTag("Player"))
        {
            BossCamera.Priority = 100;
            // ���� �̺�Ʈ ����
            bossStage.StartBossEvent();
            gameObject.SetActive(false);
            
        }
    }
}
