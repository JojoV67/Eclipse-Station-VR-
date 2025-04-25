/*using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public AudioSource dialogueSource; // Optional: Can be separate
    public AudioClip dialogueClip;     // The voice line to play
    private bool hasPlayed = false;

    void OnTriggerEnter(Collider other)
    {
        if (!hasPlayed && other.CompareTag("Player"))
        {
            if (dialogueSource != null && dialogueClip != null)
            {
                dialogueSource.clip = dialogueClip;
                dialogueSource.Play();
                hasPlayed = true;
                Debug.Log("Dialogue played: " + dialogueClip.name);
            }
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


