using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTriggerStartingScene : MonoBehaviour
{
    public DialogueStarting dialogueStarting;
    public void TriggerStartingDialogue()
    {
        FindObjectOfType<DialogueManagerStart>().FirstSceneStartDialogue(dialogueStarting);
    }
}
