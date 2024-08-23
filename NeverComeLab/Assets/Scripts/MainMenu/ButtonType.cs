using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class NewBehaviourScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public ButtonType currentType;
    public Transform buttonScale;
    Vector3 defaultScale;

    public void Start()
    {
        defaultScale = buttonScale.localScale;
    }

    public void OnClick()
    {
        switch (currentType)
        {
            case ButtonType.New:
                SceneManager.LoadScene(2);  // New 버튼 선택시 프롤로그 화면으로 진입
                break;
            case ButtonType.Exit:
                Application.Quit();
                break;
            // 추후 버튼 추가에 따라 코드 추가 작성
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
