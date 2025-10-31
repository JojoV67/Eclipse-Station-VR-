/*using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;
using System.Collections;

/// <summary>
/// Controls the Iron Man-style jetpack flight, fuel management, and crystal collection.
/// This script should be attached to the GameObject that holds the CharacterController
/// (usually the 'Body' or 'CharacterController' GameObject underneath the XR Origin).
/// </summary>
[RequireComponent(typeof(CharacterController))]
public class JetpackIronManControls : MonoBehaviour
{
    // --- UNITY COMPONENT REFERENCES ---
    [Header("Controller Setup")]
    // Assign these Transforms to your Left Hand and Right Hand controller GameObjects (e.g., LeftHandController/RightHandController).
    public Transform leftController;
    public Transform rightController;
    private CharacterController characterController;

    [Header("Input Actions")]
    // Assign these to your Left and Right Trigger/Grip Input Actions (e.g., "PrimaryButton" or "Trigger" actions).
    public InputActionReference leftTriggerAction;
    public InputActionReference rightTriggerAction;

    // --- JETPACK MOVEMENT PARAMETERS ---
    [Header("Flight Parameters")]
    public float maxThrustForce = 15f;
    public float maxDecelerationForce = 20f;
    public float forwardFlightMultiplier = 1.5f;
    public float gravity = 9.81f;
    public float drag = 0.5f;

    // Internal state
    private Vector3 currentVelocity = Vector3.zero;
    private bool isFlying = false;

    // --- FUEL SYSTEM PARAMETERS ---
    [Header("Fuel System")]
    public float maxFuel = 100f;
    public float fuelConsumptionRate = 30f; // Fuel consumed per second when flying
    public float fuelRechargeRate = 10f; // Fuel recharged per second when not flying
    public float lowFuelThreshold = 25f; // Red threshold
    public float midFuelThreshold = 50f; // Yellow threshold
    private float currentFuel;
    private bool triggersHeld = false;
    private Coroutine fuelWarningCoroutine;

    // --- UI/SFX REFERENCES ---
    [Header("UI & SFX")]
    // Assign these in the Inspector to your Canvas elements
    public Slider fuelSlider;
    public TMP_Text fuelText;
    public TMP_Text crystalCounterText;
    // NOTE: For SFX, you'd typically have an AudioSource component and an AudioClip reference.
    // For simplicity in this code block, we'll use a Debug.Log for the sound warning.
    public AudioSource sfxSource;
    public AudioClip lowFuelWarningSound;

    // --- GAME STATE ---
    private int crystalsCollected = 0;
    private readonly string CrystalTag = "Crystal"; // Ensure your collectables have this tag

    void Awake()
    {
        characterController = GetComponent<CharacterController>();
        currentFuel = maxFuel;
        UpdateFuelUI();
        UpdateCrystalUI();
    }

    void OnEnable()
    {
        // Subscribe to trigger events
        leftTriggerAction.action.performed += OnTriggerChange;
        leftTriggerAction.action.canceled += OnTriggerChange;
        rightTriggerAction.action.performed += OnTriggerChange;
        rightTriggerAction.action.canceled += OnTriggerChange;
    }

    void OnDisable()
    {
        // Unsubscribe from trigger events
        leftTriggerAction.action.performed -= OnTriggerChange;
        leftTriggerAction.action.canceled -= OnTriggerChange;
        rightTriggerAction.action.performed -= OnTriggerChange;
        rightTriggerAction.action.canceled -= OnTriggerChange;
    }

    // This handles trigger press/release and determines if the jetpack is active
    private void OnTriggerChange(InputAction.CallbackContext context)
    {
        bool leftHeld = leftTriggerAction.action.IsPressed();
        bool rightHeld = rightTriggerAction.action.IsPressed();

        triggersHeld = leftHeld && rightHeld;
    }

    void Update()
    {
        // Handle Fuel Consumption and Recharge
        HandleFuel();

        // Handle Flight State
        if (triggersHeld && currentFuel > 0.01f)
        {
            isFlying = true;
            ApplyJetpackThrust();
        }
        else
        {
            // Only stop flying if we were flying previously or if we run out of fuel
            if (isFlying)
            {
                isFlying = false;
                // Transition: Apply final momentum but re-enable gravity/ground logic
            }
        }
    }

    void FixedUpdate()
    {
        // Apply Drag (air resistance)
        currentVelocity *= (1f - Time.fixedDeltaTime * drag);

        if (isFlying)
        {
            // When flying, the CharacterController.isGrounded check is unreliable,
            // so we manage the full movement and ignore external movement providers.

            // Note: Ground movement (using the stick) is usually handled by a separate 
            // ContinuousMoveProvider. When isFlying is true, that provider's effect 
            // should be minimal or managed (e.g., by disabling the provider in code).

            // For this implementation, we assume the movement stick input is *not* used
            // during flight to prevent sudden drops (as requested by the user).

            // Apply current velocity to move the CharacterController
            characterController.Move(currentVelocity * Time.fixedDeltaTime);
        }
        else
        {
            // If not flying, apply natural gravity
            if (!characterController.isGrounded)
            {
                // Apply existing velocity but add gravity
                currentVelocity.y -= gravity * Time.fixedDeltaTime;
            }
            else if (currentVelocity.y < 0)
            {
                // Snap to ground if grounded
                currentVelocity.y = -0.5f;
            }

            // Apply existing velocity (including stick input from XR providers)
            // If you use an XR Rig template, the default move provider will 
            // update the CharacterController, so you only need to manage the Y-axis (gravity).

            // Note: Since this script is managing the *entire* movement 
            // via characterController.Move(), any external movement logic might interfere. 
            // For a robust solution, you need to ensure only ONE script calls 
            // characterController.Move() per frame. 

            // In a standard XR Rig setup, if the user moves the stick (which calls Move()), 
            // and we call Move() here too, it works fine, but we need to ensure the 
            // existing velocity is carried over.

            // Since we don't have access to the CharacterController.velocity,
            // we manually track and apply.

            // Use the CharacterController.Move() to apply the velocity
            characterController.Move(currentVelocity * Time.fixedDeltaTime);
        }
    }

    private void ApplyJetpackThrust()
    {
        // Get the average rotational direction of the two hands
        Quaternion avgRotation = Quaternion.Lerp(leftController.rotation, rightController.rotation, 0.5f);
        Vector3 avgThrustDirection = avgRotation * Vector3.up; // Jetpack thrust is typically along the controller's local UP axis when held like Iron Man.

        // Project the thrust direction onto the forward/up axes to categorize movement
        float verticalComponent = Vector3.Dot(avgThrustDirection, Vector3.up);
        float forwardComponent = Vector3.Dot(avgThrustDirection, transform.forward);

        Vector3 thrustVector = Vector3.zero;

        // 1. LIFTOFF / Hover (Pointing Downwards, verticalComponent is negative)
        if (verticalComponent < -0.8f) // Controllers pointing mostly downwards
        {
            thrustVector = Vector3.up * maxThrustForce;
        }
        // 2. FORWARD FLIGHT (Pointing Diagonally Backwards - we look at the forward component)
        else if (forwardComponent < -0.5f) // Controllers pointing backwards relative to the player's head/body
        {
            // Apply upward thrust and strong forward thrust
            thrustVector = (Vector3.up * maxThrustForce * 0.5f) + (transform.forward * maxThrustForce * forwardFlightMultiplier);
        }
        // 3. SLOW DOWN / Decelerate (Pointing Diagonally Forward)
        else if (forwardComponent > 0.5f) // Controllers pointing forward relative to the player's head/body
        {
            // Apply a force opposite to the current horizontal velocity (deceleration)
            Vector3 horizontalVelocity = new Vector3(currentVelocity.x, 0, currentVelocity.z);
            if (horizontalVelocity.magnitude > 0.1f)
            {
                thrustVector = -horizontalVelocity.normalized * maxDecelerationForce;
            }
        }
        else
        {
            // Neutral/Hover state - minimal upward thrust to counter gravity
            thrustVector = Vector3.up * (gravity * 0.9f);
        }

        // Apply the calculated thrust
        currentVelocity += thrustVector * Time.fixedDeltaTime;

        // Visual/Audio feedback (Particle effects would go here, enabled/disabled by the isFlying state)
        // Debug.Log("Thrusting: " + thrustVector);
    }

    // --- FUEL LOGIC ---

    private void HandleFuel()
    {
        if (triggersHeld && currentFuel > 0)
        {
            // Consume fuel
            currentFuel -= fuelConsumptionRate * Time.deltaTime;
            currentFuel = Mathf.Max(0, currentFuel);
        }
        else if (!triggersHeld && currentFuel < maxFuel)
        {
            // Recharge fuel
            currentFuel += fuelRechargeRate * Time.deltaTime;
            currentFuel = Mathf.Min(maxFuel, currentFuel);
        }

        UpdateFuelUI();

        // Low Fuel Warning Logic
        if (currentFuel <= lowFuelThreshold && fuelWarningCoroutine == null)
        {
            // Start warning signal
            fuelWarningCoroutine = StartCoroutine(LowFuelWarning());
        }
        else if (currentFuel > lowFuelThreshold && fuelWarningCoroutine != null)
        {
            // Stop warning signal
            StopCoroutine(fuelWarningCoroutine);
            fuelWarningCoroutine = null;
        }
    }

    private void UpdateFuelUI()
    {
        float fuelRatio = currentFuel / maxFuel;
        fuelSlider.value = fuelRatio;
        fuelText.text = $"FUEL: {Mathf.RoundToInt(currentFuel)}%";

        // Color transition logic (Green -> Yellow -> Red)
        Image fillImage = fuelSlider.fillRect.GetComponent<Image>();

        if (fuelRatio > midFuelThreshold / maxFuel)
        {
            fillImage.color = Color.Lerp(Color.yellow, Color.green, (fuelRatio - (midFuelThreshold / maxFuel)) / (1f - (midFuelThreshold / maxFuel)));
        }
        else if (fuelRatio > lowFuelThreshold / maxFuel)
        {
            fillImage.color = Color.Lerp(Color.red, Color.yellow, (fuelRatio - (lowFuelThreshold / maxFuel)) / ((midFuelThreshold / maxFuel) - (lowFuelThreshold / maxFuel)));
        }
        else
        {
            fillImage.color = Color.red;
        }
    }

    private IEnumerator LowFuelWarning()
    {
        // Simple blinking/sound loop
        while (currentFuel > 0)
        {
            // Play sound signal (if sfxSource and clip are assigned)
            if (sfxSource != null && lowFuelWarningSound != null && !sfxSource.isPlaying)
            {
                sfxSource.PlayOneShot(lowFuelWarningSound);
            }

            // Simple UI blink (could be done here too)

            yield return new WaitForSeconds(1.5f); // Wait time between beeps
        }
        fuelWarningCoroutine = null; // Ensure coroutine reference is cleared when fuel hits 0
    }


    // --- CRYSTAL COLLECTION LOGIC ---

    void OnTriggerEnter(Collider other)
    {
        // Check for the crystal tag
        if (other.CompareTag(CrystalTag))
        {
            CollectCrystal(other.gameObject);
        }
    }

    private void CollectCrystal(GameObject crystalObject)
    {
        crystalsCollected++;
        UpdateCrystalUI();

        // Optional: Play a collection sound/particle effect at the crystal's location

        Destroy(crystalObject);
    }

    private void UpdateCrystalUI()
    {
        // Assuming a total of 3 crystals as per your image (0/3)
        int totalCrystals = 3;
        crystalCounterText.text = $"{crystalsCollected}/{totalCrystals}";
    }
}*/

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System.Collections;
using TMPro;

/// <summary>
/// Controls the Iron Man-style jetpack flight, fuel management, and crystal collection.
/// This script should be attached to the GameObject that holds the CharacterController
/// (usually the 'Body' or 'CharacterController' GameObject underneath the XR Origin).
/// </summary>
[RequireComponent(typeof(CharacterController))]
public class IronManJetpack : MonoBehaviour
{
    // --- UNITY COMPONENT REFERENCES ---
    [Header("Controller Setup")]
    // Assign these Transforms to your Left Hand and Right Hand controller GameObjects (e.g., LeftHandController/RightHandController).
    public Transform leftController;
    public Transform rightController;
    private CharacterController characterController;

    // IMPORTANT: Reference to the external movement provider (Continuous Move Provider)
    // Assign the MonoBehaviour that handles stick-based walking here (usually on the XR Origin).
    [Header("XR Rig Components")]
    public MonoBehaviour groundMovementProvider;

    [Header("Input Actions")]
    // Assign these to your Left and Right Trigger/Grip Input Actions (e.g., "PrimaryButton" or "Trigger" actions).
    public InputActionReference leftTriggerAction;
    public InputActionReference rightTriggerAction;

    // --- JETPACK MOVEMENT PARAMETERS ---
    [Header("Flight Parameters")]
    public float maxThrustForce = 15f;
    public float maxDecelerationForce = 20f;
    public float forwardFlightMultiplier = 1.5f;
    public float gravity = 9.81f;
    public float drag = 0.5f;

    // Internal state
    private Vector3 currentVelocity = Vector3.zero;
    private bool isFlying = false;
    private bool wasFlying = false; // Tracks state change for ground movement toggle

    // --- FUEL SYSTEM PARAMETERS ---
    [Header("Fuel System")]
    public float maxFuel = 100f;
    public float fuelConsumptionRate = 30f; // Fuel consumed per second when flying
    public float fuelRechargeRate = 10f; // Fuel recharged per second when not flying
    public float lowFuelThreshold = 25f; // Red threshold
    public float midFuelThreshold = 50f; // Yellow threshold
    private float currentFuel;
    private bool triggersHeld = false;
    private Coroutine fuelWarningCoroutine;

    // --- UI/SFX REFERENCES ---
    [Header("UI & SFX")]
    // Assign these in the Inspector to your Canvas elements
    public Slider fuelSlider;
    public TMP_Text fuelText;
    public TMP_Text crystalCounterText;
    // NOTE: For SFX, you'd typically have an AudioSource component and an AudioClip reference.
    public AudioSource sfxSource;
    public AudioClip lowFuelWarningSound;

    // --- GAME STATE ---
    private int crystalsCollected = 0;
    private readonly string CrystalTag = "Crystal"; // Ensure your collectables have this tag

    void Awake()
    {
        characterController = GetComponent<CharacterController>();
        currentFuel = maxFuel;
        UpdateFuelUI();
        UpdateCrystalUI();
    }

    void OnEnable()
    {
        // Subscribe to trigger events
        leftTriggerAction.action.performed += OnTriggerChange;
        leftTriggerAction.action.canceled += OnTriggerChange;
        rightTriggerAction.action.performed += OnTriggerChange;
        rightTriggerAction.action.canceled += OnTriggerChange;
    }

    void OnDisable()
    {
        // Unsubscribe from trigger events
        leftTriggerAction.action.performed -= OnTriggerChange;
        leftTriggerAction.action.canceled -= OnTriggerChange;
        rightTriggerAction.action.performed -= OnTriggerChange;
        rightTriggerAction.action.canceled -= OnTriggerChange;
    }

    // This handles trigger press/release and determines if the jetpack is active
    private void OnTriggerChange(InputAction.CallbackContext context)
    {
        bool leftHeld = leftTriggerAction.action.IsPressed();
        bool rightHeld = rightTriggerAction.action.IsPressed();

        triggersHeld = leftHeld && rightHeld;
    }

    void Update()
    {
        // Handle Fuel Consumption and Recharge
        HandleFuel();

        // Store previous flying state for transition logic
        wasFlying = isFlying;

        // Determine current flying state
        if (triggersHeld && currentFuel > 0.01f)
        {
            isFlying = true;
            ApplyJetpackThrust();
        }
        else
        {
            isFlying = false;
        }

        // --- MOVEMENT PROVIDER TOGGLE (CRITICAL for conflict-free movement) ---
        // We use the groundMovementProvider field (like ContinuousMoveProvider)
        if (groundMovementProvider != null)
        {
            if (isFlying && groundMovementProvider.enabled)
            {
                // Disable ground movement when we start flying
                groundMovementProvider.enabled = false;
            }
            else if (!isFlying && !groundMovementProvider.enabled)
            {
                // Re-enable ground movement when we land or stop flying
                groundMovementProvider.enabled = true;
            }
        }
    }

    void FixedUpdate()
    {
        // Apply Drag (air resistance)
        currentVelocity *= (1f - Time.fixedDeltaTime * drag);

        if (isFlying)
        {
            // When flying, the thrust is applied in Update. We just apply the movement here.
            characterController.Move(currentVelocity * Time.fixedDeltaTime);
        }
        else
        {
            // --- GROUNDED / FALLING LOGIC ---

            if (!characterController.isGrounded)
            {
                // Apply natural gravity when airborne and not thrusting
                currentVelocity.y -= gravity * Time.fixedDeltaTime;
            }
            else
            {
                // FIX: If grounded, ensure Y-velocity is set to a small negative number 
                // to keep the CharacterController pressed against the ground 
                // without interfering with the horizontal movement applied by the 
                // ground movement provider (if it's enabled).
                if (currentVelocity.y < -0.5f)
                {
                    currentVelocity.y = -0.5f;
                }
            }

            // Move the CharacterController with the current velocity.
            // When the groundMovementProvider is enabled, it handles the horizontal movement,
            // but this script must still handle the vertical movement (gravity/falling)
            // when the jetpack is off.
            characterController.Move(currentVelocity * Time.fixedDeltaTime);
        }
    }

    private void ApplyJetpackThrust()
    {
        // Get the average rotational direction of the two hands
        Quaternion avgRotation = Quaternion.Lerp(leftController.rotation, rightController.rotation, 0.5f);
        // Vector3.up is the front-face of the controller; Vector3.forward is the handle direction.
        // We assume the thrust comes out of the palms when pointing them like Iron Man.
        // A common convention is that the controller's local UP axis points out of the 'palm' area.
        Vector3 avgThrustDirection = avgRotation * Vector3.up;

        // Project the thrust direction onto the forward/up axes to categorize movement
        float verticalComponent = Vector3.Dot(avgThrustDirection, Vector3.up);
        float forwardComponent = Vector3.Dot(avgThrustDirection, transform.forward);

        Vector3 thrustVector = Vector3.zero;

        // 1. LIFTOFF / Hover (Pointing Downwards, verticalComponent is negative)
        if (verticalComponent < -0.8f) // Controllers pointing mostly downwards
        {
            // This is the lift-off thrust
            thrustVector = Vector3.up * maxThrustForce;
        }
        // 2. FORWARD FLIGHT (Pointing Diagonally Backwards - we look at the forward component)
        else if (forwardComponent < -0.5f) // Controllers pointing backwards relative to the player's head/body
        {
            // Apply upward thrust (to maintain height) and strong forward thrust
            thrustVector = (Vector3.up * maxThrustForce * 0.5f) + (transform.forward * maxThrustForce * forwardFlightMultiplier);
        }
        // 3. SLOW DOWN / Decelerate (Pointing Diagonally Forward)
        else if (forwardComponent > 0.5f) // Controllers pointing forward relative to the player's head/body
        {
            // Apply a force opposite to the current horizontal velocity (deceleration)
            Vector3 horizontalVelocity = new Vector3(currentVelocity.x, 0, currentVelocity.z);
            if (horizontalVelocity.magnitude > 0.1f)
            {
                // Thrust opposes current motion
                thrustVector = -horizontalVelocity.normalized * maxDecelerationForce;
            }
            // Optionally apply a small downward force for quicker stop descent
            thrustVector.y += -gravity * 0.5f;
        }
        else
        {
            // Neutral/Hover state - minimal upward thrust to counter gravity
            thrustVector = Vector3.up * (gravity * 0.9f);
        }

        // Apply the calculated thrust
        currentVelocity += thrustVector * Time.fixedDeltaTime;

        // Visual/Audio feedback (Particle effects would go here, enabled/disabled by the isFlying state)
        // Debug.Log("Thrusting: " + thrustVector);
    }

    // --- FUEL LOGIC ---

    private void HandleFuel()
    {
        if (triggersHeld && currentFuel > 0)
        {
            // Consume fuel
            currentFuel -= fuelConsumptionRate * Time.deltaTime;
            currentFuel = Mathf.Max(0, currentFuel);
        }
        else if (!triggersHeld && currentFuel < maxFuel)
        {
            // Recharge fuel
            currentFuel += fuelRechargeRate * Time.deltaTime;
            currentFuel = Mathf.Min(maxFuel, currentFuel);
        }

        UpdateFuelUI();

        // Low Fuel Warning Logic
        if (currentFuel <= lowFuelThreshold && fuelWarningCoroutine == null)
        {
            // Start warning signal
            fuelWarningCoroutine = StartCoroutine(LowFuelWarning());
        }
        else if (currentFuel > lowFuelThreshold && fuelWarningCoroutine != null)
        {
            // Stop warning signal
            StopCoroutine(fuelWarningCoroutine);
            fuelWarningCoroutine = null;
        }

        // Force stop flying if fuel runs out
        if (currentFuel <= 0.01f && isFlying)
        {
            isFlying = false;
        }
    }

    private void UpdateFuelUI()
    {
        float fuelRatio = currentFuel / maxFuel;
        fuelSlider.value = fuelRatio;
        fuelText.text = $"FUEL: {Mathf.RoundToInt(currentFuel)}%";

        // Color transition logic (Green -> Yellow -> Red)
        Image fillImage = fuelSlider.fillRect.GetComponent<Image>();

        if (fuelRatio > midFuelThreshold / maxFuel)
        {
            fillImage.color = Color.Lerp(Color.yellow, Color.green, (fuelRatio - (midFuelThreshold / maxFuel)) / (1f - (midFuelThreshold / maxFuel)));
        }
        else if (fuelRatio > lowFuelThreshold / maxFuel)
        {
            fillImage.color = Color.Lerp(Color.red, Color.yellow, (fuelRatio - (lowFuelThreshold / maxFuel)) / ((midFuelThreshold / maxFuel) - (lowFuelThreshold / maxFuel)));
        }
        else
        {
            fillImage.color = Color.red;
        }
    }

    private IEnumerator LowFuelWarning()
    {
        // Simple blinking/sound loop
        while (currentFuel > 0)
        {
            // Play sound signal (if sfxSource and clip are assigned)
            if (sfxSource != null && lowFuelWarningSound != null && !sfxSource.isPlaying)
            {
                sfxSource.PlayOneShot(lowFuelWarningSound);
            }

            // Simple UI blink (could be done here too)

            yield return new WaitForSeconds(1.5f); // Wait time between beeps
        }
        fuelWarningCoroutine = null; // Ensure coroutine reference is cleared when fuel hits 0
    }


    // --- CRYSTAL COLLECTION LOGIC ---

    void OnTriggerEnter(Collider other)
    {
        // Check for the crystal tag
        if (other.CompareTag(CrystalTag))
        {
            CollectCrystal(other.gameObject);
        }
    }

    private void CollectCrystal(GameObject crystalObject)
    {
        crystalsCollected++;
        UpdateCrystalUI();

        // Optional: Play a collection sound/particle effect at the crystal's location

        Destroy(crystalObject);
    }

    private void UpdateCrystalUI()
    {
        // Assuming a total of 3 crystals as per your image (0/3)
        int totalCrystals = 3;
        crystalCounterText.text = $"{crystalsCollected}/{totalCrystals}";
    }
}