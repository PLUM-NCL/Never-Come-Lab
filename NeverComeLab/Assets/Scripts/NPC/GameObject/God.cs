using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class God : NPC, ITalkable
{
    [SerializeField] private DialogText dialogText;
    [SerializeField] private DialogController dialogController;
    
    public override void Interact()
    {
        if (isTalkable)
        {
            Talk(dialogText);
        }

        if (!DialogController.IsConversation)
        {
            isTalkable = false;
        }
        
    }

    public void Talk(DialogText dialogText)
    {
        dialogController.DisplayNextText(dialogText);
    }
}
