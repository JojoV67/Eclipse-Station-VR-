using UnityEngine;

public class KeycardCompleteFeedback : MonoBehaviour
{
    public AudioSource completeSound; // Sound to play
    public Renderer buttonRenderer;   // Renderer for the button
    public Material greenMaterial;    // Material to switch to

    private bool hasTriggered = false;

    public void TriggerFeedback()
    {
        if (hasTriggered) return;

        if (completeSound != null)
        {
            completeSound.Play();
            Debug.Log("Played completion sound.");
        }

        if (buttonRenderer != null && greenMaterial != null)
        {
            buttonRenderer.material = greenMaterial;
            Debug.Log("Changed button material to green.");
        }

        hasTriggered = true;
    }
}

