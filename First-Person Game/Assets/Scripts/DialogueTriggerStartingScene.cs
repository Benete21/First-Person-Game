using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTriggerStartingScene : MonoBehaviour
{
    Dialogue dialogueStarting;
    public void TriggerStartingDialogue()
    {
        FindObjectOfType<DialogueManager>().StartDialogue(dialogueStarting);
    }
}
