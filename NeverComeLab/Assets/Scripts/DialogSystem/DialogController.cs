using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI dialogText;

    private Queue<string> textsQueue = new Queue<string>();

    private bool isConversationEnd;

    public void DisplayNextText(DialogText dialogText)
    {
        if(textsQueue.Count == 0)
        {
            if(!isConversationEnd) 
            {
                StartConversation(dialogText);
            }
            else
            {
                EndConversation(dialogText);
            }
        }
    }

    private void StartConversation(DialogText dialogText)
    {
        if (!gameObject.activeSelf)
        {
            gameObject.SetActive(true);
        }
        nameText.text = dialogText.speakerName;

        for(int i = 0; i< dialogText.dialogTexts.Length; i++)
        {
            textsQueue.Enqueue(dialogText.dialogTexts[i]);
        }
    }

    private void EndConversation(DialogText dialogText)
    {
        if (gameObject.activeSelf)
        {
            gameObject.SetActive(false);
        }
    }
}
