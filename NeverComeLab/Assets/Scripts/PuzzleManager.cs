using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    public static PuzzleManager Instance;

    private PuzzleTarget[] currentTargets;

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

    public void SetCurrentTargets(PuzzleTarget[] targets)
    {
        currentTargets = targets;
    }

    public void CheckPuzzleCompletion()
    {
        foreach (var target in currentTargets)
        {
            if (!target.IsOccupied())
            {
                return;
            }
        }

        Debug.Log($"Puzzle for Stage {StageManager.Instance.GetCurrentStage()} Completed!");
        StageManager.Instance.OnStageCompleted(); // 문 활성화 및 스테이지 완료 처리
    }
}
