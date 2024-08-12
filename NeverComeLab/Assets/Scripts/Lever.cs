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
                target.SendMessage(OnMessage);  //Ÿ��(LeverWall)�� OnMessage �Լ��� �´� �޼��� ���� 
            }
        }
        else if (!on)
        {
            if (target != null && !string.IsNullOrEmpty(OffMessage))
            {
                target.SendMessage(OffMessage); //Ÿ��(LeverWall)�� OnMessage �Լ��� �´� �޼��� ���� 
            }
        }
    }
}
