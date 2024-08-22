using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;


public class StageManager : MonoBehaviour
{
    [System.Serializable]
    public class Stage
    {
        public string stageName;
        public GameObject[] tilemaps;
        public GameObject[] enemies;
        public PuzzleTarget[] targets;
        public GameObject[] blocks;
        public GameObject door;
        public string nextSceneName; // 다음 씬의 이름 (선택 사항, 필요시 씬 전환)
    }

    [SerializeField] private Stage[] stages;
    private int currentStage = 0;
    private int remainingEnemies;
    private int asleepEnemies;

    private void Start()
    {
        InitializeStage(currentStage);
    }

    public void InitializeStage(int stageIndex)
    {
        if (stageIndex < 0 || stageIndex >= stages.Length)
        {
            Debug.LogError("Invalid stage index");
            return;
        }

        // 이전 스테이지 비활성화
        foreach (var stage in stages)
        {
            SetStageActive(stage, false);
        }

        // 현재 스테이지 활성화
        SetStageActive(stages[stageIndex], true);

        // 남은 적 초기화
        remainingEnemies = 0;
        asleepEnemies = 0;

        foreach (var enemy in stages[stageIndex].enemies)
        {
            if (enemy != null)
            {
                Monster monster = enemy.GetComponent<Monster>();
                if (monster != null)
                {
                    remainingEnemies++;
                    monster.OnDeath += HandleMonsterDeath;
                    monster.OnSleep += HandleMonsterSleep;
                    monster.OnWake += HandleMonsterWake;  // 몬스터가 깨어났을 때 처리
                }
            }
        }

        // PuzzleManager에 현재 스테이지의 타겟 설정
        PuzzleManager puzzleManager = FindObjectOfType<PuzzleManager>();
        if (puzzleManager != null)
        {
            puzzleManager.SetCurrentTargets(stages[stageIndex].targets, this); // StageManager 참조 전달
        }

        // 문 비활성화 및 StageManager 할당
        if (stages[stageIndex].door != null)
        {
            stages[stageIndex].door.SetActive(false);

            // DoorInteraction 컴포넌트에 StageManager 할당
            DoorInteraction doorInteraction = stages[stageIndex].door.GetComponent<DoorInteraction>();
            if (doorInteraction != null)
            {
                doorInteraction.SetStageManager(this); // StageManager를 DoorInteraction에 할당
            }
        }
    }

    private void SetStageActive(Stage stage, bool isActive)
    {
        foreach (var tilemap in stage.tilemaps)
        {
            if (tilemap != null)
                tilemap.SetActive(isActive);
        }

        foreach (var block in stage.blocks)
        {
            if (block != null)
                block.SetActive(isActive);
        }

        foreach (var target in stage.targets)
        {
            if (target != null)
                target.gameObject.SetActive(isActive);
        }

        foreach (var enemy in stage.enemies)
        {
            if (enemy != null)
                enemy.SetActive(isActive);
        }

        if (stage.door != null)
            stage.door.SetActive(isActive);
    }

    // 몬스터가 죽었을 때 호출되는 메서드
    private void HandleMonsterDeath(Monster monster)
    {
        remainingEnemies--;
        CheckDoorStatus();
    }

    // 몬스터가 잠들었을 때 호출되는 메서드
    private void HandleMonsterSleep(Monster monster)
    {
        asleepEnemies++;
        CheckDoorStatus();
    }

    // 몬스터가 깨어났을 때 호출되는 메서드
    private void HandleMonsterWake(Monster monster)
    {
        asleepEnemies--;
        CheckDoorStatus();
    }

    // 문 상태를 체크하고 활성화 또는 비활성화
    private void CheckDoorStatus()
    {
        // 문을 활성화할 조건
        if (asleepEnemies == remainingEnemies || remainingEnemies == 0)
        {
            ActivateDoor();
        }
        else
        {
            DeactivateDoor();
        }
    }

    private void ActivateDoor()
    {
        if (stages[currentStage].door != null)
        {
            stages[currentStage].door.SetActive(true);
        }
    }

    private void DeactivateDoor()
    {
        if (stages[currentStage].door != null)
        {
            stages[currentStage].door.SetActive(false);
        }
    }

    public void OnPuzzleCompleted()
    {
        ActivateDoor();  // 퍼즐 완성 시 문 활성화
    }

    public void OnPuzzleIncomplete()
    {
        DeactivateDoor();  // 퍼즐이 망가지면 문 비활성화
    }

    public void OnStageCompleted()
    {
        ActivateDoor(); // 스테이지 완료 시 문을 활성화
    }

    public void OnDoorInteracted()
    {
        if (!string.IsNullOrEmpty(stages[currentStage].nextSceneName))
        {
            SceneManager.LoadScene(stages[currentStage].nextSceneName); // 씬 전환
        }
        else
        {
            NextStage(); // 다음 스테이지로 이동
        }
    }

    public void NextStage()
    {
        currentStage++;
        if (currentStage < stages.Length)
        {
            InitializeStage(currentStage);
        }
        else
        {
            Debug.Log("All stages completed!");
            // 모든 스테이지가 완료되었을 때 추가로 처리할 로직
        }
    }

    public int GetCurrentStage()
    {
        return currentStage;
    }
}