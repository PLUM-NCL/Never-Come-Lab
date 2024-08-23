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
        
        // PlayableDirector�� stopped �̺�Ʈ�� �̺�Ʈ �ڵ鷯 ���
        bossTimeline.stopped += TimelineStopped;
    }
    public void StartBossEvent()
    {
        // ���� �ö����
        anim[0].SetBool("isAttack", true);
        anim[1].SetBool("isAttack", true);
        anim[2].SetBool("isAttack", true);
        gasi.SetActive(true);
        // Timeline ���
        bossTimeline.Play();
    }

    void TimelineStopped(PlayableDirector director)
    {
        // Ÿ�Ӷ����� ������ �� BossStageStart�� true�� ����
        if (director == bossTimeline)
        {
            BossStageStart = true;
            Debug.Log("Ÿ�Ӷ��� ��");
        }
    }
}
