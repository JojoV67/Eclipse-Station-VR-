using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class VRBlaster : MonoBehaviour
{
    public GameObject laserPrefab;
    public Transform shootPoint;

    public InputActionProperty triggerAction; 
    private bool isTriggerPressed = false;

    [Header("Audio Settings")] // <-- New Section
    public AudioClip shootSound; // <-- Assign your audio clip here in the Inspector
    [Range(0f, 1f)] public float shootSoundVolume = 0.7f; // <-- Optional volume control

    // Update is called once per frame
    void Update()
    {
        if (triggerAction.action.WasPressedThisFrame())
        {
            Shoot();
        }
    }
    void Shoot()
    {
        GameObject laser = Instantiate(laserPrefab, shootPoint.position, shootPoint.rotation);

        if (shootSound != null)
        {
            // Play the sound at the muzzle's position with the specified volume
            AudioSource.PlayClipAtPoint(shootSound, shootPoint.position, shootSoundVolume);
        }
        else
        {
            Debug.LogWarning("Shoot Sound not assigned on BlasterGun.", this);
        }
    }
}
