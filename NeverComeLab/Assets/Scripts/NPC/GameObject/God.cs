using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class God : NPC, ITalkable
{
    [SerializeField] private DialogText dialogText;
    [SerializeField] private DialogController dialogController;
    public override void Interact()
    {
        Talk(dialogText);
    }

    public void Talk(DialogText dialogText)
    {
        dialogController.DisplayNextText(dialogText);
    }
}
