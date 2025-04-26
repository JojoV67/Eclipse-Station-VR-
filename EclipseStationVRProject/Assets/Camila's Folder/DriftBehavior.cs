using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
public class DriftBehavior : MonoBehaviour
{
    public float driftForce = 0.05f;    // fuerza suave continua
    private Vector3 driftDir;
    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        driftDir = Random.onUnitSphere; // dirección fija por cubo
    }

    void FixedUpdate()
    {
        if (!rb.useGravity)
            rb.AddForce(driftDir * driftForce, ForceMode.Acceleration);
    }
}
