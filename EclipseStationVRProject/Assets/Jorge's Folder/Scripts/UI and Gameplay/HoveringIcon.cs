using UnityEngine;

public class HoveringIcon : MonoBehaviour
{
    public float hoverSpeed = 2f;
    public float hoverAmount = 0.1f;
    public float rotateSpeed = 45f;

    private Vector3 startPos;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startPos = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        float newY = Mathf.Sin(Time.time * hoverSpeed) * hoverAmount;
        transform.localPosition = startPos + new Vector3(0, newY, 0);

        //Optional Rotation
        transform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime);
    }
}
