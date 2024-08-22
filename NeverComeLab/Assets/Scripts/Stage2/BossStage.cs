using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class BossStage : MonoBehaviour
{
    public PlayableDirector bossTimeline;

    public void StartBossEvent()
    {
        // Timeline을 재생하여 이벤트 시작
        bossTimeline.Play();
    }
}
