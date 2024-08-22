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
                Debug.Log("���ο� ����");
                break;
            case BtnType.Load:
                Debug.Log("�̾ �ϱ�");
                break;
            case BtnType.Setting:
                Debug.Log("ȯ�漳��");
                break;
            case BtnType.Exit:
                Debug.Log("���� ����");
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
