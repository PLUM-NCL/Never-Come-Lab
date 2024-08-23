using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class DialogCall : MonoBehaviour, ITalkable
{
    public DialogController dialogController;
    public DialogText[] dialog;
    public StageManager stageManager;
    private bool isTalkable = true;
    private bool isFirst = true;
    private void Update()
    {
        if (stageManager.isStageChange == true)
        {
            isTalkable = true;
            stageManager.isStageChange = false;
        }

        if (stageManager.currentStage == 0 && isTalkable)
        {
            TutorialConversation();
        }
        
        if (stageManager.currentStage == 1 && isTalkable)
        {
            TutorialConversation();
        }

        if (stageManager.currentStage == 2 && isTalkable)
        {
            TutorialConversation();
        }

        if (stageManager.currentStage == 3 && isTalkable)
        {
            TutorialConversation();
        }
    }
    public void Talk(DialogText dialog)
    {
        dialogController.DisplayNextText(dialog);
    }

    public void TutorialConversation()
    {
            if (Input.GetKeyDown(KeyCode.Space) || isFirst)
            {
                Talk(dialog[stageManager.currentStage]);
                Player.isStop = true;
                isFirst = false;
            }
            if (!DialogController.IsConversation)
            {
                Player.isStop = false;
                isFirst = true;
                isTalkable = false;
            }
    }
}
