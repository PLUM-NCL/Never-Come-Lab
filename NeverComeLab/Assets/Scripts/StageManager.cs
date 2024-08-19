using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public static StageManager Instance; // 싱글톤 패턴으로 StageManager를 쉽게 참조할 수 있게 설정

    [System.Serializable]
    public class Stage
    {
        public string stageName; // 스테이지 이름 (선택 사항)
        public GameObject[] tilemaps; // 스테이지별로 활성화할 타일맵
        public PuzzleTarget[] targets;  // 각 스테이지의 목표 타겟 배열
        public GameObject[] blocks;     // 각 스테이지의 블럭 배열
    }

    [SerializeField] private Stage[] stages; // 스테이지 배열

    private int currentStage = 0;

    private void Awake()
    {
        // 싱글톤 인스턴스 설정
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject); // 이미 인스턴스가 존재하면 중복된 객체를 파괴
        }
    }

    private void Start()
    {
        InitializeStage(currentStage);
    }

    // 스테이지 초기화: 타일맵, 퍼즐 구성요소 및 타겟 초기화
    public void InitializeStage(int stageIndex)
    {
        if (stageIndex < 0 || stageIndex >= stages.Length)
        {
            Debug.LogError("Invalid stage index");
            return;
        }

        // 모든 스테이지의 타일맵과 블럭, 타겟을 비활성화
        foreach (var stage in stages)
        {
            foreach (var tilemap in stage.tilemaps)
            {
                tilemap.SetActive(false);
            }

            foreach (var block in stage.blocks)
            {
                block.SetActive(false);
            }

            foreach (var target in stage.targets)
            {
                target.gameObject.SetActive(false);
            }
        }

        // 현재 스테이지의 타일맵과 블럭, 타겟을 활성화
        foreach (var tilemap in stages[stageIndex].tilemaps)
        {
            tilemap.SetActive(true);
        }

        foreach (var block in stages[stageIndex].blocks)
        {
            block.SetActive(true);
        }

        foreach (var target in stages[stageIndex].targets)
        {
            target.gameObject.SetActive(true);
        }

        // PuzzleManager에서 현재 스테이지의 목표 타겟 설정
        PuzzleManager.Instance.SetCurrentTargets(stages[stageIndex].targets);
    }

    // 다음 스테이지로 이동
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
            // 게임 완료 또는 다음 씬으로 전환
        }
    }

    // 현재 스테이지의 인덱스를 반환
    public int GetCurrentStage()
    {
        return currentStage;
    }
}
