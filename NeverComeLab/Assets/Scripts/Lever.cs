using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour
{
    //Never: �ѹ� ���� ������ �ٽ� ���󺹱� ����
    //OnUse: ���� ������ �ϴ°� �����ο�
    //Timed: ���� �ð� ������ �����·� ���ƿ� 
    //Immediately: �����ڸ��� �ٷδ�ħ(��¥ ������ Ȱ�� ���� / ��� ��������? �ù���? �̷� ����)
    public enum ResetType { Never, OnUse, Timed, Immediately }

    public ResetType resetType = ResetType.OnUse;
    public GameObject target;
    public string OnMessage;
    public string OffMessage;
    public bool isOn;
    public float resetTime;

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
        //OnUse(�����ο�)�� Immediately(�ٷο�����)�� �ش� �Լ� ����
        if (isOn && resetType != ResetType.Never && resetType != ResetType.Timed)
        {
            SetState(false);
        }
    }

    public void TimeReset()
    {
        //Timed(�����ð� ������ ��ħ)�� �ش� �Լ� ����. ���� ������� off ���ϰ� ���� �Լ��� ��. 
        SetState(false);
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
                target.SendMessage(OnMessage);  //Ÿ��(LeverWall)�� OnMessage �Լ��� �´� �޼��� ���� 
            if (resetType == ResetType.Immediately)
                TurnOff();
            else if (resetType == ResetType.Timed)
                Invoke("TimeReset", resetTime);
        }
        else if (!on)
        {
            if (target != null && !string.IsNullOrEmpty(OffMessage))
                target.SendMessage(OffMessage); //Ÿ��(LeverWall)�� OnMessage �Լ��� �´� �޼��� ���� 
        }
    }
}
