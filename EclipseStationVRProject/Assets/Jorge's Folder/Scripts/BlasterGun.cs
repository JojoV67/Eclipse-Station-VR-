using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit; // Required for XR events
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class BlasterGun : MonoBehaviour
{
    /*[Header("Projectile Settings")]
    public GameObject projectilePrefab; // Assign your BlasterBoltPrefab here in the Inspector
    public Transform muzzlePoint;       // Assign the MuzzlePoint child GameObject here
    public float shootForce = 20f;      // Adjust the speed/force of the projectile
    public AudioClip shootSound;

    private XRGrabInteractable grabInteractable;

    void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        if (grabInteractable == null)
        {
            Debug.LogError("BlasterGun script requires an XRGrabInteractable component on the same GameObject.", this);
        }
    }

    void OnEnable()
    {
        if (grabInteractable != null)
        {
            // Subscribe to the 'activated' event (usually the trigger pull when held)
            grabInteractable.activated.AddListener(Shoot);
        }
    }

    void OnDisable()
    {
        if (grabInteractable != null)
        {
            // Unsubscribe when the object is disabled or destroyed
            grabInteractable.activated.RemoveListener(Shoot);
        }
    }

    // This method is called when the 'activated' event occurs on the XR Grab Interactable
    public void Shoot(ActivateEventArgs args)
    {
        if (projectilePrefab != null && muzzlePoint != null)
        {
            // Instantiate the projectile at the muzzle point's position and rotation
            GameObject projectileInstance = Instantiate(projectilePrefab, muzzlePoint.position, muzzlePoint.rotation);

            // Get the Rigidbody component of the new projectile
            Rigidbody rb = projectileInstance.GetComponent<Rigidbody>();

            if (rb != null)
            {
                // Apply force in the forward direction of the muzzle point
                rb.AddForce(muzzlePoint.forward * shootForce, ForceMode.VelocityChange); // VelocityChange ignores mass for consistent speed
            }
            else
            {
                Debug.LogError("Projectile prefab is missing a Rigidbody component.", projectilePrefab);
            }

            // Optional: Add sound effect instantiation here
            AudioSource.PlayClipAtPoint(shootSound, muzzlePoint.position);
        }
        else
        {
            if (projectilePrefab == null) Debug.LogError("Projectile Prefab is not assigned in the Inspector.", this);
            if (muzzlePoint == null) Debug.LogError("Muzzle Point is not assigned in the Inspector.", this);
        }
    }

    // Optional: If you want to shoot on trigger press *without* grabbing,
    // you might need a different setup involving Input Actions directly.
    // This setup assumes the gun must be grabbed first.*/

    [Header("Projectile Settings")]
    public GameObject projectilePrefab;
    public Transform muzzlePoint;
    public float shootForce = 20f;

    [Header("Audio Settings")] // <-- New Section
    public AudioClip shootSound; // <-- Assign your audio clip here in the Inspector
    [Range(0f, 1f)] public float shootSoundVolume = 0.7f; // <-- Optional volume control

    private XRGrabInteractable grabInteractable;

    void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        if (grabInteractable == null)
        {
            Debug.LogError("BlasterGun script requires an XRGrabInteractable component on the same GameObject.", this);
        }
    }

    void OnEnable()
    {
        if (grabInteractable != null)
        {
            grabInteractable.activated.AddListener(Shoot);
        }
    }

    void OnDisable()
    {
        if (grabInteractable != null)
        {
            grabInteractable.activated.RemoveListener(Shoot);
        }
    }

    public void Shoot(ActivateEventArgs args)
    {
        if (projectilePrefab != null && muzzlePoint != null)
        {
            // Instantiate projectile (same as before)
            GameObject projectileInstance = Instantiate(projectilePrefab, muzzlePoint.position, muzzlePoint.rotation);
            Rigidbody rb = projectileInstance.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddForce(muzzlePoint.forward * shootForce, ForceMode.VelocityChange);
            }
            else
            {
                Debug.LogError("Projectile prefab is missing a Rigidbody component.", projectilePrefab);
            }

            // --- Play Sound Effect --- // <-- New Code
            if (shootSound != null)
            {
                // Play the sound at the muzzle's position with the specified volume
                AudioSource.PlayClipAtPoint(shootSound, muzzlePoint.position, shootSoundVolume);
            }
            else
            {
                Debug.LogWarning("Shoot Sound not assigned on BlasterGun.", this);
            }
            // --- End Sound Effect --- //

        }
        else
        {
            if (projectilePrefab == null) Debug.LogError("Projectile Prefab is not assigned in the Inspector.", this);
            if (muzzlePoint == null) Debug.LogError("Muzzle Point is not assigned in the Inspector.", this);
        }
    }
}
