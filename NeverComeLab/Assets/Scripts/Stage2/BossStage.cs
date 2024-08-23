using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class BossStage : MonoBehaviour
{
    public PlayableDirector bossTimeline;
    public bool BossStageStart = false;
    [SerializeField] Animator[] anim;
    [SerializeField] GameObject gasi;

    void Start()
    {
        
        // PlayableDirector의 stopped 이벤트에 이벤트 핸들러 등록
        bossTimeline.stopped += TimelineStopped;
    }
    public void StartBossEvent()
    {
        // 가시 올라오고
        anim[0].SetBool("isAttack", true);
        anim[1].SetBool("isAttack", true);
        anim[2].SetBool("isAttack", true);
        gasi.SetActive(true);
        // Timeline 재생
        bossTimeline.Play();
    }

    void TimelineStopped(PlayableDirector director)
    {
        // 타임라인이 끝났을 때 BossStageStart를 true로 변경
        if (director == bossTimeline)
        {
            BossStageStart = true;
            Debug.Log("타임라인 끝");
        }
    }
}
