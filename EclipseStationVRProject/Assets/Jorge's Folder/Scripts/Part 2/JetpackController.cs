using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;         // For Slider
using UnityEngine.XR;         // For haptics

[RequireComponent(typeof(CharacterController))]
public class JetpackController : MonoBehaviour
{
    [Header("Settings")]
    public float thrust = 3f;           // How fast you rise
    public float fuel = 5f;             // Max fuel (seconds)
    public float rechargeRate = 1f;     // Fuel recharge per second
    public float gravityStrength = -2f; // Gentle gravity when not flying

    [Header("References")]
    public InputActionProperty jetpackAction; // Button binding
    public Slider fuelBar;                    // Optional: UI Slider
    public Image fuelFill;                    // The Fill image inside the slider
    public TMP_Text fuelText;                 // Optional: TMP text
    public AudioSource thrusterAudio;         // Jetpack looping sound

    [Header("Fuel Bar Colors")]
    public Color fullColor = Color.green;
    public Color midColor = Color.yellow;
    public Color lowColor = Color.red;

    [Header("Thruster Sound Settings")]
    public float minPitch = 0.6f; // pitch when almost empty
    public float maxPitch = 1.4f; // pitch when full fuel

    private CharacterController controller;
    private float currentFuel;
    private Vector3 velocity;
    private bool wasFlying = false;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        currentFuel = fuel;

        if (jetpackAction.action != null)
            jetpackAction.action.Enable();

        UpdateUI();
    }

    void OnEnable()
    {
        if (jetpackAction.action != null)
            jetpackAction.action.Enable();
    }

    void OnDisable()
    {
        if (jetpackAction.action != null)
            jetpackAction.action.Disable();
    }

    void Update()
    {
        bool isFlying = jetpackAction.action != null && jetpackAction.action.ReadValue<float>() > 0.5f;

        if (isFlying && currentFuel > 0f)
        {
            velocity.y = Mathf.Lerp(velocity.y, thrust, Time.deltaTime * 5f);//thrust; // push upward
            currentFuel -= Time.deltaTime;

            // Start audio if not playing
            if (thrusterAudio != null && !thrusterAudio.isPlaying)
                thrusterAudio.Play();

            // Adjust pitch based on remaining fuel
            if (thrusterAudio != null)
            {
                float fuelPercent = Mathf.Clamp01(currentFuel / fuel);
                thrusterAudio.pitch = Mathf.Lerp(minPitch, maxPitch, fuelPercent);
            }

            // Gentle vibration while flying
            SendHaptics(0.2f, 0.05f);
        }
        else
        {
            // Smooth gentle gravity (not harsh)
            velocity.y = Mathf.Lerp(velocity.y, gravityStrength, Time.deltaTime * 2f);

            // Stop audio if flying stopped
            if (thrusterAudio != null && thrusterAudio.isPlaying)
                thrusterAudio.Stop();

            // Recharge fuel while not flying
            if (currentFuel < fuel)
                currentFuel += rechargeRate * Time.deltaTime;

            // If just ran out of fuel, give a strong vibration bump
            if (wasFlying && currentFuel <= 0f)
                SendHaptics(0.7f, 0.2f);
        }

        controller.Move(velocity * Time.deltaTime);

        // Update UI
        UpdateUI();
        Debug.Log("Jetpack value: " + jetpackAction.action.ReadValue<float>());

        wasFlying = isFlying;
    }
    private void UpdateUI()
    {
        float normalizedFuel = Mathf.Clamp01(currentFuel / fuel);

        if (fuelBar != null)
            fuelBar.value = normalizedFuel;

        if (fuelText != null)
            fuelText.text = $"Fuel: {(int)(normalizedFuel * 100)}%";

        if (fuelFill != null)
        {
            if (normalizedFuel > 0.6f)
                fuelFill.color = fullColor;
            else if (normalizedFuel > 0.3f)
                fuelFill.color = midColor;
            else
                fuelFill.color = lowColor;
        }
    }
    private void SendHaptics(float amplitude, float duration)
    {
        // Send haptic impulse to both controllers
        var left = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
        var right = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);

        if (left.isValid)
            left.SendHapticImpulse(0u, amplitude, duration);

        if (right.isValid)
            right.SendHapticImpulse(0u, amplitude, duration);
    }
}
