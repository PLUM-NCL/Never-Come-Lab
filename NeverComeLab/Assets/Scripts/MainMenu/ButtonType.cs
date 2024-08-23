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
                SceneManager.LoadScene("Prologue");  // New ��ư ���ý� ���ѷα� ȭ������ ����
                break;
            case ButtonType.Exit:
                Application.Quit();
                break;
            // ���� ��ư �߰��� ���� �ڵ� �߰� �ۼ�
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
