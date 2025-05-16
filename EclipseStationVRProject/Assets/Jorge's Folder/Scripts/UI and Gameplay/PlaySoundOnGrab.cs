/*using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(XRGrabInteractable))]
public class PlaySoundOnGrab : MonoBehaviour
{
    private AudioSource audioSource;
    private XRGrabInteractable grabInteractable;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        grabInteractable = GetComponent<XRGrabInteractable>();

        // Hook into the select entered event (when the object is grabbed)
        grabInteractable.selectEntered.AddListener(OnGrab);
    }

    void OnDestroy()
    {
        // Clean up the listener
        grabInteractable.selectEntered.RemoveListener(OnGrab);
    }

    private void OnGrab(SelectEnterEventArgs args)
    {
        if (audioSource != null && audioSource.clip != null)
        {
            audioSource.Play();
        }
    }
}*/

/*using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables; // Make sure this is included

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(XRGrabInteractable))]
public class PlaySoundOnGrab : MonoBehaviour
{
    private AudioSource audioSource;
    private XRGrabInteractable grabInteractable;

    [Header("Subtitle Settings")]
    [TextArea(3, 5)]
    public string subtitleText = "Object grabbed!";
    public SubtitleManager subtitleManager; // Assign your SubtitleManager instance

    // Optional: to make it play only once per grab/lifetime
    // public bool playOnlyOnce = false;
    // private bool hasBeenPlayed = false;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        grabInteractable = GetComponent<XRGrabInteractable>();

        // Attempt to find SubtitleManager if not assigned (optional)
        if (subtitleManager == null)
        {
            subtitleManager = FindFirstObjectByType<SubtitleManager>();
            if (subtitleManager == null)
            {
                Debug.LogError($"PlaySoundOnGrab on {gameObject.name}: SubtitleManager not assigned and could not be found! Subtitles will not work.", this);
            }
        }

        if (audioSource.clip == null)
        {
            Debug.LogWarning($"PlaySoundOnGrab on {gameObject.name}: The AudioSource component is missing an AudioClip. No sound will play.", this);
        }

        grabInteractable.selectEntered.AddListener(OnGrab);
    }

    void OnDestroy()
    {
        if (grabInteractable != null)
        {
            grabInteractable.selectEntered.RemoveListener(OnGrab);
        }
    }

    private void OnGrab(SelectEnterEventArgs args)
    {
        // if (playOnlyOnce && hasBeenPlayed) return;

        if (audioSource != null && audioSource.clip != null && subtitleManager != null)
        {
            // Let the SubtitleManager handle playing the audio and showing the text
            subtitleManager.PlayDialogue(audioSource, audioSource.clip, subtitleText);
            // hasBeenPlayed = true;
        }
        else
        {
            if (subtitleManager == null) Debug.LogWarning($"PlaySoundOnGrab on {gameObject.name}: SubtitleManager reference is missing.");
            if (audioSource == null) Debug.LogWarning($"PlaySoundOnGrab on {gameObject.name}: AudioSource is missing.");
            else if (audioSource.clip == null) Debug.LogWarning($"PlaySoundOnGrab on {gameObject.name}: AudioClip on AudioSource is missing.");
        }
    }
}*/

//Update to enable subtitles
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables; // Make sure this is included

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(XRGrabInteractable))]
public class PlaySoundOnGrab : MonoBehaviour
{
    private AudioSource audioSource;
    private XRGrabInteractable grabInteractable;

    [Header("Subtitle Settings")]
    [TextArea(3, 5)]
    public string subtitleText = "Object grabbed!";
    public SubtitleManager subtitleManager; // Assign your SubtitleManager instance

    [Header("Playback Control")]
    [Tooltip("If true, the sound and subtitle will only play the first time the object is grabbed.")]
    public bool playOnlyOnce = true; // Set this to true by default or in Inspector

    private bool hasBeenPlayed = false; // Flag to track if it has played

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        grabInteractable = GetComponent<XRGrabInteractable>();

        if (subtitleManager == null)
        {
            subtitleManager = FindFirstObjectByType<SubtitleManager>();
            if (subtitleManager == null)
            {
                Debug.LogError($"PlaySoundOnGrab on {gameObject.name}: SubtitleManager not assigned and could not be found! Subtitles will not work.", this);
            }
        }

        if (audioSource.clip == null)
        {
            Debug.LogWarning($"PlaySoundOnGrab on {gameObject.name}: The AudioSource component is missing an AudioClip. No sound will play.", this);
        }

        grabInteractable.selectEntered.AddListener(OnGrab);
    }

    void OnDestroy()
    {
        if (grabInteractable != null)
        {
            grabInteractable.selectEntered.RemoveListener(OnGrab);
        }
    }

    private void OnGrab(SelectEnterEventArgs args)
    {
        // Check if we should play only once AND if it has already been played
        if (playOnlyOnce && hasBeenPlayed)
        {
            return; // If so, do nothing and exit the method
        }

        if (audioSource != null && audioSource.clip != null && subtitleManager != null)
        {
            subtitleManager.PlayDialogue(audioSource, audioSource.clip, subtitleText);

            // If playOnlyOnce is true, mark it as played so it doesn't play again
            if (playOnlyOnce)
            {
                hasBeenPlayed = true;
            }
        }
        else
        {
            // Your existing warning logs
            if (subtitleManager == null) Debug.LogWarning($"PlaySoundOnGrab on {gameObject.name}: SubtitleManager reference is missing.");
            if (audioSource == null) Debug.LogWarning($"PlaySoundOnGrab on {gameObject.name}: AudioSource is missing.");
            else if (audioSource.clip == null) Debug.LogWarning($"PlaySoundOnGrab on {gameObject.name}: AudioClip on AudioSource is missing.");
        }
    }

    /// <summary>
    /// Public method to reset the played state, allowing the sound to play again on next grab.
    /// Useful if you need to re-enable it via another script or event.
    /// </summary>
    public void ResetPlayedState()
    {
        hasBeenPlayed = false;
    }
}