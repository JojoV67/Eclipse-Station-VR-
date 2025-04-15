using UnityEngine;

public class DialogueTriggerZone : MonoBehaviour
{
    public DialogueLine[] dialogueLines;

    private bool hasPlayed = false;

    private void OnTriggerEnter(Collider other)
    {
        if(!hasPlayed && other.CompareTag("MainCamera"))
        {
            hasPlayed = true;
            DialogueManager.Instance.StartDialogue(dialogueLines);
        }
    }
}
