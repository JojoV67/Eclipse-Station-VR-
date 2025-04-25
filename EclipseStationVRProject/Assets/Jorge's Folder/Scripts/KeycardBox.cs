/*using UnityEngine;

public class KeycardBox : MonoBehaviour
{
    public GameObject[] keycardIcons; // Assign the 3 icon GameObjects in order
    public AudioSource insertSound;
    public GameObject triggerZone; // Reference to Vertical_Doors_Bridge
    private int insertedCount = 0;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Keycard") && insertedCount < 3)
        {
            Destroy(other.gameObject); // Remove the card
            keycardIcons[insertedCount].SetActive(true); // Turn on the icon
            insertSound.Play(); // Play sound
            insertedCount++;

            if (insertedCount >= 3 && triggerZone != null)
            {
                triggerZone.SetActive(true); // Enable the trigger zone
            }
        }
    }
}*/

/*using UnityEngine;
using System.Collections;

public class KeycardBox : MonoBehaviour
{
    public GameObject[] keycardIcons;
    public AudioSource insertSound;
    public AudioSource completeSound;
    public GameObject triggerZone;
    public Renderer buttonRenderer; // The red/green button's renderer
    public Material redMaterial;    // Assign in inspector
    public Material greenMaterial;  // Assign in inspector

    private int insertedCount = 0;

    void Start()
    {
        // Start with red button if materials are assigned
        if (buttonRenderer != null && redMaterial != null)
        {
            buttonRenderer.material = redMaterial;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Keycard") && insertedCount < 3)
        {
            Destroy(other.gameObject);
            keycardIcons[insertedCount].SetActive(true);
            insertSound.Play();
            insertedCount++;

            if (insertedCount >= 3)
            {
                StartCoroutine(ActivateTriggerZoneWithDelay());
            }
        }
    }

    private IEnumerator ActivateTriggerZoneWithDelay()
    {
        Debug.Log("Coroutine started: activating trigger zone...");
        yield return new WaitForSeconds(1f); // 1 second delay

        if (triggerZone != null)
        {
            triggerZone.SetActive(true);
        }

        if (completeSound != null)
        {
            //completeSound.Play();
            completeSound.PlayOneShot(completeSound.clip);
        }

        Debug.Log("Sound should be playing.");
        Debug.Log("Button material should now be green.");

        if (buttonRenderer != null && greenMaterial != null)
        {
            buttonRenderer.material = greenMaterial;
        }
    }
}*/

/*using UnityEngine;
using System.Collections;

public class KeycardBox : MonoBehaviour
{
    public GameObject[] keycardIcons;
    public AudioSource insertSound;
    public AudioSource completeSound;
    public GameObject triggerZone;
    public Renderer buttonRenderer;    // Assign your button's MeshRenderer here
    public Material redMaterial;
    public Material greenMaterial;

    private int insertedCount = 0;

    void Start()
    {
        // Start with red material if assigned
        if (buttonRenderer != null && redMaterial != null)
        {
            Debug.Log("Setting button to red at start.");
            buttonRenderer.material = redMaterial;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Keycard") && insertedCount < 3)
        {
            Destroy(other.gameObject);
            keycardIcons[insertedCount].SetActive(true);
            insertSound.Play();
            insertedCount++;

            Debug.Log($"Keycard {insertedCount} inserted.");

            if (insertedCount >= 3)
            {
                Debug.Log("All 3 keycards inserted. Starting coroutine.");
                StartCoroutine(ActivateTriggerZoneWithDelay());
            }
        }
    }

    private IEnumerator ActivateTriggerZoneWithDelay()
    {
        Debug.Log("Coroutine started: activating trigger zone...");
        yield return new WaitForSeconds(1f); // 1 second delay

        if (triggerZone != null)
        {
            Debug.Log("Trigger zone enabled.");
            triggerZone.SetActive(true);
        }

        if (completeSound != null && completeSound.clip != null)
        {
            Debug.Log("Playing complete sound.");
            completeSound.PlayOneShot(completeSound.clip);
        }
        else
        {
            Debug.LogWarning("completeSound or clip is missing!");
        }

        if (buttonRenderer != null)
        {
            if (greenMaterial != null)
            {
                Debug.Log("Switching button to green material.");
                buttonRenderer.material = greenMaterial;
            }
            else
            {
                // If green material is not assigned, fallback to direct color
                Debug.Log("Green material missing, changing color instead.");
                buttonRenderer.material.color = Color.green;
            }
        }
        else
        {
            Debug.LogWarning("buttonRenderer is not assigned!");
        }
    }
}*/
/*using UnityEngine;

public class KeycardBox : MonoBehaviour
{
    public GameObject[] keycardIcons;           // Assign the 3 icon GameObjects in order
    public AudioSource insertSound;             // Sound for each keycard inserted
    public AudioSource completeSound;           // Sound when all keycards are inserted
    public GameObject triggerZone;              // Reference to Vertical_Doors_Bridge
    public Renderer buttonRenderer;             // Renderer of the red/green button
    public Material redMaterial;                // Assign red material in Inspector
    public Material greenMaterial;              // Assign green material in Inspector

    private int insertedCount = 0;

    void Start()
    {
        // Set the button to red at the start, if everything is assigned
        if (buttonRenderer != null && redMaterial != null)
        {
            buttonRenderer.material = redMaterial;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Keycard") && insertedCount < 3)
        {
            Destroy(other.gameObject);                             // Remove the card
            keycardIcons[insertedCount].SetActive(true);           // Turn on the icon
            insertSound.Play();                                     // Play insert sound
            insertedCount++;

            /*if (insertedCount >= 3 && triggerZone != null)
            {
                triggerZone.SetActive(true);                        // Enable the trigger zone

                if (completeSound != null)
                {
                    completeSound.Play();                           // Play completion sound
                }

                if (buttonRenderer != null && greenMaterial != null)
                {
                    buttonRenderer.material = greenMaterial;        // Change button to green
                }
            }*/
/*if (insertedCount >= 3 && triggerZone != null)
{
    Debug.Log("All keycards inserted — enabling trigger zone!");
    triggerZone.SetActive(true);

    /*if (completeSound != null)
    {
        Debug.Log("Playing complete sound...");
        completeSound.Play();
    }
    if (completeSound != null && completeSound.clip != null)
    {
        completeSound.PlayOneShot(completeSound.clip);
    }
    else
    {
        Debug.LogWarning("completeSound or AudioClip is missing!");
    }

    if (buttonRenderer != null && greenMaterial != null)
    {
        Debug.Log("Changing button material to green.");
        buttonRenderer.material = greenMaterial;
    }
    else
    {
        Debug.LogWarning("buttonRenderer or greenMaterial is missing!");
    }
}
}
}
}*/
/*using UnityEngine;

public class KeycardBox : MonoBehaviour
{
    public GameObject[] keycardIcons;           // 3 icon GameObjects
    public AudioSource insertSound;             // Sound when inserting a card
    public AudioSource completeSound;           // Sound when all are inserted
    public GameObject triggerZone;              // Vertical_Doors_Bridge
    public Renderer buttonRenderer;             // Button MeshRenderer
    public Material redMaterial;                // Red material
    public Material greenMaterial;              // Green material

    private int insertedCount = 0;

    void Start()
    {
        if (buttonRenderer != null && redMaterial != null)
        {
            buttonRenderer.material = new Material(redMaterial);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Keycard") && insertedCount < 3)
        {
            Destroy(other.gameObject);
            keycardIcons[insertedCount].SetActive(true);
            insertSound.Play();
            insertedCount++;

            if (insertedCount >= 3 && triggerZone != null)
            {
                Debug.Log("All keycards inserted — enabling trigger zone!");
                triggerZone.SetActive(true);

                if (completeSound != null && completeSound.clip != null)
                {
                    completeSound.PlayOneShot(completeSound.clip);
                }

                if (buttonRenderer != null && greenMaterial != null)
                {
                    Debug.Log("Changing button to green.");
                    buttonRenderer.material = new Material(greenMaterial);
                }
            }
        }
    }
}*/
/*using UnityEngine;

public class KeycardBox : MonoBehaviour
{
    public GameObject[] keycardIcons; // Assign the 3 icon GameObjects in order
    public AudioSource insertSound; // sound for each keycard inserted
    public AudioSource completeSound; // sound when all keycards are inserted
    public GameObject triggerZone; // Reference to Vertical_Doors_Bridge
    private int insertedCount = 0;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Keycard") && insertedCount < 3)
        {
            Destroy(other.gameObject); // Remove the card
            keycardIcons[insertedCount].SetActive(true); // Turn on the icon
            insertSound.Play(); // Play sound for inserting a keycard
            insertedCount++;

            if (insertedCount >= 3 && triggerZone != null)
            {
                triggerZone.SetActive(true); // Enable the trigger zone
                if (completeSound != null)
                {
                    completeSound.Play(); // Play the completion sound!
                }
            }
        }
    }
}*/
/*using UnityEngine;

public class KeycardBox : MonoBehaviour
{
    public GameObject[] keycardIcons; // Assign the 3 icon GameObjects in order
    public AudioSource insertSound; // Sound for each keycard inserted
    public AudioSource completeSound; // Sound when all keycards are inserted
    public GameObject triggerZone; // Reference to Vertical_Doors_Bridge

    public Renderer buttonRenderer; // Assign the Renderer of your red/green button
    public Material greenMaterial;  // The material it should switch to when complete

    private int insertedCount = 0;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Keycard") && insertedCount < 3)
        {
            Destroy(other.gameObject); // Remove the keycard
            keycardIcons[insertedCount].SetActive(true); // Turn on the next icon
            insertSound.Play(); // Play insert sound
            insertedCount++;

            if (insertedCount >= 3)
            {
                if (triggerZone != null)
                {
                    triggerZone.SetActive(true); // Enable trigger zone for the door
                    Debug.Log("All keycards inserted - enabling trigger zone");

                    // Play the completion sound
                    if (completeSound != null)
                    {
                        completeSound.Play();
                    }

                    // Change button material
                    if (buttonRenderer != null && greenMaterial != null)
                    {
                        buttonRenderer.material = greenMaterial;
                        Debug.Log("Button material changed to green!");
                    }
                }
            }
        }
    }
}*/

using UnityEngine;

public class KeycardBox : MonoBehaviour
{
    public GameObject[] keycardIcons;
    public AudioSource insertSound;
    public GameObject triggerZone;

    public KeycardCompleteFeedback completeFeedback; // Reference to feedback script

    private int insertedCount = 0;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Keycard") && insertedCount < 3)
        {
            Destroy(other.gameObject);
            keycardIcons[insertedCount].SetActive(true);
            insertSound.Play();
            insertedCount++;

            if (insertedCount >= 3)
            {
                if (triggerZone != null)
                {
                    triggerZone.SetActive(true);
                    Debug.Log("All keycards inserted - enabling trigger zone");
                }

                if (completeFeedback != null)
                {
                    completeFeedback.TriggerFeedback();
                }
            }
        }
    }
}





