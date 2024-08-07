using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/Dialog")]
public class DialogText : ScriptableObject
{
    public string speakerName;

    [TextArea(5, 10)]
    public string[] dialogTexts;
}
