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
        // Ÿ�Ӷ����� ���� �� ������ ������ ���⿡ �ۼ��մϴ�.
        Debug.Log("Ÿ�Ӷ����� �������ϴ�!");
        // ��: ���� ������ �̵�, ���� ������Ʈ Ȱ��ȭ ��
        ExecuteGameLogic();
    }

    void ExecuteGameLogic()
    {
        // ���⿡ ���� ������ �ۼ��մϴ�.
        Debug.Log("���� ���� ���� ��...");
    }

    void OnDestroy()
    {
        // �̺�Ʈ �ڵ鷯�� �����մϴ�.
        playableDirector.stopped -= OnPlayableDirectorStopped;
    }
}
