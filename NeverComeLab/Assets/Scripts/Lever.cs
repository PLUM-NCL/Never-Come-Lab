using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour
{
    public GameObject target;
    public string OnMessage;
    public string OffMessage;
    public bool isOn;

    Animator anim;


    void Start()
    {
        anim = GetComponent<Animator>();    
    }

    public void TurnOn()
    {
        if (!isOn)
        {
            SetState(true); 
        }
    }

    public void TurnOff()
    {
        if (isOn)
        {
            SetState(false);
        }
    }

    public void Toggle()
    {
        if (isOn)
        {
            TurnOff();
        }
        else if (!isOn){
            TurnOn();
        }
    }

    void SetState(bool on)
    {
        isOn = on;
        anim.SetBool("On", on);

        if (on)
        {
            if (target != null && !string.IsNullOrEmpty(OnMessage))
            {
                target.SendMessage(OnMessage);  //타켓(LeverWall)에 OnMessage 함수명에 맞는 메세지 전송 
            }
        }
        else if (!on)
        {
            if (target != null && !string.IsNullOrEmpty(OffMessage))
            {
                target.SendMessage(OffMessage); //타켓(LeverWall)에 OnMessage 함수명에 맞는 메세지 전송 
            }
        }
    }
}
