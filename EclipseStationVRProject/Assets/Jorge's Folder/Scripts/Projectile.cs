using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float maxLifetime = 5f; // Time in seconds before the projectile destroys itself anyway

    void Start()
    {
        // Destroy the projectile after a set time to prevent clutter
        Destroy(gameObject, maxLifetime);
    }

    // This function is called when this collider/rigidbody has begun touching another rigidbody/collider
    void OnCollisionEnter(Collision collision)
    {
        // Check if the object we hit has the "TargetBlock" tag
        if (collision.gameObject.CompareTag("TargetBlock"))
        {
            // Try to get the TargetBlock component from the hit object
            TargetBlock target = collision.gameObject.GetComponent<TargetBlock>();
            if (target != null)
            {
                // Call the function on the target block to handle being hit
                target.HitByBlaster();
            }
            else
            {
                // Optional: Log if the tag is correct but the script is missing
                // Debug.LogWarning("Hit object tagged as TargetBlock, but missing TargetBlock script.", collision.gameObject);
            }

            // Destroy the projectile immediately after hitting a valid target
            Destroy(gameObject);
        }
        else
        {
            // Optional: If you want the projectile to be destroyed when hitting *anything* else (like walls)
            // Add other conditions here if needed (e.g., ignore hitting the player or other projectiles)
            // For simplicity, we destroy it on any collision that isn't the target (or maybe add a small delay)
            // Destroy(gameObject, 0.02f); // Small delay can help effects play out
            Destroy(gameObject); // Destroy immediately on hitting anything
        }
    }
}
