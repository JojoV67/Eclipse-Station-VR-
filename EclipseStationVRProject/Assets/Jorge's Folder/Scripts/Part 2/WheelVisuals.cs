using UnityEngine;

public class WheelVisuals : MonoBehaviour
{
    [Header("Front Wheel Transforms")]
    public Transform frontLeftWheel;
    public Transform frontRightWheel;

    [Header("Visual Settings")]
    public float maxTurnAngle = 30f; // how far the wheels visually turn
    public float smoothSpeed = 5f;

    [Header("Wheel Spin")]
    public Transform rearLeftWheel;
    public Transform rearRightWheel;
    public float spinSpeedMultiplier = 300f;

    private float wheelSpinAngle;

    private float currentVisualAngle;

    public void UpdateWheelVisuals(float steerInput)
    {
        if (!frontLeftWheel || !frontRightWheel) return;

        float targetAngle = steerInput * maxTurnAngle;
        currentVisualAngle = Mathf.Lerp(currentVisualAngle, targetAngle, Time.deltaTime * smoothSpeed);

        frontLeftWheel.localRotation = Quaternion.Euler(0, currentVisualAngle, 0);
        frontRightWheel.localRotation = Quaternion.Euler(0, currentVisualAngle, 0);
    }

    public void SpinWheels(float speed)
    {
        wheelSpinAngle += speed * spinSpeedMultiplier * Time.deltaTime;

        if (frontLeftWheel) frontLeftWheel.Rotate(wheelSpinAngle, 0, 0);
        if (frontRightWheel) frontRightWheel.Rotate(wheelSpinAngle, 0, 0);
        if (rearLeftWheel) rearLeftWheel.Rotate(wheelSpinAngle, 0, 0);
        if (rearRightWheel) rearRightWheel.Rotate(wheelSpinAngle, 0, 0);
    }
}
