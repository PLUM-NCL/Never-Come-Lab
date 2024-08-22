using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class DoorInteraction : MonoBehaviour
{
    private bool isPlayerInRange = false;
    private StageManager stageManager;

    // StageManager를 DoorInteraction에 전달하는 메서드
    public void SetStageManager(StageManager manager)
    {
        stageManager = manager;
    }

    void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.F))
        {
            stageManager.OnDoorInteracted(); // 문과 상호작용 시 StageManager에 알림
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            Debug.Log("Player in range of the door.");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            Debug.Log("Player left the range of the door.");
        }
    }
    /*
    private bool isPlayerInRange = false;

    void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.F))
        {
            StageManager.Instance.OnDoorInteracted(); // 문과 상호작용 시 StageManager에 알림
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            Debug.Log("Player in range of the door.");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            Debug.Log("Player left the range of the door.");
        }
    }*/
}