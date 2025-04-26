using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ZeroGravityZone : MonoBehaviour
{
    [Header("Fuerza inicial al entrar")]
    public float initialImpulse = 1f;
    [Header("Factor de drift hacia el centro")]
    public float driftStrength = 0.2f;

    private Vector3 zoneCenter;

    void Start()
    {
        // Guarda la posición del centro de tu zona trigger
        zoneCenter = transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        var rb = other.attachedRigidbody;
        if (rb == null) return;

        // Desactiva gravedad y limpia velocidad
        rb.useGravity = false;
        rb.linearVelocity = Vector3.zero;

        // Calcula dirección hacia arriba+centro
        Vector3 toCenter = (zoneCenter - other.transform.position).normalized;
        Vector3 upward = Vector3.up;
        Vector3 forceDir = (toCenter + upward * 0.5f).normalized;

        // Impulso combinado
        rb.AddForce(forceDir * initialImpulse, ForceMode.Impulse);
    }

    private void OnTriggerStay(Collider other)
    {
        var rb = other.attachedRigidbody;
        if (rb == null) return;

        // Suave drift constante hacia el centro para que no toque paredes
        Vector3 toCenter = (zoneCenter - other.transform.position).normalized;
        rb.AddForce(toCenter * driftStrength, ForceMode.Acceleration);
    }

    private void OnTriggerExit(Collider other)
    {
        var rb = other.attachedRigidbody;
        if (rb != null)
            rb.useGravity = true;
    }
}
