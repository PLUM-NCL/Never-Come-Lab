using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    public static PuzzleManager Instance; // 싱글톤 패턴으로 퍼즐 매니저를 쉽게 참조할 수 있게 설정
    public PuzzleTarget[] targets; // 각 목표 위치를 관리하는 PuzzleTarget 배열

    private void Awake()
    {
        // 싱글톤 인스턴스 설정
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // 퍼즐 완료 상태를 체크하는 메서드
    public void CheckPuzzleCompletion()
    {
        foreach (var target in targets)
        {
            if (!target.IsOccupied())
            {
                return; // 목표 위치 중 하나라도 블럭이 없으면 퍼즐이 완료되지 않음
            }
        }

        Debug.Log("Puzzle Completed!");
        OnPuzzleCompleted();
    }

    // 퍼즐이 완료되었을 때 실행할 로직
    private void OnPuzzleCompleted()
    {
        Debug.Log("Puzzle Completed! Loading next stage...");
        // 씬 전환 로직을 여기에 추가
    }
}
