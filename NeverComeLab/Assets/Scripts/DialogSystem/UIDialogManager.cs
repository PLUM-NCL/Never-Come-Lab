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
        dialogText.text = "���..";
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            if(dialogCount == 1)
            {
                dialogText.text = "���� �����ǰ�..?";
   
                StartCoroutine(TypingRoutine());
            }
            else if(dialogCount == 2) {
                dialogText.text = "��.. �̰� ���ΰ�..";

                StartCoroutine(TypingRoutine());
            }
            else if(dialogCount == 3)
            {
                dialogText.text = "����.. �� ���� �� ���̾��ٸ�.....";

                StartCoroutine(TypingRoutine());
            }
        }
    }
    IEnumerator TypingRoutine() // Ÿ���� �ڷ�ƾ
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
