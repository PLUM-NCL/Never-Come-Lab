using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageManager : MonoBehaviour
{
    public static StageManager Instance; // 싱글톤 패턴으로 StageManager를 쉽게 참조할 수 있게 설정

    [System.Serializable]
    public class Stage
    {
        public string stageName;
        public GameObject[] tilemaps;
        public GameObject[] enemies;
        public PuzzleTarget[] targets;
        public GameObject[] blocks;
        public GameObject door; // 문 오브젝트
        public string nextSceneName; // 다음 씬의 이름 (선택 사항, 필요시 씬 전환)
    }

    [SerializeField] private Stage[] stages; // 스테이지 배열

    private int currentStage = 0;

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

        foreach (var stage in stages)
        {
            foreach (var tilemap in stage.tilemaps)
            {
                tilemap.gameObject.SetActive(false);
            }

            foreach (var block in stage.blocks)
            {
                block.gameObject.SetActive(false);
            }

            foreach (var target in stage.targets)
            {
                target.gameObject.SetActive(false);
            }

            foreach (var enemy in stage.enemies)
            {
                if (enemy != null && enemy.gameObject != null)
                {
                    enemy.gameObject.SetActive(false);
                }
            }

            if (stage.door != null)
            {
                stage.door.gameObject.SetActive(false);
            }
        }

        // 현재 스테이지의 타일맵, 블럭, 타겟, 몬스터를 활성화
        foreach (var tilemap in stages[stageIndex].tilemaps)
        {
            tilemap.gameObject.SetActive(true);
        }

        foreach (var block in stages[stageIndex].blocks)
        {
            block.gameObject.SetActive(true);
        }

        foreach (var target in stages[stageIndex].targets)
        {
            target.gameObject.SetActive(true);
        }

        foreach (var enemy in stages[stageIndex].enemies)
        {
            enemy.gameObject.SetActive(true);
        }

        // PuzzleManager에서 현재 스테이지의 목표 타겟 설정
        PuzzleManager.Instance?.SetCurrentTargets(stages[stageIndex].targets);

        if (stages[stageIndex].door != null)
        {
            stages[stageIndex].door.SetActive(false); // 초기에는 문을 비활성화
        }
    }

    // 몬스터가 처치되었을 때 호출
    public void OnEnemyDefeated(GameObject enemy)
    {
        enemy.SetActive(false);
        CheckStageCompletion();
    }

    // 모든 몬스터가 처치되었는지 체크
    public void CheckStageCompletion()
    {
        bool allEnemiesDefeated = true;
        foreach (var enemy in stages[currentStage].enemies)
        {
            if (enemy != null && enemy.activeInHierarchy)
            {
                allEnemiesDefeated = false;
                break;
            }
        }

        if (allEnemiesDefeated)
        {
            OnStageCompleted();
        }
    }

    // 모든 몬스터가 수면 상태인지 체크
    public void CheckSleepStatus()
    {
        bool allEnemiesAsleep = true;
        foreach (var enemy in stages[currentStage].enemies)
        {
            if (enemy != null && enemy.activeInHierarchy)
            {
                Monster monster = enemy.GetComponent<Monster>();
                if (monster != null && !monster.IsAsleep())
                {
                    allEnemiesAsleep = false;
                    break;
                }
            }
        }

        if (allEnemiesAsleep)
        {
            ActivateDoorTemporarily();
        }
    }

    // 문을 일시적으로 활성화
    private void ActivateDoorTemporarily()
    {
        if (stages[currentStage].door != null)
        {
            stages[currentStage].door.SetActive(true);
            StartCoroutine(DeactivateDoorAfterTime(10f)); // 10초 후 문 비활성화
        }
    }

    private IEnumerator DeactivateDoorAfterTime(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (stages[currentStage].door != null)
        {
            stages[currentStage].door.SetActive(false);
        }
    }
    public void OnPuzzleCompleted()
    {
        if (stages[currentStage].door != null)
        {
            stages[currentStage].door.SetActive(true);  // 퍼즐 완성 시 문 활성화
        }
    }

    public void OnPuzzleIncomplete()
    {
        if (stages[currentStage].door != null)
        {
            stages[currentStage].door.SetActive(false);  // 퍼즐이 망가지면 문 비활성화
        }
    }
    
    public void OnStageCompleted()
    {
        if (stages[currentStage].door != null)
        {
            stages[currentStage].door.SetActive(true); // 스테이지 완료 시 문을 활성화
        }
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