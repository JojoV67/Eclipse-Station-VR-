using UnityEngine;

public class RedButtonTrigger : MonoBehaviour
{
    private bool isActivated = false;

    public AudioSource pressSound;
    public Renderer buttonRenderer;
    public Material pressedMaterial;

    private void OnTriggerEnter(Collider other)
    {
        if (isActivated) return;

        if (other.CompareTag("Player"))
        {
            isActivated = true;

            // Optional: play press sound
            if (pressSound != null) 
                pressSound.Play();

            // Optional: change material
            if (buttonRenderer != null && pressedMaterial != null)
                buttonRenderer.material = pressedMaterial;

            /* Register the reset with the FinalSequenceManager
            if (FinalSequenceManager.instance != null)
                FinalSequenceManager.instance.RegisterReset();
            */

            // Optional: disable collider to prevent re-press
            GetComponent<Collider>().enabled = false;
        }
    }
}
