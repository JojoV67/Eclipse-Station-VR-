using UnityEngine;

public class CamisAntiGravityZone : MonoBehaviour
{
    public float maxHeight = 3.7f;       // Roof limit
    public float floatSpeed = 1.2f;      // Adjust to control float smoothness

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Transform player = other.transform;

            // Only float if player hasn't reached maxHeight yet
            if (player.position.y < maxHeight)
            {
                Vector3 upward = Vector3.up * floatSpeed * Time.deltaTime;
                player.Translate(upward, Space.World);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // You can reset flags here if needed in future
    }
}
