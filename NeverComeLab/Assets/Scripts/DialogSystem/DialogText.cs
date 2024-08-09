using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/Dialog")]
public class DialogText : ScriptableObject
{
    [System.Serializable]
    public class SpeakerData
    {
        public string speakerName;

        [TextArea(5, 10)]
        public string dialogText;
    }
    public SpeakerData[] speakerData;
}
