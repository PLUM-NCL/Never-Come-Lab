using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using KoreanTyper;

public class DialogController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI conversationText;
    [SerializeField] private GameObject nextMark;

    private Queue<DialogText.SpeakerData> textsQueue = new Queue<DialogText.SpeakerData>();

    private bool isTellAll;

    private static bool isConversation = false;
    public static bool IsConversation
    {
        get { return isConversation; }
    }

    private DialogText.SpeakerData temp;
    private Coroutine typingRoutine = null;
    public void DisplayNextText(DialogText dialogText)
    {
        nextMark.SetActive(false);

        if (textsQueue.Count == 0 && typingRoutine == null)
        {
            if(!isTellAll) 
            {
                StartConversation(dialogText);
            }
            else
            {
                EndConversation();
                return;
            }
        }

        if (typingRoutine != null)
        {
            StopCoroutine(typingRoutine); 
            typingRoutine = null;
            conversationText.text = temp.dialogText;
            nextMark.SetActive(true);
            return;
        }

        if (textsQueue.Count > 0 && typingRoutine == null) {
            
            temp = textsQueue.Dequeue();
            nameText.text = temp.speakerName;
            conversationText.text = temp.dialogText;
             typingRoutine = StartCoroutine(TypingRoutine());

        }

        if (textsQueue.Count == 0)
            isTellAll = true;

        
    }

    private void StartConversation(DialogText dialogText)
    {
        
        if (!gameObject.activeSelf)
        {
            isConversation = true;
            gameObject.SetActive(true);
        }
        

         for(int i = 0; i< dialogText.speakerData.Length; i++)
        {
            textsQueue.Enqueue(dialogText.speakerData[i]);
        }
    }

    private void EndConversation()
    {
        
        isTellAll = false;

        if (gameObject.activeSelf)
        {
            gameObject.SetActive(false);
            isConversation = false;
        }
    }

    IEnumerator TypingRoutine() // 타이핑 코루틴
    {
        temp.dialogText = conversationText.text;

        int typingLength = temp.dialogText.GetTypingLength();
        for (int index = 0; index <= typingLength; index++)
        {
            conversationText.text = temp.dialogText.Typing(index);
            yield return new WaitForSeconds(0.05f);
            
        }
        nextMark.SetActive(true);
        typingRoutine = null;
    }

}
