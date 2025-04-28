using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;
    public List<UnityEvent> dialogueEvents;

    private bool triggeredDialogue;
    private bool preTriggeredDialogue;

    public void TriggerDialogue()
    {
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue, dialogueEvents);
        Destroy(gameObject, 1f);
    }

    private void Update()
    {
        if(triggeredDialogue && !preTriggeredDialogue)
        {
            TriggerDialogue();
        }
        preTriggeredDialogue = triggeredDialogue;
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Player"))
            triggeredDialogue = true;
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            triggeredDialogue = false;
    }
}
