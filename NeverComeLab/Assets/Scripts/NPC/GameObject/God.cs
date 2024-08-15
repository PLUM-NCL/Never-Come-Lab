using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class God : NPC, ITalkable
{
    [SerializeField] private DialogText dialogText;
    [SerializeField] private DialogController dialogController;

    [SerializeField] GameObject timeLoop;
    
    public override void Interact()
    {
        if (isTalkable)
        {
            Talk(dialogText);
        }

        if (!DialogController.IsConversation)
        {
            isTalkable = false;
            timeLoop.SetActive(true);
        }
    }

    public void Talk(DialogText dialogText)
    {
        dialogController.DisplayNextText(dialogText);
    }
}
