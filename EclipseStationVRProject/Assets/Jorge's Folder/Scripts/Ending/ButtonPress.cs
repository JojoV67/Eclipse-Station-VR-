using UnityEngine;

public class ButtonPress : MonoBehaviour
{
    [HideInInspector] public bool isPressed = false; // Hide it from Inspector

    private Material originalMaterial;
    public Material pressedMaterial;
    private Renderer rend;
    public AudioSource pressSound;

    public FadeScreen fadeScreen; // Assign this in the inspector

    void Start()
    {
        rend = GetComponent<Renderer>();
        originalMaterial = rend.material;
        isPressed = false; // Ensure it starts false
    }

    public void PressButton()
    {
        if (!isPressed)
        {
            isPressed = true;
            rend.material = pressedMaterial;

            if (pressSound != null)
                pressSound.Play();

            FindFirstObjectByType<CountdownStarter>().RegisterButtonPress();

            if (fadeScreen != null)
            {
                fadeScreen.FadeOut(); // Or FadeIn() depending on what you want
            }
        }
    }

    public void ResetButton()
    {
        isPressed = false;
        rend.material = originalMaterial;
    }
}