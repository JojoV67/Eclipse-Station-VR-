/*using UnityEngine;

public class ButtonPress : MonoBehaviour
{
    private bool hasBeenPressed = false;

    public void PressButton()
    {
        if (!hasBeenPressed)
        {
            hasBeenPressed = true;
            CountdownStarter countdown = FindFirstObjectByType<CountdownStarter>();
            if (countdown != null)
            {
                countdown.RegisterButtonPress();
            }
            Debug.Log("Button Pressed!");
        }
    }
}*/

/*using UnityEngine;

public class ButtonPress : MonoBehaviour
{
    public bool isPressed = false;
    private Material originalMaterial;
    public Material pressedMaterial;
    private Renderer rend;

    void Start()
    {
        rend = GetComponent<Renderer>();
        originalMaterial = rend.material;
    }

    public void PressButton()
    {
        if (!isPressed)
        {
            isPressed = true;
            rend.material = pressedMaterial;
            FindFirstObjectByType<CountdownStarter>().RegisterButtonPress();
        }
    }

    public void ResetButton()
    {
        isPressed = false;
        rend.material = originalMaterial;
    }
}*/

/*using UnityEngine;

public class ButtonPress : MonoBehaviour
{
    public bool isPressed = false;
    private Material originalMaterial;
    public Material pressedMaterial;
    private Renderer rend;
    public AudioSource pressSound; // <-- Add this line

    void Start()
    {
        rend = GetComponent<Renderer>();
        originalMaterial = rend.material;
    }

    public void PressButton()
    {
        if (!isPressed)
        {
            isPressed = true;
            rend.material = pressedMaterial;

            if (pressSound != null)
            {
                pressSound.Play(); // <-- Play sound
            }

            FindFirstObjectByType<CountdownStarter>().RegisterButtonPress();
        }
    }

    public void ResetButton()
    {
        isPressed = false;
        rend.material = originalMaterial;
    }
}*/

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

            // Optionally delay destruction to allow sound and visual change to play
            //Destroy(gameObject, 0.3f); // Adjust delay if needed

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






