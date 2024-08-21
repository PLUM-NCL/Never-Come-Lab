using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : NPC
{
    [SerializeField] private GameObject clue;

    public override void Interact()
    {
        isTalkable = false;
        gameObject.SetActive(false);
        clue.SetActive(true);
        
    }
}
