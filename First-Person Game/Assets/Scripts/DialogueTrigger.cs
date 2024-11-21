using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{

    public Dialogue dialogue;
    public GameObject player;
    public Collider playerCollider;

    public void TriggerDialogue()
    {
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
    }
    private void OnTriggerEnter(Collider other)
    {
        // Check the name of the collided object
        if (other = playerCollider)
        {
            TriggerDialogue();
            Destroy(gameObject);
        }

    }

}
