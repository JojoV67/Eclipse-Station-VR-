/*using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public DialogueLine[] dialogueLines;

    private bool triggered = false;

    void OnTriggerEnter(Collider other)
    {
        if (triggered) return;

        if (other.CompareTag("Player"))
        {
            DialogueManager.Instance.StartDialogue(dialogueLines);
            triggered = true;
        }
    }
}*/
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public AudioSource dialogueSource;
    public AudioClip dialogueClip;
    private bool hasPlayed = false;

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Entered trigger with: " + other.name);

        if (!hasPlayed && other.CompareTag("Player"))
        {
            Debug.Log("Player entered. Trying to play dialogue.");

            if (dialogueSource != null && dialogueClip != null)
            {
                dialogueSource.clip = dialogueClip;
                dialogueSource.Play();
                Debug.Log("Dialogue played: " + dialogueClip.name);
                hasPlayed = true;
            }
            else
            {
                Debug.LogWarning("Missing AudioSource or AudioClip.");
            }
        }
    }
}

