using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour
{
    //Never: 한번 레버 만지면 다신 원상복구 못함
    //OnUse: 왼쪽 오른쪽 하는거 자유로움
    //Timed: 일정 시간 지나면 원상태로 돌아옴 
    //Immediately: 열리자마자 바로닫침(가짜 레버로 활용 가능 / 사실 사용못하쥬? 꼴받쥬? 이런 느낌)
    public enum ResetType { Never, OnUse, Timed, Fake }

    public ResetType resetType = ResetType.OnUse;
    public List<TargetMessage> targets = new List<TargetMessage>();
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
        //OnUse(자유로움)와 Immediately(바로원상태)만 해당 함수 실행
        if (isOn && resetType != ResetType.Never && resetType != ResetType.Timed)
        {
            SetState(false);
        }
    }

    public void TimeReset()
    {
        //Timed(일정시간 지나면 닫침)만 해당 함수 실행. 유저 마음대로 off 못하게 따로 함수로 뺌. 
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

        AudioManager.instance.PlaySfx(AudioManager.Sfx.Lever);

        if (on)
        {
            foreach (var target in targets)
            {
                if (target.targetObject != null && !string.IsNullOrEmpty(target.onMessage))
                {
                    target.targetObject.SendMessage(target.onMessage); 
                }
            }  //타켓(LeverWall)에 OnMessage 함수명에 맞는 메세지 전송 


            if (resetType == ResetType.Fake)
                TurnOff();
            else if (resetType == ResetType.Timed)
                Invoke("TimeReset", resetTime);
        }
        else if (!on)
        {
            foreach (var target in targets)
            {
                if (target.targetObject != null && !string.IsNullOrEmpty(target.offMessage))
                {
                    target.targetObject.SendMessage(target.offMessage); // OffMessage 전송
                }
            } //타켓(LeverWall)에 OffMessage 함수명에 맞는 메세지 전송 
        }
    }
}
