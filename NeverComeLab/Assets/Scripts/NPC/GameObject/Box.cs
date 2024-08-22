using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : NPC
{
    [SerializeField] private GameObject clue;
    [SerializeField] private Monster[] monster;
    
    public override void Interact()
    {
        isTalkable = false;
        gameObject.SetActive(false);
        clue.SetActive(true);
        
        foreach(var monster in monster)
        {
            monster.gameObject.SetActive(true);
            monster.SetPlayerDetected(true);
        }
    }
}
