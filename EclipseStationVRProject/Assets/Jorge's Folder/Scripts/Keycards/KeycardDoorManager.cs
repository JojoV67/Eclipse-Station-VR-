/*using UnityEngine;

public class KeycardDoorManager : MonoBehaviour
{
    public GameObject[] keycardIcons; // Assign the 3 icon GameObjects in order
    public AudioSource insertSound;
    public AudioSource completeSound; // sound when all keycards are inserted
    public GameObject triggerZone; // Reference to Vertical_Doors_Bridge
    private int insertedCount = 0;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Keycard") && insertedCount < 3)
        {
            Debug.Log("Keycard detected!");

            Destroy(other.gameObject); // Remove the card
            keycardIcons[insertedCount].SetActive(true); // Turn on the icon
            insertSound.Play(); // Play sound
            insertedCount++;

            if (insertedCount >= 3 && triggerZone != null)
            {
                Debug.Log("All keycards inserted. Enabling trigger zone.");
                triggerZone.SetActive(true); // Enable the trigger zone
                if (completeSound != null)
                {
                    completeSound.Play(); // Play the completion sound!
                }
            }
        }
    }
}*/
using UnityEngine;

public class KeycardDoorManager : MonoBehaviour
{
    public GameObject[] keycardIcons; // Assign icons or 3D capsules in Inspector
    public AudioSource insertSound;
    public AudioSource completeSound;
    public GameObject triggerZone;
    private int insertedCount = 0;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Keycard") && insertedCount < keycardIcons.Length)
        {
            Debug.Log("Keycard detected!");

            if (other.gameObject != null)
                Destroy(other.gameObject); // Safely destroy the keycard

            if (keycardIcons[insertedCount] != null)
            {
                keycardIcons[insertedCount].SetActive(true); // Show inserted icon
            }

            insertSound?.Play();
            insertedCount++;

            if (insertedCount >= keycardIcons.Length && triggerZone != null)
            {
                Debug.Log("All keycards inserted. Enabling trigger zone.");
                triggerZone.SetActive(true);
                completeSound?.Play();
            }
        }
    }
}

