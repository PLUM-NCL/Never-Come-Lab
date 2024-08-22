using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleTarget : MonoBehaviour
{
    private bool isOccupied;
    private PuzzleManager puzzleManager;

    public void SetPuzzleManager(PuzzleManager manager)
    {
        puzzleManager = manager;
    }

    public bool IsOccupied()
    {
        return isOccupied;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Block"))
        {
            isOccupied = true;
            puzzleManager.CheckPuzzleCompletion();  // 타겟이 블록으로 채워질 때 알림
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Block"))
        {
            isOccupied = false;
            puzzleManager.CheckPuzzleCompletion();  // 타겟에서 블록이 빠져나갈 때 알림
        }
    }
    
    /*
    private bool isOccupied; // 이 목표 위치에 블럭이 있는지 여부를 추적

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Block"))
        {
            Debug.Log("Block entered the target area.");
            isOccupied = true;
            PuzzleManager.Instance.CheckPuzzleCompletion(); // 퍼즐 완료 여부를 확인
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Block"))
        {
            Debug.Log("Block exited the target area.");
            isOccupied = false;
            PuzzleManager.Instance.CheckPuzzleCompletion(); // 퍼즐 완료 여부를 확인
        }
    }

    // 이 목표 위치에 블럭이 있는지 여부를 반환
    public bool IsOccupied()
    {
        return isOccupied;
    }*/
}
