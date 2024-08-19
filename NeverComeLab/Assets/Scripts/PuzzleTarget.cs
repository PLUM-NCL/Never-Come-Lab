using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleTarget : MonoBehaviour
{
    private bool isOccupied = false; // 이 목표 위치에 블럭이 있는지 여부를 추적

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Block"))
        {
            isOccupied = true;
            PuzzleManager.Instance.CheckPuzzleCompletion(); // 퍼즐 완료 여부를 확인
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Block"))
        {
            isOccupied = false;
            PuzzleManager.Instance.CheckPuzzleCompletion(); // 퍼즐 완료 여부를 확인
        }
    }

    public bool IsOccupied()
    {
        return isOccupied;
    }
}
