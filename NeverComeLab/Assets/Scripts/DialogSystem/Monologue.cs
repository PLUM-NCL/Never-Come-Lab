using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monologue : MonoBehaviour, ITalkable
{
    [SerializeField] private DialogController dialogController;
    [SerializeField] private DialogText dialogText;
    public void Talk(DialogText dialogText)
    {
        dialogController.DisplayNextText(dialogText);
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            if (!DialogController.IsConversation)
            {
                Talk(dialogText);
                
            }
            else
            {
                //Player.isStop = false;
                //gameObject.SetActive(false);
            }
        }

    }
    void OnEnable()
    {
        Player.isStop = true;
    }
    void OnDisable()
    {
         Player.isStop = false;
    }
}
