/*using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.InputSystem.Controls;

public class LevelInstructionsManager : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject instructionPanel;
    public TMP_Text instructionText;
    public TMP_Text continueText;
    public Image instructionImage;
    [TextArea] public string levelInstructions = "Collect all the plants to complete the mission!";
    public Sprite instructionSprite;

    [Header("Settings")]
    public bool freezePlayer = true;
    public GameObject player; // optional

    private bool instructionsActive = true;
    private CanvasGroup canvasGroup;

    void Start()
    {
        if (instructionPanel)
        {
            instructionPanel.SetActive(true);
            canvasGroup = instructionPanel.GetComponent<CanvasGroup>();

            if (!canvasGroup)
                canvasGroup = instructionPanel.AddComponent<CanvasGroup>();

            canvasGroup.alpha = 0f;
            StartCoroutine(FadeCanvas(1f, 0.8f)); // Fade in
        }

        if (instructionText)
            instructionText.text = levelInstructions;

        if (instructionImage && instructionSprite)
            instructionImage.sprite = instructionSprite;

        if (freezePlayer && player)
            player.SetActive(false);

        if (continueText)
            StartCoroutine(PulseContinueText());
    }

    void Update()
    {
        if (instructionsActive && AnyInputPressed())
        {
            StartCoroutine(FadeOutAndHide());
        }
    }

    bool AnyInputPressed()
    {
        if (Keyboard.current != null && Keyboard.current.anyKey.wasPressedThisFrame)
            return true;

        if (Gamepad.current != null)
        {
            foreach (var control in Gamepad.current.allControls)
            {
                if (control is ButtonControl button && button.wasPressedThisFrame)
                    return true;
            }
        }

        return false;
    }

    IEnumerator FadeOutAndHide()
    {
        instructionsActive = false;
        yield return FadeCanvas(0f, 0.5f);

        if (instructionPanel)
            instructionPanel.SetActive(false);

        if (freezePlayer && player)
            player.SetActive(true);
    }

    IEnumerator FadeCanvas(float targetAlpha, float duration)
    {
        if (!canvasGroup) yield break;
        float startAlpha = canvasGroup.alpha;
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, time / duration);
            yield return null;
        }
        canvasGroup.alpha = targetAlpha;
    }

    IEnumerator PulseContinueText()
    {
        while (true)
        {
            // Fade out
            for (float t = 0; t < 1f; t += Time.deltaTime)
            {
                continueText.alpha = Mathf.Lerp(1f, 0.3f, t);
                yield return null;
            }
            // Fade in
            for (float t = 0; t < 1f; t += Time.deltaTime)
            {
                continueText.alpha = Mathf.Lerp(0.3f, 1f, t);
                yield return null;
            }
        }
    }
}*

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.InputSystem.Controls;

public class LevelInstructionsManager : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject instructionPanel;
    public TMP_Text instructionText;
    public TMP_Text continueText;
    public Image instructionImage;
    [TextArea] public string levelInstructions = "Collect all the plants to complete the mission!";
    public Sprite instructionSprite;

    [Header("Settings")]
    public bool freezePlayer = true;
    public GameObject player; // optional

    private bool instructionsActive = true;
    private CanvasGroup canvasGroup;

    void Start()
    {
        if (instructionPanel)
        {
            instructionPanel.SetActive(true);
            canvasGroup = instructionPanel.GetComponent<CanvasGroup>() ?? instructionPanel.AddComponent<CanvasGroup>();
            canvasGroup.alpha = 0f;
            StartCoroutine(FadeCanvas(1f, 0.8f)); // Fade in
        }

        if (instructionText)
            instructionText.text = levelInstructions;

        if (instructionImage && instructionSprite)
            instructionImage.sprite = instructionSprite;

        if (freezePlayer && player)
            player.SetActive(false);

        if (continueText)
            StartCoroutine(PulseContinueText());
    }

    void Update()
    {
        if (instructionsActive && AnyInputPressed())
        {
            StartCoroutine(FadeOutAndHide());
        }
    }

    bool AnyInputPressed()
    {
        // Keyboard (for testing in editor)
        if (Keyboard.current != null && Keyboard.current.anyKey.wasPressedThisFrame)
            return true;

        // Gamepad (if using controller)
        if (Gamepad.current != null)
        {
            foreach (var control in Gamepad.current.allControls)
                if (control is ButtonControl button && button.wasPressedThisFrame)
                    return true;
        }

        //VR controllers (Quest 3 / XR)
        if (XRController.leftHand != null && ControllerPressed(XRController.leftHand))
            return true;

        if (XRController.rightHand != null && ControllerPressed(XRController.rightHand))
            return true;

        return false;
    }

    bool ControllerPressed(XRController controller)
    {
        foreach (var control in controller.allControls)
        {
            if (control is ButtonControl button && button.wasPressedThisFrame)
                return true;
        }
        return false;
    }

    IEnumerator FadeOutAndHide()
    {
        instructionsActive = false;
        yield return FadeCanvas(0f, 0.5f);

        if (instructionPanel)
            instructionPanel.SetActive(false);

        if (freezePlayer && player)
            player.SetActive(true);
    }

    IEnumerator FadeCanvas(float targetAlpha, float duration)
    {
        if (!canvasGroup) yield break;
        float startAlpha = canvasGroup.alpha;
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, time / duration);
            yield return null;
        }
        canvasGroup.alpha = targetAlpha;
    }

    IEnumerator PulseContinueText()
    {
        while (true)
        {
            // Fade out
            for (float t = 0; t < 1f; t += Time.deltaTime)
            {
                continueText.alpha = Mathf.Lerp(1f, 0.3f, t);
                yield return null;
            }
            // Fade in
            for (float t = 0; t < 1f; t += Time.deltaTime)
            {
                continueText.alpha = Mathf.Lerp(0.3f, 1f, t);
                yield return null;
            }
        }
    }
}*/
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.InputSystem.Controls;

public class LevelInstructionsManager : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject instructionPanel;
    public TMP_Text instructionText;
    public TMP_Text continueText;
    public Image instructionImage;
    [TextArea] public string levelInstructions = "Collect all the plants to complete the mission!";
    public Sprite instructionSprite;

    [Header("Settings")]
    public bool freezePlayer = true;
    public GameObject player; // optional
    public float dismissDelay = 5f; // Wait before allowing input

    private bool instructionsActive = true;
    private bool canDismiss = false;
    private CanvasGroup canvasGroup;

    void Start()
    {
        if (instructionPanel)
        {
            instructionPanel.SetActive(true);
            canvasGroup = instructionPanel.GetComponent<CanvasGroup>() ?? instructionPanel.AddComponent<CanvasGroup>();
            canvasGroup.alpha = 0f;
            StartCoroutine(FadeCanvas(1f, 0.8f)); // Fade in
        }

        if (instructionText)
            instructionText.text = levelInstructions;

        if (instructionImage && instructionSprite)
            instructionImage.sprite = instructionSprite;

        if (freezePlayer && player)
            player.SetActive(false);

        if (continueText)
            continueText.alpha = 0f;

        // Start the delay timer
        StartCoroutine(EnableDismissAfterDelay());
    }

    void Update()
    {
        if (instructionsActive && canDismiss && AnyInputPressed())
        {
            StartCoroutine(FadeOutAndHide());
        }
    }

    IEnumerator EnableDismissAfterDelay()
    {
        yield return new WaitForSeconds(dismissDelay);
        canDismiss = true;

        if (continueText)
        {
            continueText.alpha = 1f;
            StartCoroutine(PulseContinueText());
        }
    }

    bool AnyInputPressed()
    {
        // Keyboard (for editor testing)
        if (Keyboard.current != null && Keyboard.current.anyKey.wasPressedThisFrame)
            return true;

        // Gamepad (for controller testing)
        if (Gamepad.current != null)
        {
            foreach (var control in Gamepad.current.allControls)
                if (control is ButtonControl button && button.wasPressedThisFrame)
                    return true;
        }

        //VR controllers (Quest 3)
        if (XRController.leftHand != null && ControllerPressed(XRController.leftHand))
            return true;

        if (XRController.rightHand != null && ControllerPressed(XRController.rightHand))
            return true;

        return false;
    }

    bool ControllerPressed(XRController controller)
    {
        foreach (var control in controller.allControls)
        {
            if (control is ButtonControl button && button.wasPressedThisFrame)
                return true;
        }
        return false;
    }

    IEnumerator FadeOutAndHide()
    {
        instructionsActive = false;
        yield return FadeCanvas(0f, 0.5f);

        if (instructionPanel)
            instructionPanel.SetActive(false);

        if (freezePlayer && player)
            player.SetActive(true);
    }

    IEnumerator FadeCanvas(float targetAlpha, float duration)
    {
        if (!canvasGroup) yield break;
        float startAlpha = canvasGroup.alpha;
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, time / duration);
            yield return null;
        }
        canvasGroup.alpha = targetAlpha;
    }

    IEnumerator PulseContinueText()
    {
        while (true)
        {
            // Fade out
            for (float t = 0; t < 1f; t += Time.deltaTime)
            {
                continueText.alpha = Mathf.Lerp(1f, 0.3f, t);
                yield return null;
            }
            // Fade in
            for (float t = 0; t < 1f; t += Time.deltaTime)
            {
                continueText.alpha = Mathf.Lerp(0.3f, 1f, t);
                yield return null;
            }
        }
    }
}


