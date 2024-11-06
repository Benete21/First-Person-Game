using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueStarting
{
        public string startingDialogueDiscription;
        [TextArea(3, 10)]
        public string[] startingSentences;
}
