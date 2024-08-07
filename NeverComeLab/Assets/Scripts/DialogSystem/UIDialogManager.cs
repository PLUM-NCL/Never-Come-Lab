using System.Collections;
using System.Collections.Generic;
using KoreanTyper;
using UnityEngine;
using TMPro;

public class UIDialogManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI dialogText;
    private string textString;
    private int dialogCount = 1;
    // Start is called before the first frame update
    void Start()
    {
        dialogText.text = "어라..";
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            if(dialogCount == 1)
            {
                dialogText.text = "나는 죽은건가..?";
   
                StartCoroutine(TypingRoutine());
            }
            else if(dialogCount == 2) {
                dialogText.text = "아.. 이건 꿈인가..";

                StartCoroutine(TypingRoutine());
            }
            else if(dialogCount == 3)
            {
                dialogText.text = "흐흐.. 이 모든게 다 꿈이었다면.....";

                StartCoroutine(TypingRoutine());
            }
        }
    }
    IEnumerator TypingRoutine() // 타이핑 코루틴
    {
        textString = dialogText.text;
        int typingLength = textString.GetTypingLength();

        for (int index = 0; index <= typingLength; index++)
        {
            dialogText.text = textString.Typing(index);
            yield return new WaitForSeconds(0.05f);
        }

        dialogCount++;
    }
}
