using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class SteeringWheelController : MonoBehaviour
{
    public Transform wheelCenter; // pivot for rotation
    public float maxRotation = 90f; // how far it can turn left/right

    private float currentAngle = 0f;
    private Quaternion initialRotation;
    private XRGrabInteractable grabInteractable;

    void Start()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        initialRotation = transform.localRotation;

        grabInteractable.selectEntered.AddListener(OnGrab);
        grabInteractable.selectExited.AddListener(OnRelease);
    }

    void OnGrab(SelectEnterEventArgs args) { }
    void OnRelease(SelectExitEventArgs args)
    {
        // Smoothly reset to center when released
        StartCoroutine(ResetWheel());
    }

    void Update()
    {
        if (grabInteractable.isSelected)
        {
            // Read hand rotation relative to the wheel
            Quaternion localRot = transform.localRotation;
            float zAngle = localRot.eulerAngles.z;
            if (zAngle > 180) zAngle -= 360;
            currentAngle = Mathf.Clamp(zAngle, -maxRotation, maxRotation);
        }
    }

    IEnumerator ResetWheel()
    {
        float elapsed = 0f;
        while (elapsed < 0.5f)
        {
            transform.localRotation = Quaternion.Lerp(transform.localRotation, initialRotation, elapsed / 0.5f);
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.localRotation = initialRotation;
        currentAngle = 0f;
    }

    public float GetSteerInput()
    {
        return Mathf.Clamp(currentAngle / maxRotation, -1f, 1f);
    }
}
