using UnityEngine;
using System.Collections;

public class ZeroGravityZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var rb = other.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.useGravity = false;
                rb.linearVelocity = Vector3.zero;
            }

            // Wait one frame before disabling the CharacterController
            StartCoroutine(DisableControllerNextFrame(other));
        }
    }

    private IEnumerator DisableControllerNextFrame(Collider other)
    {
        yield return null; // wait one frame

        var cc = other.GetComponent<CharacterController>();
        if (cc != null && cc.enabled)
            cc.enabled = false;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var cc = other.GetComponent<CharacterController>();
            if (cc != null && !cc.enabled)
                cc.enabled = true;

            var rb = other.GetComponent<Rigidbody>();
            if (rb != null)
                rb.useGravity = true;
        }
    }
}
