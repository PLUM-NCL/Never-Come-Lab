using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Reporting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class ButtonType : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public BtnType currentType;
    public Transform buttonScale;
    Vector3 defaultScale;

    private void Start()
    {
        defaultScale = buttonScale.localScale;
    }

    public void OnButtonClick()
    {
        switch(currentType)
        {
            case BtnType.New:
                SceneManager.LoadScene("Prologue");
                Debug.Log("새로운 시작");
                break;
            case BtnType.Load:
                Debug.Log("이어서 하기");
                break;
            case BtnType.Setting:
                Debug.Log("환경설정");
                break;
            case BtnType.Exit:
                Debug.Log("게임 종료");
                Application.Quit();
                break;
          
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        buttonScale.localScale = defaultScale * 1.2f;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        buttonScale.localScale = defaultScale;
    }
}
