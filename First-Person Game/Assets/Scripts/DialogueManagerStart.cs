using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueManagerStart : MonoBehaviour
{

    public TextMeshProUGUI dialogueText;
    private Queue<string> sentences;

    void Start()
    {
        sentences = new Queue<string>();
    }

    public void FirstSceneStartDialogue(DialogueStarting dialogueStarting)
    {
        sentences.Clear();

        foreach (string sentence in dialogueStarting.startingSentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return null;
        }
    }
    void EndDialogue()
    {

    }

}

