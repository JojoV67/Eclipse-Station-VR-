using UnityEngine;

public class LaserProjectile : MonoBehaviour
{
    public float speed = 20f;
    public float lifeTime = 5f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("SpaceRock"))
        {
            Destroy(other.gameObject);
        }
        Destroy(gameObject);
    }
}
