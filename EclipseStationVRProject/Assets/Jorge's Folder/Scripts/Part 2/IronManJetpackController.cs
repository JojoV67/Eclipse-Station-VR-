/*using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using TMPro;
using System;

[RequireComponent(typeof(Rigidbody))]
public class IronManJetpackController : MonoBehaviour
{
    [Header("References")]
    public Transform leftHand;
    public Transform rightHand;
    public InputActionProperty leftJetAction;
    public InputActionProperty rightJetAction;
    public ParticleSystem leftThrusterFX;
    public ParticleSystem rightThrusterFX;
    public AudioSource jetAudio;

    [Header("UI Elements")]
    public Slider fuelBar;
    public TMP_Text fuelText;
    public Image fuelFill;

    [Header("Settings")]
    public float thrustForce = 20f;
    public float maxSpeed = 10f;
    public float rotationSmoothing = 5f;
    public float fuel = 100f;
    public float fuelDrainRate = 10f;
    public float fuelRechargeRate = 8f;
    public float hoverStabilization = 0.98f;

    private Rigidbody rb;
    private bool isThrusting;
    private float lastThrustPower;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = true;
        rb.linearDamping = 1f;

        if (jetAudio != null)
        {
            jetAudio.loop = true;
            jetAudio.spatialBlend = 1f;
        }
        UpdateFuelUI();
    }

    void FixedUpdate()
    {
        float leftThrust = leftJetAction.action?.ReadValue<float>() ?? 0f;
        float rightThrust = rightJetAction.action?.ReadValue<float>() ?? 0f;
        float totalThrust = Mathf.Clamp01((leftThrust + rightThrust) * 0.5f);

        bool usingJets = totalThrust > 0.05f && fuel > 0f;

        if (usingJets)
        {
            // Drain fuel
            fuel -= fuelDrainRate * Time.fixedDeltaTime;
            fuel = Mathf.Max(0f, fuel);

            // Apply thrust in direction of both hands
            Vector3 combinedDir = ((leftHand.forward + rightHand.forward) * 0.5f).normalized;
            rb.AddForce(combinedDir * thrustForce, ForceMode.Acceleration);

            // Limit speed
            rb.linearVelocity = Vector3.ClampMagnitude(rb.linearVelocity, maxSpeed);

            // Light stabilization
            rb.linearVelocity *= hoverStabilization;

            // Effects
            if (leftThrusterFX && !leftThrusterFX.isPlaying) leftThrusterFX.Play();
            if (rightThrusterFX && !rightThrusterFX.isPlaying) rightThrusterFX.Play();

            if (jetAudio && !jetAudio.isPlaying) jetAudio.Play();
            jetAudio.volume = Mathf.Lerp(jetAudio.volume, totalThrust, 0.1f);
            jetAudio.pitch = 1f + totalThrust * 0.5f;
        }
        else
        {
            // Stop effects
            if (leftThrusterFX) leftThrusterFX.Stop();
            if (rightThrusterFX) rightThrusterFX.Stop();

            if (jetAudio) jetAudio.volume = Mathf.Lerp(jetAudio.volume, 0f, 0.05f);

            // Recharge fuel
            fuel += fuelRechargeRate * Time.fixedDeltaTime;
            fuel = Mathf.Min(fuel, 100f);
        }
        UpdateFuelUI();
    }

    private void UpdateFuelUI()
    {
        if (fuelBar != null)
            fuelBar.value = fuel;

        if (fuelText != null)
            fuelText.text = $"Fuel: {Mathf.RoundToInt(fuel)}%";

        if (fuelFill != null)
        {
            Color newColor = Color.Lerp(Color.red, Color.green, fuel / 100f);
            fuelFill.color = newColor;
        }
    }
}*/

/*using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR;

[RequireComponent(typeof(Rigidbody))]
public class IronManJetpackController : MonoBehaviour
{
    [Header("References")]
    public Transform leftHand;
    public Transform rightHand;
    public InputActionProperty leftJetAction;
    public InputActionProperty rightJetAction;
    public ParticleSystem leftThrusterFX;
    public ParticleSystem rightThrusterFX;
    public AudioSource jetAudio;
    public AudioSource warningAudio; // Optional low-fuel beep
    private UnityEngine.XR.InputDevice leftDevice;
    private UnityEngine.XR.InputDevice rightDevice;

    [Header("UI Elements")]
    public Slider fuelBar;
    public TMP_Text fuelText;
    public Image fuelFill;

    [Header("Settings")]
    public float thrustForce = 20f;
    public float maxSpeed = 10f;
    public float rotationSmoothing = 5f;
    public float fuel = 100f;
    public float fuelDrainRate = 10f;
    public float fuelRechargeRate = 8f;
    public float hoverStabilization = 0.98f;
    public float lowFuelThreshold = 15f;
    public float flashSpeed = 4f;

    private Rigidbody rb;
    private bool lowFuelActive;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = true;
        rb.linearDamping = 1f;

        // Get the devices
        leftDevice = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
        rightDevice = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);

        if (jetAudio != null)
        {
            jetAudio.loop = true;
            jetAudio.spatialBlend = 1f;
        }

        if (warningAudio != null)
        {
            warningAudio.loop = true;
            warningAudio.spatialBlend = 0f; // 2D sound
        }

        UpdateFuelUI();
    }

    private void SendHaptics(UnityEngine.XR.InputDevice device, float amplitude)
    {
        if (device.isValid)
            device.SendHapticImpulse(0u, amplitude, Time.fixedDeltaTime * 2f);
    }

    private void StopHaptics(UnityEngine.XR.InputDevice device)
    {
        if (device.isValid)
            device.SendHapticImpulse(0u, 0f, 0f);
    }


    void FixedUpdate()
    {
        float leftThrust = leftJetAction.action?.ReadValue<float>() ?? 0f;
        float rightThrust = rightJetAction.action?.ReadValue<float>() ?? 0f;
        float totalThrust = Mathf.Clamp01((leftThrust + rightThrust) * 0.5f);

        bool usingJets = totalThrust > 0.05f && fuel > 0f;

        if (usingJets)
        {
            fuel -= fuelDrainRate * Time.fixedDeltaTime;
            fuel = Mathf.Max(0f, fuel);

            // ? Use downward palms (Iron Man style)
            Vector3 combinedDir = ((-leftHand.up) + (-rightHand.up)) * 0.5f;
            rb.AddForce(combinedDir.normalized * thrustForce, ForceMode.Acceleration);

            rb.linearVelocity = Vector3.ClampMagnitude(rb.linearVelocity, maxSpeed);
            rb.linearVelocity *= hoverStabilization;

            // FX & audio
            if (leftThrusterFX && !leftThrusterFX.isPlaying) leftThrusterFX.Play();
            if (rightThrusterFX && !rightThrusterFX.isPlaying) rightThrusterFX.Play();

            if (jetAudio && !jetAudio.isPlaying) jetAudio.Play();
            jetAudio.volume = Mathf.Lerp(jetAudio.volume, totalThrust, 0.1f);
            jetAudio.pitch = 1f + totalThrust * 0.5f;
        }
        else
        {
            if (leftThrusterFX) leftThrusterFX.Stop();
            if (rightThrusterFX) rightThrusterFX.Stop();

            if (jetAudio) jetAudio.volume = Mathf.Lerp(jetAudio.volume, 0f, 0.05f);

            fuel += fuelRechargeRate * Time.fixedDeltaTime;
            fuel = Mathf.Min(fuel, 100f);
        }

        UpdateFuelUI();
        HandleLowFuelWarning();
    }

    private void UpdateFuelUI()
    {
        if (fuelBar != null)
            fuelBar.value = fuel;

        if (fuelText != null)
            fuelText.text = $"Fuel: {Mathf.RoundToInt(fuel)}%";

        if (fuelFill != null)
        {
            Color newColor = Color.Lerp(Color.red, Color.green, fuel / 100f);
            fuelFill.color = newColor;
        }
    }

    private void HandleLowFuelWarning()
    {
        bool lowFuel = fuel <= lowFuelThreshold;

        if (lowFuel && !lowFuelActive)
        {
            lowFuelActive = true;
            if (warningAudio && !warningAudio.isPlaying)
                warningAudio.Play();
        }
        else if (!lowFuel && lowFuelActive)
        {
            lowFuelActive = false;
            if (warningAudio)
                warningAudio.Stop();
        }

        // Flash the UI fill color
        if (lowFuel && fuelFill != null)
        {
            float flash = Mathf.Abs(Mathf.Sin(Time.time * flashSpeed));
            fuelFill.color = Color.Lerp(Color.red, Color.white, flash);
        }
    }
}*/

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;
using UnityEngine.XR;

[RequireComponent(typeof(Rigidbody))]
public class IronManJetpackController : MonoBehaviour
{
    [Header("References")]
    public Transform leftHand;
    public Transform rightHand;
    public InputActionProperty leftJetAction;
    public InputActionProperty rightJetAction;
    public InputActionProperty moveAction;
    public ParticleSystem leftThrusterFX;
    public ParticleSystem rightThrusterFX;
    public AudioSource jetAudio;
    public AudioSource warningAudio;

    [Header("UI Elements")]
    public Slider fuelBar;
    public TMP_Text fuelText;
    public Image fuelFill;

    [Header("Settings")]
    public float walkSpeed = 3f;
    public float thrustForce = 20f;
    public float airControlMultiplier = 1.5f;
    public float maxSpeed = 10f;
    public float hoverStabilization = 0.98f;
    public float fuel = 100f;
    public float fuelDrainRate = 10f;
    public float fuelRechargeRate = 8f;
    public float groundCheckDistance = 0.2f;
    public LayerMask groundLayer;
    public float lowFuelThreshold = 15f;
    public float flashSpeed = 4f;

    private Rigidbody rb;
    private bool isGrounded;
    private bool lowFuelActive;

    private UnityEngine.XR.InputDevice leftDevice;
    private UnityEngine.XR.InputDevice rightDevice;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = true;
        rb.linearDamping = 1f;

        leftDevice = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
        rightDevice = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);

        if (jetAudio != null)
        {
            jetAudio.loop = true;
            jetAudio.spatialBlend = 1f;
        }

        if (warningAudio != null)
        {
            warningAudio.loop = true;
            warningAudio.spatialBlend = 0f;
        }

        UpdateFuelUI();
    }

    void FixedUpdate()
    {
        CheckGrounded();

        float leftThrust = leftJetAction.action?.ReadValue<float>() ?? 0f;
        float rightThrust = rightJetAction.action?.ReadValue<float>() ?? 0f;
        float totalThrust = Mathf.Clamp01((leftThrust + rightThrust) * 0.5f);

        bool usingJets = totalThrust > 0.05f && fuel > 0f;

        if (isGrounded && !usingJets)
        {
            HandleGroundMovement();
        }
        else
        {
            HandleAirMovement(leftThrust, rightThrust, totalThrust, usingJets);
        }

        UpdateFuelUI();
        HandleLowFuelWarning();
    }

    private void CheckGrounded()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, groundCheckDistance, groundLayer);
    }

    private void HandleGroundMovement()
    {
        Vector2 moveInput = moveAction.action?.ReadValue<Vector2>() ?? Vector2.zero;
        Vector3 move = new Vector3(moveInput.x, 0, moveInput.y);
        move = transform.TransformDirection(move);
        rb.MovePosition(rb.position + move * walkSpeed * Time.fixedDeltaTime);
    }

    private void HandleAirMovement(float leftThrust, float rightThrust, float totalThrust, bool usingJets)
    {
        /*if (usingJets)
        {
            fuel -= fuelDrainRate * Time.fixedDeltaTime;
            fuel = Mathf.Max(0f, fuel);

            // Calculate combined direction
            Vector3 leftDir = -leftHand.up;
            Vector3 rightDir = -rightHand.up;
            Vector3 combinedDir = (leftDir + rightDir).normalized;

            // Apply thrust force
            rb.AddForce(combinedDir * thrustForce, ForceMode.Acceleration);

            // Keep velocity stable and smooth
            rb.linearVelocity = Vector3.ClampMagnitude(rb.linearVelocity, maxSpeed);
            rb.linearVelocity *= hoverStabilization;

            // Play FX & sound
            if (leftThrusterFX && !leftThrusterFX.isPlaying) leftThrusterFX.Play();
            if (rightThrusterFX && !rightThrusterFX.isPlaying) rightThrusterFX.Play();

            if (jetAudio && !jetAudio.isPlaying) jetAudio.Play();
            jetAudio.volume = Mathf.Lerp(jetAudio.volume, totalThrust, 0.1f);
            jetAudio.pitch = 1f + totalThrust * 0.5f;

            // Haptics
            SendHaptics(leftDevice, leftThrust);
            SendHaptics(rightDevice, rightThrust);
        }
        else
        {
            // Refill fuel slowly
            fuel += fuelRechargeRate * Time.fixedDeltaTime;
            fuel = Mathf.Min(fuel, 100f);

            if (leftThrusterFX) leftThrusterFX.Stop();
            if (rightThrusterFX) rightThrusterFX.Stop();

            if (jetAudio) jetAudio.volume = Mathf.Lerp(jetAudio.volume, 0f, 0.05f);

            StopHaptics(leftDevice);
            StopHaptics(rightDevice);
        }*/

        //Update 10/23/2025
        if (usingJets)
        {
            fuel -= fuelDrainRate * Time.fixedDeltaTime;
            fuel = Mathf.Max(0f, fuel);

            // --- Calculate thrust direction based on hand tilt ---
            // Hands pointing down push you up; pointing back pushes you forward
            Vector3 leftDir = -leftHand.up;
            Vector3 rightDir = -rightHand.up;
            Vector3 combinedDir = (leftDir + rightDir).normalized;

            // Slightly favor forward motion if player tilts arms backward
            float forwardInfluence = Vector3.Dot(combinedDir, transform.forward);
            Vector3 forwardBoost = transform.forward * Mathf.Clamp01(forwardInfluence) * 1f;

            // Blend vertical and forward thrusts
            Vector3 thrustDirection = (combinedDir + forwardBoost).normalized;

            // Apply force more naturally
            rb.AddForce(thrustDirection * thrustForce, ForceMode.Acceleration);

            // Maintain some momentum instead of cutting it
            rb.linearVelocity = Vector3.Lerp(rb.linearVelocity,
                Vector3.ClampMagnitude(rb.linearVelocity, maxSpeed),
                Time.fixedDeltaTime * hoverStabilization);

            // Jet sound & FX
            if (leftThrusterFX && !leftThrusterFX.isPlaying) leftThrusterFX.Play();
            if (rightThrusterFX && !rightThrusterFX.isPlaying) rightThrusterFX.Play();

            if (jetAudio && !jetAudio.isPlaying) jetAudio.Play();
            jetAudio.volume = Mathf.Lerp(jetAudio.volume, totalThrust, 0.1f);
            jetAudio.pitch = 1f + totalThrust * 0.5f;

            SendHaptics(leftDevice, leftThrust);
            SendHaptics(rightDevice, rightThrust);

            // --- Calculate thrust direction based on hand orientation ---
            /*Vector3 leftDown = -leftHand.up;      // down direction of left hand
            Vector3 rightDown = -rightHand.up;    // down direction of right hand

            // Calculate how much the hands are tilted backward or forward relative to the player's forward
            float leftForwardFactor = Vector3.Dot(leftHand.forward, Vector3.up);   // if pointing forward, this is small
            float rightForwardFactor = Vector3.Dot(rightHand.forward, Vector3.up);

            // Base lift direction (downward palms = up force)
            Vector3 liftDir = (leftDown + rightDown).normalized;

            // Add forward influence when hands tilt backward
            Vector3 forwardDir = transform.forward * Mathf.Clamp01((-leftForwardFactor - rightForwardFactor) * 0.5f);

            // Combine the two for Iron-Man-like control
            Vector3 thrustDirection = (liftDir + forwardDir * 1.2f).normalized;

            // Apply the force
            rb.AddForce(thrustDirection * thrustForce, ForceMode.Acceleration);

            // Smooth velocity (preserve forward motion)
            Vector3 horizontalVel = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
            horizontalVel = Vector3.ClampMagnitude(horizontalVel, maxSpeed);
            rb.linearVelocity = new Vector3(horizontalVel.x, rb.linearVelocity.y, horizontalVel.z);*/
        }
        else
        {
            // Refill fuel slowly
            fuel += fuelRechargeRate * Time.fixedDeltaTime;
            fuel = Mathf.Min(fuel, 100f);

            if (leftThrusterFX) leftThrusterFX.Stop();
            if (rightThrusterFX) rightThrusterFX.Stop();

            if (jetAudio) jetAudio.volume = Mathf.Lerp(jetAudio.volume, 0f, 0.05f);

            StopHaptics(leftDevice);
            StopHaptics(rightDevice);
        }
    }

    private void SendHaptics(UnityEngine.XR.InputDevice device, float amplitude)
    {
        if (device.isValid)
            device.SendHapticImpulse(0u, amplitude, Time.fixedDeltaTime * 2f);
    }

    private void StopHaptics(UnityEngine.XR.InputDevice device)
    {
        if (device.isValid)
            device.SendHapticImpulse(0u, 0f, 0f);
    }

    private void UpdateFuelUI()
    {
        if (fuelBar != null)
            fuelBar.value = fuel;

        if (fuelText != null)
            fuelText.text = $"Fuel: {Mathf.RoundToInt(fuel)}%";

        if (fuelFill != null)
        {
            Color newColor = Color.Lerp(Color.red, Color.green, fuel / 100f);
            fuelFill.color = newColor;
        }
    }

    private void HandleLowFuelWarning()
    {
        bool lowFuel = fuel <= lowFuelThreshold;

        if (lowFuel && !lowFuelActive)
        {
            lowFuelActive = true;
            if (warningAudio && !warningAudio.isPlaying)
                warningAudio.Play();
        }
        else if (!lowFuel && lowFuelActive)
        {
            lowFuelActive = false;
            if (warningAudio)
                warningAudio.Stop();
        }

        if (lowFuel && fuelFill != null)
        {
            float flash = Mathf.Abs(Mathf.Sin(Time.time * flashSpeed));
            fuelFill.color = Color.Lerp(Color.red, Color.white, flash);
        }
    }
}


