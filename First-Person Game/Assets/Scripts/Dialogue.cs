using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogue
{
    public string dialogueDiscription;
    [TextArea(3, 10)]
    public string[] sentences;
}