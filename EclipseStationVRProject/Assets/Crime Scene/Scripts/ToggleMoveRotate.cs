using UnityEngine;
using UnityEngine.InputSystem;

public class ToggleMoveRotate : MonoBehaviour
{
    private bool toggled = false;

    private Vector3 originalPos;
    private Vector3 targetPos = new Vector3(0.877f, 1.218f, -1.493f);

    private Vector3 originalEuler;
    private Vector3 targetEuler;

    void Start()
    {
        originalPos = transform.position;
        originalEuler = transform.eulerAngles;

        targetEuler = new Vector3(
            originalEuler.x,
            originalEuler.y,
            originalEuler.z - 90f
        );
    }

    void Update()
    {
        if (Keyboard.current != null && Keyboard.current.digit3Key.wasPressedThisFrame)
        {
            Toggle();
        }
    }

    void Toggle()
    {
        if (!toggled)
        {
            transform.position = targetPos;
            transform.eulerAngles = targetEuler;
        }
        else
        {
            transform.position = originalPos;
            transform.eulerAngles = originalEuler;
        }

        toggled = !toggled;
    }
}
