using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    public static PuzzleManager Instance; // 싱글톤 패턴으로 퍼즐 매니저를 쉽게 참조할 수 있게 설정

    private PuzzleTarget[] currentTargets; // 현재 스테이지의 목표 타겟 배열

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // 현재 스테이지의 타겟을 설정
    public void SetCurrentTargets(PuzzleTarget[] targets)
    {
        currentTargets = targets;
    }

    // 퍼즐 완료 상태를 체크하는 메서드
    public void CheckPuzzleCompletion()
    {
        foreach (var target in currentTargets)
        {
            if (!target.IsOccupied())
            {
                return; // 목표 위치 중 하나라도 블럭이 없으면 퍼즐이 완료되지 않음
            }
        }

        Debug.Log($"Puzzle for Stage {StageManager.Instance.GetCurrentStage()} Completed!");
        StageManager.Instance.NextStage();
    }
    
}
