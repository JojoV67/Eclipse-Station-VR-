/*using TMPro;
using UnityEngine;

public class CountdownStarter : MonoBehaviour
{
    public TextMeshProUGUI countdownText;
    public float countdownTime = 120f; // 2 minutes
    private float currentTime;
    private bool timerActive = false;

    public AudioSource successSound;
    private int buttonsPressed = 0;

    private void Update()
    {
        if (timerActive)
        {
            currentTime -= Time.deltaTime;
            if (currentTime <= 0)
            {
                currentTime = 0;
                timerActive = false;
                countdownText.text = "Time's up!";
            }
            else
            {
                int minutes = Mathf.FloorToInt(currentTime / 60);
                int seconds = Mathf.FloorToInt(currentTime % 60);
                countdownText.text = minutes.ToString("00") + ":" + seconds.ToString("00");
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !timerActive)
        {
            currentTime = countdownTime;
            timerActive = true;
            Debug.Log("Countdown started!");
        }
    }

    public void RegisterButtonPress()
    {
        buttonsPressed++;

        if (buttonsPressed >= 3 && timerActive)
        {
            timerActive = false;
            countdownText.text = "System Reset!";
            if (successSound != null)
            {
                successSound.Play();
            }
            Debug.Log("All buttons pressed. Countdown stopped.");
        }
    }
}*/

/*using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class CountdownStarter : MonoBehaviour
{
    public TextMeshProUGUI countdownText;
    public GameObject goodEndingCanvas;
    public AudioSource systemRebootAudio;
    public FadeController fadeController; // Handles fading
    private float countdownTime = 120f;
    private bool countdownActive = false;
    private int buttonsPressed = 0;

    public GameObject gameOverCanvas; // The canvas that shows Retry/Quit
    public AudioSource explosionAudio; // Sound that plays when player fails

    void Update()
    {
        if (countdownActive)
        {
            countdownTime -= Time.deltaTime;
            UpdateCountdownDisplay();

            if (countdownTime <= 0f)
            {
                countdownActive = false;
                StartCoroutine(BadEndingSequence());
            }
        }
    }

    public void StartCountdown()
    {
        countdownActive = true;
        countdownTime = 120f; // 2 minutes
        UpdateCountdownDisplay();
    }

    public void RegisterButtonPress()
    {
        buttonsPressed++;
        Debug.Log($"Buttons pressed: {buttonsPressed}");

        if (buttonsPressed >= 3)
        {
            countdownActive = false;
            StartCoroutine(GoodEndingSequence());
        }
    }

    private IEnumerator GoodEndingSequence()
    {
        Debug.Log("Good ending sequence started.");

        if (systemRebootAudio != null)
        {
            systemRebootAudio.Play();
        }

        // Wait for the audio clip to finish
        yield return new WaitForSeconds(systemRebootAudio.clip.length);

        // Fade to white
        if (fadeController != null)
        {
            yield return fadeController.FadeToWhite();
        }

        // Activate the Good Ending UI
        goodEndingCanvas.SetActive(true);
    }

    private IEnumerator BadEndingSequence()
    {
        Debug.Log("Bad ending sequence started.");

        // Fade to black
        if (fadeController != null)
        {
            yield return fadeController.FadeToBlack();
        }

        // Play explosion sound
        if (explosionAudio != null)
        {
            explosionAudio.Play();
        }

        // Small wait so explosion isn't cut off
        yield return new WaitForSeconds(2f);

        // Show Game Over UI
        if (gameOverCanvas != null)
        {
            gameOverCanvas.SetActive(true);
        }
    }

    private void UpdateCountdownDisplay()
    {
        int minutes = Mathf.FloorToInt(countdownTime / 60);
        int seconds = Mathf.FloorToInt(countdownTime % 60);
        countdownText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    // Button method to quit
    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit();
    }
}*/

/*using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class CountdownStarter : MonoBehaviour
{
    public TextMeshProUGUI countdownText;
    public GameObject goodEndingCanvas;
    public GameObject gameOverCanvas;
    public AudioSource systemRebootAudio;
    public AudioSource explosionAudio;
    public FadeController fadeController;
    public Transform checkpointLocation;
    public GameObject badEndingCanvas;
    public GameObject player; // Your VR rig or player object

    private float countdownTime = 120f;
    private bool countdownActive = false;
    private int buttonsPressed = 0;

    public ButtonPress[] buttons;

    void Update()
    {
        if (countdownActive)
        {
            countdownTime -= Time.deltaTime;
            UpdateCountdownDisplay();

            if (countdownTime <= 0f)
            {
                countdownTime = 0f; // clamp to 0
                countdownActive = false;
                StartCoroutine(BadEndingSequence());
            }
            UpdateCountdownDisplay();
        }
    }

    public void StartCountdown()
    {
        countdownActive = true;
        countdownTime = 120f; // 2 minutes
        UpdateCountdownDisplay();
    }

    public void RegisterButtonPress()
    {
        buttonsPressed++;
        Debug.Log($"Buttons pressed: {buttonsPressed}");

        if (buttonsPressed >= 3)
        {
            countdownActive = false;
            StartCoroutine(GoodEndingSequence());
        }
    }

    private IEnumerator GoodEndingSequence()
    {
        Debug.Log("Good ending sequence started.");

        if (systemRebootAudio != null)
        {
            systemRebootAudio.Play();
        }

        yield return new WaitForSeconds(systemRebootAudio.clip.length);

        if (fadeController != null)
        {
            yield return fadeController.FadeToWhite();
        }

        goodEndingCanvas.SetActive(true);
    }

    private IEnumerator BadEndingSequence()
    {
        Debug.Log("Bad ending sequence started.");

        if (fadeController != null)
        {
            yield return fadeController.FadeToBlack();
        }

        if (explosionAudio != null)
        {
            explosionAudio.Play();
        }

        // Wait a little after explosion
        yield return new WaitForSeconds(2f);

        gameOverCanvas.SetActive(true);
    }

    private void UpdateCountdownDisplay()
    {
        int minutes = Mathf.FloorToInt(countdownTime / 60);
        int seconds = Mathf.FloorToInt(countdownTime % 60);
        countdownText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void RetryCountdown()
    {
        StartCoroutine(RestartSequence());
    }

    private IEnumerator RestartSequence()
    {
        gameOverCanvas.SetActive(false);

        if (fadeController != null)
        {
            yield return fadeController.FadeFromBlack(); // Fade back in
        }

        // Teleport player
        if (player != null && checkpointLocation != null)
        {
            player.transform.position = checkpointLocation.position;
            player.transform.rotation = checkpointLocation.rotation;
        }

        // Reset timer and button counter
        buttonsPressed = 0;
        countdownTime = 120f;
        countdownActive = true;
        UpdateCountdownDisplay();
    }

    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit();
    }

    public void Retry()
    {
        Debug.Log("Retrying...");

        // Reset timer and button press count
        countdownTime = 120f;
        countdownActive = false;
        buttonsPressed = 0;
        UpdateCountdownDisplay();

        // Reset button visuals/states
        foreach (ButtonPress button in buttons)
        {
            button.ResetButton();
        }

        // Move the player back to the start (if you already have that code)
        MovePlayerToStart();

        // Hide the bad ending canvas
        badEndingCanvas.SetActive(false);

        // Start countdown again
        StartCountdown();
    }

    private void ResetRespawnableItems()
    {
        RespawnableItem[] items = FindObjectsByType<RespawnableItem>(FindObjectsSortMode.None);
        foreach (var item in items)
        {
            item.ForceRespawn();
        }
    }

    private void MovePlayerToStart()
    {
        GameObject player = GameObject.FindWithTag("Player");
        Transform startZone = GameObject.FindWithTag("StartZone").transform;

        if (player != null && startZone != null)
        {
            player.transform.position = startZone.position;
            player.transform.rotation = startZone.rotation;
        }
        else
        {
            Debug.LogWarning("Player or StartZone not found! Check your tags.");
        }
    }
}*/

/*using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class CountdownStarter : MonoBehaviour
{
    public float timeLeft = 120f;
    public TMP_Text countdownText;
    public TMP_Text statusText;
    public GameObject doorTrigger;
    public CanvasGroup fadeCanvasGroup;
    public ButtonPress[] buttons;
    public AudioSource failSound;
    public AudioSource successSound;

    private bool isCountingDown = false;
    private bool hasFailed = false;

    public void RegisterButtonPress()
    {
        if (!isCountingDown)
            return;

        // Check if all buttons are pressed
        foreach (ButtonPress button in buttons)
        {
            if (!button.isPressed)
                return;
        }

        // All buttons pressed!
        StartCoroutine(SuccessSequence());
    }

    public void StartCountdown()
    {
        if (!isCountingDown)
        {
            isCountingDown = true;
            hasFailed = false;
            StartCoroutine(CountdownRoutine());
        }
    }

    IEnumerator CountdownRoutine()
    {
        while (timeLeft > 0f)
        {
            countdownText.text = "Time Left: " + Mathf.CeilToInt(timeLeft).ToString();
            yield return new WaitForSeconds(1f);
            timeLeft -= 1f;
        }

        Fail();
    }

    void Fail()
    {
        if (hasFailed) return;

        hasFailed = true;
        isCountingDown = false;
        countdownText.text = "Time Left: 0";
        statusText.text = "FAILED!";
        if (failSound) failSound.Play();
        fadeCanvasGroup.alpha = 1f;

        // You can show retry UI or auto-retry here
    }

    IEnumerator SuccessSequence()
    {
        isCountingDown = false;
        if (successSound) successSound.Play();
        statusText.text = "SUCCESS!";
        yield return new WaitForSeconds(1f);
        fadeCanvasGroup.alpha = 1f;
        yield return new WaitForSeconds(2f);
        doorTrigger.SetActive(true);
    }

    public void Retry()
    {
        StopAllCoroutines();
        isCountingDown = false;
        hasFailed = false;
        timeLeft = 120f;
        countdownText.text = "";
        statusText.text = "";
        fadeCanvasGroup.alpha = 0f;

        // Reset all button states
        foreach (ButtonPress button in buttons)
        {
            button.ResetButton(); // Ensure buttons reset visually and logically
        }

        doorTrigger.SetActive(false);
        StartCoroutine(EnableButtonsAfterDelay());
    }

    IEnumerator EnableButtonsAfterDelay()
    {
        yield return new WaitForSeconds(0.5f);
        StartCountdown(); // Restart the countdown
    }
}*/

/*using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class CountdownStarter : MonoBehaviour
{
    public TextMeshProUGUI countdownText;
    public GameObject goodEndingCanvas;
    public GameObject gameOverCanvas;
    public AudioSource systemRebootAudio;
    public AudioSource explosionAudio;
    public FadeController fadeController;
    public Transform checkpointLocation;
    public GameObject badEndingCanvas;
    public GameObject player; // Your VR rig or player object

    private float countdownTime = 120f;
    private bool countdownActive = false;
    private int buttonsPressed = 0;

    public ButtonPress[] buttons;

    void Update()
    {
        if (countdownActive)
        {
            countdownTime -= Time.deltaTime;
            UpdateCountdownDisplay();

            if (countdownTime <= 0f)
            {
                countdownTime = 0f; // clamp to 0
                countdownActive = false;
                StartCoroutine(BadEndingSequence());
            }
        }
    }

    public void StartCountdown()
    {
        countdownActive = true;
        countdownTime = 120f; // 2 minutes
        UpdateCountdownDisplay();
    }

    public void RegisterButtonPress()
    {
        buttonsPressed++;
        Debug.Log($"Buttons pressed: {buttonsPressed}");

        if (buttonsPressed >= 3)
        {
            countdownActive = false;
            StartCoroutine(GoodEndingSequence());
        }
    }

    private IEnumerator GoodEndingSequence()
    {
        Debug.Log("Good ending sequence started.");

        if (systemRebootAudio != null)
        {
            systemRebootAudio.Play();
        }

        yield return new WaitForSeconds(systemRebootAudio.clip.length);

        if (fadeController != null)
        {
            yield return fadeController.FadeToWhite();
        }

        goodEndingCanvas.SetActive(true);
    }

    private IEnumerator BadEndingSequence()
    {
        Debug.Log("Bad ending sequence started.");

        if (fadeController != null)
        {
            yield return fadeController.FadeToBlack();
        }

        if (explosionAudio != null)
        {
            explosionAudio.Play();
        }

        // Wait a little after explosion
        yield return new WaitForSeconds(2f);

        gameOverCanvas.SetActive(true);
    }

    private void UpdateCountdownDisplay()
    {
        int minutes = Mathf.FloorToInt(countdownTime / 60);
        int seconds = Mathf.FloorToInt(countdownTime % 60);
        countdownText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void RetryCountdown()
    {
        StartCoroutine(RestartSequence());
    }

    private IEnumerator RestartSequence()
    {
        gameOverCanvas.SetActive(false);

        if (fadeController != null)
        {
            yield return fadeController.FadeFromBlack(); // Fade back in
        }

        // Teleport player
        if (player != null && checkpointLocation != null)
        {
            player.transform.position = checkpointLocation.position;
            player.transform.rotation = checkpointLocation.rotation;
        }

        // Reset timer and button counter
        buttonsPressed = 0;
        countdownTime = 120f;
        countdownActive = true;
        UpdateCountdownDisplay();
    }

    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit();
    }

    public void Retry()
    {
        Debug.Log("Retrying...");

        // Reset timer and button press count
        countdownTime = 120f;
        countdownActive = false;
        buttonsPressed = 0;
        UpdateCountdownDisplay();

        // Reset all button states
        if (buttons != null)
        {
            foreach (ButtonPress button in buttons)
            {
                if (button != null)
                {
                    button.ResetButton();
                }
            }
        }

        // Move the player back to the start
        MovePlayerToStart();

        // Hide the bad ending canvas
        badEndingCanvas.SetActive(false);

        // Start countdown again
        StartCountdown();
    }

    private void ResetRespawnableItems()
    {
        RespawnableItem[] items = FindObjectsByType<RespawnableItem>(FindObjectsSortMode.None);
        foreach (var item in items)
        {
            item.ForceRespawn();
        }
    }

    private void MovePlayerToStart()
    {
        GameObject player = GameObject.FindWithTag("Player");
        Transform startZone = GameObject.FindWithTag("StartZone")?.transform;

        if (player != null && startZone != null)
        {
            player.transform.position = startZone.position;
            player.transform.rotation = startZone.rotation;
        }
        else
        {
            Debug.LogWarning("Player or StartZone not found! Check your tags.");
        }
    }
}*/

/*using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class CountdownStarter : MonoBehaviour
{
    public TextMeshProUGUI countdownText;
    public GameObject goodEndingCanvas;
    public GameObject gameOverCanvas;
    public AudioSource systemRebootAudio;
    public AudioSource explosionAudio;
    public FadeController fadeController;
    public Transform checkpointLocation;
    public GameObject badEndingCanvas;
    public GameObject player;

    private float countdownTime = 120f;
    private bool countdownActive = false;
    private int buttonsPressed = 0;

    public ButtonPress[] buttons;

    [Header("Button Groups")]
    public GameObject[] activeButtonObjects;      // Initially visible in the scene
    public GameObject[] alternateButtonObjects;   // Initially hidden in the scene

    void Update()
    {
        if (countdownActive)
        {
            countdownTime -= Time.deltaTime;
            UpdateCountdownDisplay();

            if (countdownTime <= 0f)
            {
                countdownTime = 0f;
                countdownActive = false;
                StartCoroutine(BadEndingSequence());
            }
        }
    }

    public void StartCountdown()
    {
        countdownActive = true;
        countdownTime = 120f;
        UpdateCountdownDisplay();
    }

    public void RegisterButtonPress()
    {
        buttonsPressed++;
        Debug.Log($"Buttons pressed: {buttonsPressed}");

        if (buttonsPressed >= 3)
        {
            countdownActive = false;
            StartCoroutine(GoodEndingSequence());
        }
    }

    private IEnumerator GoodEndingSequence()
    {
        Debug.Log("Good ending sequence started.");

        if (systemRebootAudio != null)
        {
            systemRebootAudio.Play();
        }

        yield return new WaitForSeconds(systemRebootAudio.clip.length);

        if (fadeController != null)
        {
            yield return fadeController.FadeToWhite();
        }

        goodEndingCanvas.SetActive(true);
    }

    private IEnumerator BadEndingSequence()
    {
        Debug.Log("Bad ending sequence started.");

        if (fadeController != null)
        {
            yield return fadeController.FadeToBlack();
        }

        if (explosionAudio != null)
        {
            explosionAudio.Play();
        }

        yield return new WaitForSeconds(2f);
        gameOverCanvas.SetActive(true);
    }

    private void UpdateCountdownDisplay()
    {
        int minutes = Mathf.FloorToInt(countdownTime / 60);
        int seconds = Mathf.FloorToInt(countdownTime % 60);
        countdownText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void Retry()
    {
        Debug.Log("Retrying...");

        // Reset timer and state
        countdownTime = 120f;
        countdownActive = false;
        buttonsPressed = 0;
        UpdateCountdownDisplay();

        // Hide the original buttons
        foreach (GameObject btn in activeButtonObjects)
        {
            if (btn != null) btn.SetActive(false);
        }

        // Show alternate buttons
        foreach (GameObject altBtn in alternateButtonObjects)
        {
            if (altBtn != null) altBtn.SetActive(true);
        }

        // Reset the ButtonPress scripts (in case they persist)
        foreach (ButtonPress button in buttons)
        {
            if (button != null) button.ResetButton();
        }

        // Move the player back to the start
        MovePlayerToStart();

        badEndingCanvas.SetActive(false);

        // Start the countdown again
        StartCountdown();
    }

    private void ResetRespawnableItems()
    {
        RespawnableItem[] items = FindObjectsByType<RespawnableItem>(FindObjectsSortMode.None);
        foreach (var item in items)
        {
            item.ForceRespawn();
        }
    }

    private void MovePlayerToStart()
    {
        GameObject player = GameObject.FindWithTag("Player");
        Transform startZone = GameObject.FindWithTag("StartZone").transform;

        if (player != null && startZone != null)
        {
            player.transform.position = startZone.position;
            player.transform.rotation = startZone.rotation;
        }
        else
        {
            Debug.LogWarning("Player or StartZone not found! Check your tags.");
        }
    }

    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit();
    }
}*/

/*using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class CountdownStarter : MonoBehaviour
{
    public TextMeshProUGUI countdownText;
    public GameObject goodEndingCanvas;
    public GameObject gameOverCanvas;
    public AudioSource systemRebootAudio;
    public AudioSource explosionAudio;
    public FadeController fadeController;
    public Transform checkpointLocation;
    public GameObject badEndingCanvas;
    public GameObject player; // Your VR rig or player object

    private float countdownTime = 120f;
    private bool countdownActive = false;
    private int buttonsPressed = 0;

    public ButtonPress[] buttons;

    public GameObject[] activeButtonObjects;   // initially visible
    public GameObject[] alternateButtonObjects; // initially hidden

    private void Start()
    {
        // Ensure alternate buttons are hidden at start
        foreach (GameObject obj in alternateButtonObjects)
        {
            obj.SetActive(false);
        }
    }

    void Update()
    {
        if (countdownActive)
        {
            countdownTime -= Time.deltaTime;
            UpdateCountdownDisplay();

            if (countdownTime <= 0f)
            {
                countdownTime = 0f; // clamp to 0
                countdownActive = false;
                StartCoroutine(BadEndingSequence());
            }
            UpdateCountdownDisplay();
        }
    }

    public void StartCountdown()
    {
        countdownActive = true;
        countdownTime = 120f; // 2 minutes
        UpdateCountdownDisplay();
    }

    public void RegisterButtonPress()
    {
        buttonsPressed++;
        Debug.Log($"Buttons pressed: {buttonsPressed}");

        if (buttonsPressed >= 3)
        {
            countdownActive = false;
            StartCoroutine(GoodEndingSequence());
        }
    }

    private IEnumerator GoodEndingSequence()
    {
        Debug.Log("Good ending sequence started.");

        if (systemRebootAudio != null)
        {
            systemRebootAudio.Play();
        }

        yield return new WaitForSeconds(systemRebootAudio.clip.length);

        if (fadeController != null)
        {
            yield return fadeController.FadeToWhite();
        }

        goodEndingCanvas.SetActive(true);
    }

    private IEnumerator BadEndingSequence()
    {
        Debug.Log("Bad ending sequence started.");

        if (fadeController != null)
        {
            yield return fadeController.FadeToBlack();
        }

        if (explosionAudio != null)
        {
            explosionAudio.Play();
        }

        // Wait a little after explosion
        yield return new WaitForSeconds(2f);

        gameOverCanvas.SetActive(true);
    }

    private void UpdateCountdownDisplay()
    {
        int minutes = Mathf.FloorToInt(countdownTime / 60);
        int seconds = Mathf.FloorToInt(countdownTime % 60);
        countdownText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void RetryCountdown()
    {
        StartCoroutine(RestartSequence());
    }

    private IEnumerator RestartSequence()
    {
        gameOverCanvas.SetActive(false);

        if (fadeController != null)
        {
            yield return fadeController.FadeFromBlack(); // Fade back in
        }

        // Teleport player
        if (player != null && checkpointLocation != null)
        {
            player.transform.position = checkpointLocation.position;
            player.transform.rotation = checkpointLocation.rotation;
        }

        // Reset timer and button counter
        buttonsPressed = 0;
        countdownTime = 120f;
        countdownActive = true;
        UpdateCountdownDisplay();
    }

    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit();
    }

    public void Retry()
    {
        Debug.Log("Retrying...");

        // Reset timer and button press count
        countdownTime = 120f;
        countdownActive = false;
        buttonsPressed = 0;
        UpdateCountdownDisplay();

        // Reset button visuals/states
        foreach (ButtonPress button in buttons)
        {
            button.ResetButton();
        }

        // Move the player back to the start (if you already have that code)
        MovePlayerToStart();

        // Hide the bad ending canvas
        badEndingCanvas.SetActive(false);

        // Start countdown again
        StartCountdown();
        Debug.Log("Retrying...");

        countdownTime = 120f;
        countdownActive = false;
        buttonsPressed = 0;
        UpdateCountdownDisplay();

        // Hide active buttons
        /*foreach (GameObject btn in activeButtonObjects)
        {
            btn.SetActive(false);
        }
        foreach (GameObject obj in activeButtonObjects)
        {
            obj.SetActive(false);
        }

        // Show alternate buttons
        /*foreach (GameObject altBtn in alternateButtonObjects)
        {
            altBtn.SetActive(true);
        }
        foreach (GameObject obj in alternateButtonObjects)
        {
            obj.SetActive(true);
        }

        // Reset logic for ButtonPress components on new buttons
        foreach (var button in buttons)
        {
            if (button != null)
            button.ResetButton();
        }

        // Move the player back to the start
        MovePlayerToStart();

        badEndingCanvas.SetActive(false);

        StartCountdown();
    }

    private void ResetRespawnableItems()
    {
        RespawnableItem[] items = FindObjectsByType<RespawnableItem>(FindObjectsSortMode.None);
        foreach (var item in items)
        {
            item.ForceRespawn();
        }
    }

    private void MovePlayerToStart()
    {
        GameObject player = GameObject.FindWithTag("Player");
        Transform startZone = GameObject.FindWithTag("StartZone").transform;

        if (player != null && startZone != null)
        {
            player.transform.position = startZone.position;
            player.transform.rotation = startZone.rotation;
        }
        else
        {
            Debug.LogWarning("Player or StartZone not found! Check your tags.");
        }
    }
}*/

//Latest
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class CountdownStarter : MonoBehaviour
{
    public TextMeshProUGUI countdownText;
    public GameObject goodEndingCanvas;
    public GameObject gameOverCanvas;
    public AudioSource systemRebootAudio;
    public AudioSource explosionAudio;
    public FadeController fadeController;
    public Transform checkpointLocation;
    public GameObject badEndingCanvas;
    public GameObject player; // Your VR rig or player object

    private float countdownTime = 120f;
    private bool countdownActive = false;
    private int buttonsPressed = 0;

    public ButtonPress[] buttons;

    public GameObject[] activeButtonObjects;   // initially visible
    public GameObject[] alternateButtonObjects; // initially hidden

    private void Start()
    {
        // Ensure alternate buttons are hidden at start
        foreach (GameObject obj in alternateButtonObjects)
        {
            obj.SetActive(false);
        }
    }

    void Update()
    {
        if (countdownActive)
        {
            countdownTime -= Time.deltaTime;
            UpdateCountdownDisplay();

            if (countdownTime <= 0f)
            {
                countdownTime = 0f; // clamp to 0
                countdownActive = false;
                StartCoroutine(BadEndingSequence());
            }
            UpdateCountdownDisplay();
        }
    }

    public void StartCountdown()
    {
        countdownActive = true;
        countdownTime = 120f; // 2 minutes
        UpdateCountdownDisplay();
    }

    public void RegisterButtonPress()
    {
        buttonsPressed++;
        Debug.Log($"Buttons pressed: {buttonsPressed}");

        if (buttonsPressed >= 1)//3) //I will try with just one button
        {
            countdownActive = false;
            StartCoroutine(GoodEndingSequence());
        }
    }

    private IEnumerator GoodEndingSequence()
    {
        Debug.Log("Good ending sequence started.");

        if (systemRebootAudio != null)
        {
            systemRebootAudio.Play();
        }

        yield return new WaitForSeconds(systemRebootAudio.clip.length);

        if (fadeController != null)
        {
            yield return fadeController.FadeToWhite();
        }

        goodEndingCanvas.SetActive(true);
    }

    private IEnumerator BadEndingSequence()
    {
        Debug.Log("Bad ending sequence started.");

        if (fadeController != null)
        {
            yield return fadeController.FadeToBlack();
        }

        if (explosionAudio != null)
        {
            explosionAudio.Play();
        }

        // Wait a little after explosion
        yield return new WaitForSeconds(2f);

        gameOverCanvas.SetActive(true);
    }

    private void UpdateCountdownDisplay()
    {
        int minutes = Mathf.FloorToInt(countdownTime / 60);
        int seconds = Mathf.FloorToInt(countdownTime % 60);
        countdownText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void RetryCountdown()
    {
        StartCoroutine(RestartSequence());
    }

    private IEnumerator RestartSequence()
    {
        gameOverCanvas.SetActive(false);

        if (fadeController != null)
        {
            yield return fadeController.FadeFromBlack(); // Fade back in
        }

        // Teleport player
        if (player != null && checkpointLocation != null)
        {
            player.transform.position = checkpointLocation.position;
            player.transform.rotation = checkpointLocation.rotation;
        }

        // Reset timer and button counter
        buttonsPressed = 0;
        countdownTime = 120f;
        countdownActive = true;
        UpdateCountdownDisplay();
    }

    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit();
    }

    /*public void Retry()
    {
        Debug.Log("Retrying...");

        // Reset timer and button press count
        countdownTime = 120f;
        countdownActive = false;
        buttonsPressed = 0;
        UpdateCountdownDisplay();

        // Reset button visuals/states
        foreach (ButtonPress button in buttons)
        {
            button.ResetButton();
        }

        // Move the player back to the start (if you already have that code)
        MovePlayerToStart();

        // Hide the bad ending canvas
        badEndingCanvas.SetActive(false);

        // Start countdown again
        StartCountdown();
        Debug.Log("Retrying...");

        countdownTime = 120f;
        countdownActive = false;
        buttonsPressed = 0;
        UpdateCountdownDisplay();

        // Hide active buttons
        /*foreach (GameObject btn in activeButtonObjects)
        {
            btn.SetActive(false);
        }
        foreach (GameObject obj in activeButtonObjects)
        {
            obj.SetActive(false);
        }

        // Show alternate buttons
        /*foreach (GameObject altBtn in alternateButtonObjects)
        {
            altBtn.SetActive(true);
        }
        foreach (GameObject obj in alternateButtonObjects)
        {
            obj.SetActive(true);
        }

        // Reset logic for ButtonPress components on new buttons
        foreach (var button in buttons)
        {
            if (button != null)
                button.ResetButton();
        }

        // Move the player back to the start
        MovePlayerToStart();

        badEndingCanvas.SetActive(false);

        StartCountdown();
    }*/

    /*public void Retry()
    {
        Debug.Log("Retrying...");

        // Stop countdown and reset values
        countdownTime = 120f;
        countdownActive = false;
        buttonsPressed = 0;
        UpdateCountdownDisplay();

        // Hide bad ending canvas
        badEndingCanvas.SetActive(false);
        gameOverCanvas.SetActive(false);

        // Hide initial active buttons and show alternate ones
        foreach (GameObject obj in activeButtonObjects)
            obj.SetActive(false);

        foreach (GameObject obj in alternateButtonObjects)
            obj.SetActive(true);

        // Reset all ButtonPress components in the scene (active + alternate)
        ButtonPress[] allButtons = FindObjectsByType<ButtonPress>(FindObjectsSortMode.None);
        foreach (ButtonPress button in allButtons)
        {
            button.ResetButton();
        }

        // Move the player back to the start
        MovePlayerToStart();

        // Start the countdown again
        StartCountdown();
    }*/

    public void Retry()
    {
        Debug.Log("Retrying...");

        // Stop countdown and reset values
        countdownTime = 120f;
        countdownActive = false;
        buttonsPressed = 0;
        UpdateCountdownDisplay();

        // Hide bad ending canvas
        badEndingCanvas.SetActive(false);
        gameOverCanvas.SetActive(false);

        // Hide initial active buttons and show alternate ones
        foreach (GameObject obj in activeButtonObjects)
            obj.SetActive(false);

        foreach (GameObject obj in alternateButtonObjects)
            obj.SetActive(true);

        // Reset all ButtonPress components in the scene (active + alternate)
        ButtonPress[] allButtons = FindObjectsByType<ButtonPress>(FindObjectsSortMode.None);
        foreach (ButtonPress button in allButtons)
        {
            button.ResetButton();
        }

        // Reset all ButtonPress components (active and alternate)
        foreach (GameObject obj in activeButtonObjects)
        {
            ButtonPress press = obj.GetComponent<ButtonPress>();
            if (press != null)
                press.ResetButton();
        }

        foreach (GameObject obj in alternateButtonObjects)
        {
            ButtonPress press = obj.GetComponent<ButtonPress>();
            if (press != null)
                press.ResetButton();
        }

        // Move the player back to the start
        MovePlayerToStart();

        // Start the countdown again
        StartCountdown();
    }

    private void ResetRespawnableItems()
    {
        RespawnableItem[] items = FindObjectsByType<RespawnableItem>(FindObjectsSortMode.None);
        foreach (var item in items)
        {
            item.ForceRespawn();
        }
    }

    private void MovePlayerToStart()
    {
        GameObject player = GameObject.FindWithTag("Player");
        Transform startZone = GameObject.FindWithTag("StartZone").transform;

        if (player != null && startZone != null)
        {
            player.transform.position = startZone.position;
            player.transform.rotation = startZone.rotation;
        }
        else
        {
            Debug.LogWarning("Player or StartZone not found! Check your tags.");
        }
    }
}