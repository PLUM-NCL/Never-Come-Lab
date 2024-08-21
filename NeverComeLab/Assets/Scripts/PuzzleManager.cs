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
        CheckPuzzleCompletion();  // 새로운 타겟이 설정될 때 퍼즐 상태 확인
    }

    public void CheckPuzzleCompletion()
    {
        bool allTargetsFilled = true;

        foreach (var target in currentTargets)
        {
            if (!target.IsOccupied())
            {
                allTargetsFilled = false;
                break;
            }
        }

        if (allTargetsFilled)
        {
            Debug.Log("Puzzle Completed!");
            StageManager.Instance.OnPuzzleCompleted();  // 퍼즐이 완성되었을 때 StageManager에 알림
        }
        else
        {
            Debug.Log("Puzzle Incomplete");
            StageManager.Instance.OnPuzzleIncomplete();  // 퍼즐이 망가졌을 때 StageManager에 알림
        }
    }
}
