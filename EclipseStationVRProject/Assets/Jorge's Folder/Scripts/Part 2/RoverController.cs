/*using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit.Inputs;

[RequireComponent(typeof(Rigidbody))]
public class RoverController : MonoBehaviour
{
    [Header("Driving Settings")]
    public float acceleration = 15f;
    public float brakeForce = 20f;
    public float maxSpeed = 10f;
    public float turnSpeed = 45f;

    [Header("Input Actions")]
    public InputActionProperty accelerateAction; // Right trigger
    public InputActionProperty brakeAction;      // Left trigger
    public InputActionProperty steerAction;      // Left joystick (Vector2)

    [Header("Audio")]
    public AudioSource engineSource;
    public AudioSource brakeSource;
    public AudioClip engineLoop;
    public AudioClip brakeClip;
    public AudioClip boostSound;
    [Range(0.5f, 2f)] public float enginePitchMin = 0.8f;
    [Range(0.5f, 2f)] public float enginePitchMax = 1.5f;
    public float fadeSpeed = 2f;
    public float maxEngineVolume = 0.6f;

    [Header("Boost Settings")]
    public float boostForce = 30f;
    public float boostDuration = 2f;
    public float boostCooldown = 5f;
    public float boostMeter = 100f;   // 0–100
    public float boostUsageRate = 50f; // how fast it drains
    public float boostRegenPerBattery = 33.3f;

    [Header("Boost UI")]
    public Slider boostSlider;
    public float boostUISmoothSpeed = 3f;
    private float targetBoostValue;

    private bool boosting = false;
    private bool canBoost => boostMeter > 0f;

    public InputActionProperty boostAction; // Assign to B button

    private Rigidbody rb;
    private bool isBraking;
    private float targetEngineVolume;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (boostSlider)
        {
            boostSlider.value = boostMeter / 100f;
            targetBoostValue = boostSlider.value;
        }

        if (engineSource && engineLoop)
        {
            engineSource.clip = engineLoop;
            engineSource.loop = true;
            engineSource.volume = 0f; // start silent
            engineSource.Play();
        }
    }

    void FixedUpdate()
    {
        float accel = accelerateAction.action?.ReadValue<float>() ?? 0f;
        float brake = brakeAction.action?.ReadValue<float>() ?? 0f;
        Vector2 steer = steerAction.action?.ReadValue<Vector2>() ?? Vector2.zero;

        // Forward acceleration
        if (accel > 0.1f && rb.linearVelocity.magnitude < maxSpeed)
            rb.AddForce(transform.forward * accel * acceleration, ForceMode.Acceleration);

        // Reverse / brake
        if (brake > 0.1f)
        {
            rb.AddForce(-transform.forward * brake * brakeForce, ForceMode.Acceleration);

            if (!isBraking && brakeSource && brakeClip)
            {
                brakeSource.PlayOneShot(brakeClip);
                isBraking = true;
            }
        }
        else
        {
            isBraking = false;
        }

        // Steering (left stick X-axis)
        float turn = steer.x * turnSpeed * Time.fixedDeltaTime;
        transform.Rotate(0f, turn, 0f);

        // --- Boost Input ---
        /*if (Keyboard.current != null && Keyboard.current.bKey.wasPressedThisFrame) // For PC testing
        {
            StartCoroutine(Boost());
        }
        else if (boostAction.action.WasPressedThisFrame())
        {
            StartCoroutine(Boost());
        }*/
/*if (!boosting)
{
    // For PC testing
    if ((Keyboard.current != null && Keyboard.current.bKey.wasPressedThisFrame) ||
        boostAction.action.WasPressedThisFrame())
    {
        if (canBoost)
            StartCoroutine(Boost());
    }
}

bool boostPressed = false;

// Check both PC key and Quest controller input
if (Keyboard.current != null && Keyboard.current.bKey.isPressed)
    boostPressed = true;
else if (boostAction.action.IsPressed())
    boostPressed = true;

if (boostPressed && canBoost && !boosting)
{
    StartCoroutine(Boost());
}


// Engine sound pitch scaling with speed
if (engineSource)
{
    float speedPercent = Mathf.Clamp01(rb.linearVelocity.magnitude / maxSpeed);
    float accelOrBrake = Mathf.Max(accel, brake);

    // Smooth pitch based on speed
    engineSource.pitch = Mathf.Lerp(enginePitchMin, enginePitchMax, speedPercent);

    // Target volume increases with acceleration/speed
    targetEngineVolume = Mathf.Lerp(0.1f, maxEngineVolume, Mathf.Max(speedPercent, accelOrBrake));

    // Smoothly fade towards target volume
    engineSource.volume = Mathf.MoveTowards(engineSource.volume, targetEngineVolume, fadeSpeed * Time.fixedDeltaTime);
    //engineSource.pitch = Mathf.Lerp(enginePitchMin, enginePitchMax, speedPercent);
}
}

void Update()
{
if (boostSlider)
{
    boostSlider.value = Mathf.Lerp(boostSlider.value, targetBoostValue, boostUISmoothSpeed * Time.deltaTime);
}
}

public void UpdateBoostUI()
{
/*if (boostSlider)
    boostSlider.value = boostMeter / 100f;

if (!boostSlider) return;

targetBoostValue = boostMeter / 100f;
}

private IEnumerator Boost()
{
/*if (boosting || !canBoost)
    yield break;

boosting = true;
float elapsed = 0f;

while (elapsed < boostDuration && boostMeter > 0f)
{
    rb.AddForce(transform.forward * boostForce, ForceMode.Acceleration);
    boostMeter -= boostUsageRate * Time.deltaTime;

    UpdateBoostUI();

    elapsed += Time.deltaTime;
    yield return null;
}

boosting = false;*/

/*if (boosting || !canBoost)
    yield break;

boosting = true;

// Play boost sound once
if (engineSource && !engineSource.isPlaying)
    engineSource.Play(); // optional, if you have a separate source for boost use that

if (boostSound)
    AudioSource.PlayClipAtPoint(boostSound, transform.position, 1f);

while (boostMeter > 0f)
{
    // Apply boost acceleration
    rb.AddForce(transform.forward * boostForce, ForceMode.Acceleration);

    // Drain the boost meter
    boostMeter -= boostUsageRate * Time.deltaTime;

    // Clamp so it never goes negative
    boostMeter = Mathf.Max(boostMeter, 0f);

    // Update the UI smoothly
    UpdateBoostUI();

    // Stop boosting if the meter hits zero
    if (boostMeter <= 0.01f)
        break;

    yield return null;
}

// Once boost meter is empty, stop boosting
boosting = false;
yield return new WaitForSeconds(boostCooldown);
boostMeter = Mathf.Clamp(boostMeter, 0f, 100f);
UpdateBoostUI();


}
}*/

using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit.Inputs;

[RequireComponent(typeof(Rigidbody))]
public class RoverController : MonoBehaviour
{
    [Header("Driving Settings")]
    public float acceleration = 15f;
    public float brakeForce = 20f;
    public float maxSpeed = 10f;
    public float turnSpeed = 45f;

    [Header("Input Actions")]
    public InputActionProperty accelerateAction; // Right trigger
    public InputActionProperty brakeAction;      // Left trigger
    public InputActionProperty steerAction;      // Left joystick (Vector2)
    public InputActionProperty boostAction;      // B button

    [Header("Audio")]
    public AudioSource engineSource;
    public AudioSource brakeSource;
    public AudioClip engineLoop;
    public AudioClip brakeClip;
    public AudioClip boostSound;
    [Range(0.5f, 2f)] public float enginePitchMin = 0.8f;
    [Range(0.5f, 2f)] public float enginePitchMax = 1.5f;
    public float fadeSpeed = 2f;
    public float maxEngineVolume = 0.6f;

    [Header("Boost Settings")]
    public float boostForce = 30f;
    public float boostUsageRate = 50f; // how fast it drains
    public float boostMeter = 100f;    // 0–100
    public float boostRegenPerBattery = 33.3f;

    [Header("Boost UI")]
    public Slider boostSlider;
    public float boostUISmoothSpeed = 3f;
    private float targetBoostValue;

    [Header("Steering Wheel (Optional)")]
    public Transform steeringWheel;
    public float maxSteerAngle = 45f;

    private bool boosting = false;
    private bool canBoost => boostMeter > 0f;

    private Rigidbody rb;
    private bool isBraking;
    private float targetEngineVolume;
    private float currentSteerInput;

    // For terrain modifiers
    private float originalAcceleration;
    private float originalMaxSpeed;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.maxAngularVelocity = 5f;

        originalAcceleration = acceleration;
        originalMaxSpeed = maxSpeed;

        if (boostSlider)
        {
            boostSlider.value = boostMeter / 100f;
            targetBoostValue = boostSlider.value;
        }

        if (engineSource && engineLoop)
        {
            engineSource.clip = engineLoop;
            engineSource.loop = true;
            engineSource.volume = 0f;
            engineSource.Play();
        }
    }

    void FixedUpdate()
    {
        float accel = accelerateAction.action?.ReadValue<float>() ?? 0f;
        float brake = brakeAction.action?.ReadValue<float>() ?? 0f;

        // Handle steering input
        if (steeringWheel != null)
        {
            float steerAngle = steeringWheel.localEulerAngles.z;
            if (steerAngle > 180f) steerAngle -= 360f;
            currentSteerInput = Mathf.Clamp(steerAngle / maxSteerAngle, -1f, 1f);
        }
        else
        {
            Vector2 steer = steerAction.action?.ReadValue<Vector2>() ?? Vector2.zero;
            currentSteerInput = steer.x;
        }

        // Movement
        if (accel > 0.1f && rb.linearVelocity.magnitude < maxSpeed)
            rb.AddForce(transform.forward * accel * acceleration, ForceMode.Acceleration);

        if (brake > 0.1f)
        {
            rb.AddForce(-transform.forward * brake * brakeForce, ForceMode.Acceleration);

            if (!isBraking && brakeSource && brakeClip)
            {
                brakeSource.PlayOneShot(brakeClip);
                isBraking = true;
            }
        }
        else
        {
            isBraking = false;
        }

        // Steering
        float turn = currentSteerInput * turnSpeed * Time.fixedDeltaTime;
        transform.Rotate(0f, turn, 0f);

        // --- Boost Input ---
        bool boostPressed = false;
        if (Keyboard.current != null && Keyboard.current.bKey.isPressed)
            boostPressed = true;
        else if (boostAction.action.IsPressed())
            boostPressed = true;

        if (boostPressed && canBoost && !boosting)
        {
            StartCoroutine(Boost());
        }

        // Engine audio scaling
        if (engineSource)
        {
            float speedPercent = Mathf.Clamp01(rb.linearVelocity.magnitude / maxSpeed);
            float accelOrBrake = Mathf.Max(accel, brake);

            engineSource.pitch = Mathf.Lerp(enginePitchMin, enginePitchMax, speedPercent);
            targetEngineVolume = Mathf.Lerp(0.1f, maxEngineVolume, Mathf.Max(speedPercent, accelOrBrake));
            engineSource.volume = Mathf.MoveTowards(engineSource.volume, targetEngineVolume, fadeSpeed * Time.fixedDeltaTime);
        }
    }

    void Update()
    {
        if (boostSlider)
        {
            boostSlider.value = Mathf.Lerp(boostSlider.value, targetBoostValue, boostUISmoothSpeed * Time.deltaTime);
        }
    }

    public void UpdateBoostUI()
    {
        if (!boostSlider) return;
        targetBoostValue = boostMeter / 100f;
    }

    private IEnumerator Boost()
    {
        boosting = true;

        if (boostSound)
            AudioSource.PlayClipAtPoint(boostSound, transform.position, 0.8f);

        while ((Keyboard.current != null && Keyboard.current.bKey.isPressed || boostAction.action.IsPressed()) && boostMeter > 0f)
        {
            rb.AddForce(transform.forward * boostForce, ForceMode.Acceleration);
            boostMeter -= boostUsageRate * Time.deltaTime;
            boostMeter = Mathf.Max(boostMeter, 0f);
            UpdateBoostUI();
            yield return null;
        }

        boosting = false;
        UpdateBoostUI();
    }

    // --- Swamp slow-down ---
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("SlowTerrain"))
        {
            acceleration *= 0.5f;
            maxSpeed *= 0.6f;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("SlowTerrain"))
        {
            acceleration = originalAcceleration;
            maxSpeed = originalMaxSpeed;
        }
    }
}


