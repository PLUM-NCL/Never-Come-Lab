using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class PrologueController : MonoBehaviour
{
    public PlayableDirector playableDirector;

    void Start()
    {
        playableDirector.stopped += OnPlayableDirectorStopped;
    }

    void OnPlayableDirectorStopped(PlayableDirector director)
    {
        // 타임라인이 끝난 후 실행할 로직을 여기에 작성합니다.
        Debug.Log("타임라인이 끝났습니다!");
        // 예: 다음 레벨로 이동, 게임 오브젝트 활성화 등
        ExecuteGameLogic();
    }

    void ExecuteGameLogic()
    {
        // 여기에 게임 로직을 작성합니다.
        Debug.Log("게임 로직 실행 중...");
    }

    void OnDestroy()
    {
        // 이벤트 핸들러를 제거합니다.
        playableDirector.stopped -= OnPlayableDirectorStopped;
    }
}
