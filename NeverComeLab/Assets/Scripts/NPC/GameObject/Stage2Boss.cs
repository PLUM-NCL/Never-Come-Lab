using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage2Boss : MonoBehaviour, ITalkable
{
    [SerializeField] private DialogText dialogText;
    [SerializeField] private DialogController dialogController;
    [SerializeField] private CinemachineVirtualCamera BossCamera;
    [SerializeField] private GameObject Demo;

    [SerializeField] private BossStage bossStage;
    private bool isFirst = true;

    void Update()
    {
        
        if (isFirst && bossStage.BossStageStart)
        {
            
            Talk(dialogText);
            isFirst = false;
        }

        if(Input.GetKeyDown(KeyCode.Space) && bossStage.BossStageStart)
        {
            Talk(dialogText);
        }

        
    }

    public void Talk(DialogText dialogText)
    {
        dialogController.DisplayNextText(dialogText);
        if (!DialogController.IsConversation)
        {
            BossCamera.Priority = 0;
            Demo.SetActive(true);
        }
    }
}
