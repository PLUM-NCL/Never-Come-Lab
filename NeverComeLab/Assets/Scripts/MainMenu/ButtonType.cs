using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonType : MonoBehaviour
{
    public BtnType currentType;

    public void OnButtonClick()
    {
        switch(currentType)
        {
            case BtnType.New:
                Debug.Log("���ο� ����");
                break;
            case BtnType.Continue:
                Debug.Log("�̾ �ϱ�");
                break;
            case BtnType.Option:
                break;
            case BtnType.Exit:
                break;
          
        }
    }
}
