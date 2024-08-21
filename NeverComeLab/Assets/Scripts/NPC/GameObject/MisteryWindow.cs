using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MisteryWindow : NPC, ITalkable
{
    [SerializeField] private DialogText dialogText;
    [SerializeField] private DialogController dialogController;
    [SerializeField] private GameObject clue;


    public override void Interact()
    {
        if (isTalkable)
        {
            Talk(dialogText);
        }

        if (!DialogController.IsConversation)
        {
            isTalkable = false;
            clue.SetActive(true);
        }
    }

    public void Talk(DialogText dialogText)
    {
        dialogController.DisplayNextText(dialogText);
    }
}
