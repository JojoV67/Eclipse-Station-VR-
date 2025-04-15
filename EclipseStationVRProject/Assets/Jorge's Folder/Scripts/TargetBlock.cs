/*using UnityEngine;

public class TargetBlock : MonoBehaviour
{
    [Header("Effects")]
    public GameObject disintegrationEffectPrefab; // Assign your disintegration particle effect Prefab here
    public float delayBeforeDestroy = 0.1f; // Small delay to let effect start playing

    private bool isHit = false; // Prevents trying to destroy multiple times

    // This method is called by the Projectile script when hit
    public void HitByBlaster()
    {
        if (isHit) return; // Already hit, do nothing more
        isHit = true;

        // --- Disintegration Logic ---

        // 1. Instantiate the particle effect (if assigned)
        if (disintegrationEffectPrefab != null)
        {
            Instantiate(disintegrationEffectPrefab, transform.position, Quaternion.identity);
        }

        // 2. Optional: Disable visuals/collider immediately if needed
        // GetComponent<Renderer>().enabled = false;
        // GetComponent<Collider>().enabled = false;

        // 3. Destroy the block GameObject after a short delay
        Destroy(gameObject, delayBeforeDestroy);

        // Optional: Add sound effect instantiation here
        // AudioSource.PlayClipAtPoint(destroySound, transform.position);

        // Optional: Add score update logic here
        // GameManager.Instance.AddScore(10);
    }

    // Optional: You could add health here later if needed
    // public float health = 1f;
    // public void TakeDamage(float amount) {
    //    health -= amount;
    //    if (health <= 0 && !isHit) {
    //       HitByBlaster(); // Rename HitByBlaster to something like Die()
    //    }
    // }
}*/

/*using UnityEngine;

public class TargetBlock : MonoBehaviour
{
    [Header("Effects")]
    public GameObject disintegrationEffectPrefab; // Assign your disintegration particle effect prefab
    public float delayBeforeDestroy = 0.1f;

    [Header("Audio")]
    public AudioClip destroySound; // Optional sound to play when disintegrating
    [Range(0f, 1f)]
    public float destroySoundVolume = 0.8f;

    private bool isHit = false;

    public void HitByBlaster()
    {
        if (isHit) return;
        isHit = true;

        // --- Spawn the particle effect ---
        if (disintegrationEffectPrefab != null)
        {
            Instantiate(disintegrationEffectPrefab, transform.position, Quaternion.identity);
        }

        // --- Play sound effect ---
        if (destroySound != null)
        {
            AudioSource.PlayClipAtPoint(destroySound, transform.position, destroySoundVolume);
        }

        // --- Optional: Instantly hide the block visually ---
        Renderer rend = GetComponent<Renderer>();
        if (rend != null) rend.enabled = false;

        Collider col = GetComponent<Collider>();
        if (col != null) col.enabled = false;

        // --- Schedule destruction ---
        Destroy(gameObject, delayBeforeDestroy);
    }
}*/

using UnityEngine;

public class TargetBlock : MonoBehaviour
{
    [Header("Effects")]
    public GameObject dissolveEffectPrefab; // <- Assign the edited "Dissolve" prefab here
    public float delayBeforeDestroy = 2f; // Match this with effect duration

    [Header("Audio")]
    public AudioClip destroySound; // Optional sound to play when disintegrating
    [Range(0f, 1f)]
    public float destroySoundVolume = 0.8f;

    private bool isHit = false;

    public void HitByBlaster()
    {
        if (isHit) return;
        isHit = true;

        // Spawn the dissolve effect
        if (dissolveEffectPrefab != null)
        {
            //GameObject effect = Instantiate(dissolveEffectPrefab, transform.position, Quaternion.identity);
            GameObject effect = Instantiate(dissolveEffectPrefab, transform.position, Quaternion.identity, transform);
        }

        // --- Play sound effect ---
        if (destroySound != null)
        {
            AudioSource.PlayClipAtPoint(destroySound, transform.position, destroySoundVolume);
        }

        // Optional: Hide visuals immediately
        Renderer rend = GetComponent<Renderer>();
        if (rend != null) rend.enabled = false;

        Collider col = GetComponent<Collider>();
        if (col != null) col.enabled = false;

        // Destroy the object after delay
        Destroy(gameObject, delayBeforeDestroy);
    }
}


