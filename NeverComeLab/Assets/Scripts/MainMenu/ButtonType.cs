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
                Debug.Log("새로운 시작");
                break;
            case BtnType.Continue:
                Debug.Log("이어서 하기");
                break;
            case BtnType.Option:
                break;
            case BtnType.Exit:
                break;
          
        }
    }
}
