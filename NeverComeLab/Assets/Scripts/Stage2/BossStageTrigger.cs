using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStageTrigger : MonoBehaviour
{
    public BossStage bossStage; // 보스 스테이지 관리 스크립트 참조
    [SerializeField] private CinemachineVirtualCamera BossCamera;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 플레이어가 트리거 영역에 들어왔는지 확인
        if (other.CompareTag("Player"))
        {
            BossCamera.Priority = 100;
            // 보스 이벤트 시작
            bossStage.StartBossEvent();
            gameObject.SetActive(false);
            
        }
    }
}
