using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseLever : Lever
{
    //Player에서 Use()라는 메세지를 받아왔다면 
    public void Use()
    {
        Toggle();
    }
}
