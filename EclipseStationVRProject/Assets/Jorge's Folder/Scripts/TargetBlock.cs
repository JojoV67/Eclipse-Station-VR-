using UnityEngine;

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
}
