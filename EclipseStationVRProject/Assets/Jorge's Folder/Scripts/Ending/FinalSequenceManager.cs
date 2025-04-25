using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;

public class FinalSequenceManager : MonoBehaviour
{
    [Header("Timer Settings")]
    public float countdownTime = 120f; // 2 minutes
    private float currentTime;
    private bool isCountingDown = false;

    [Header("Computer Reset Tracking")]
    private int resetCount = 0;
    public int totalResetsNeeded = 3;

    [Header("UI and Audio")]
    public TextMeshProUGUI countdownText;
    public GameObject gameOverScreen;
    public AudioSource systemVoice;
    public AudioSource playerVoice;
    public AudioSource explosionAudio;

    [Header("Fade")]
    public FadeController fadeController; // fade to white/black
    public string mainMenuScene = "MainMenu";

    [Header("Checkpoint")]
    public Transform player;
    public Transform checkpointLocation;

    private bool sequenceEnded = false;

    void Update()
    {
        if (isCountingDown && !sequenceEnded)
        {
            currentTime -= Time.deltaTime;
            UpdateCountdownUI();

            if (currentTime <= 0f)
            {
                TriggerFailure();
            }
        }
    }

    public void StartFinalSequence()
    {
        Debug.Log("Final sequence started!");
        resetCount = 0;
        currentTime = countdownTime;
        isCountingDown = true;
        sequenceEnded = false;
        gameOverScreen.SetActive(false);
    }

    public void RegisterReset()
    {
        if (sequenceEnded) return;

        resetCount++;
        if (resetCount >= totalResetsNeeded)
        {
            TriggerSuccess();
        }
    }

    void TriggerSuccess()
    {
        sequenceEnded = true;
        isCountingDown = false;
        Debug.Log("All computers reset — player wins!");
        StartCoroutine(HandleSuccess());
    }

    void TriggerFailure()
    {
        sequenceEnded = true;
        isCountingDown = false;
        Debug.Log("Time's up — player loses.");
        StartCoroutine(HandleFailure());
    }

    IEnumerator HandleSuccess()
    {
        systemVoice.Play();
        yield return new WaitForSeconds(systemVoice.clip.length + 0.5f);

        playerVoice.Play();
        yield return new WaitForSeconds(playerVoice.clip.length + 0.5f);

        fadeController.FadeToWhite();
        yield return new WaitForSeconds(2f);

        // Show "This odyssey will continue..."
        // Then return to menu
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(mainMenuScene);
    }

    IEnumerator HandleFailure()
    {
        fadeController.FadeToBlack();
        explosionAudio.Play();

        yield return new WaitForSeconds(2f);
        gameOverScreen.SetActive(true);
    }

    void UpdateCountdownUI()
    {
        int minutes = Mathf.FloorToInt(currentTime / 60);
        int seconds = Mathf.FloorToInt(currentTime % 60);
        countdownText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void Retry()
    {
        Debug.Log("Retrying final sequence...");
        player.position = checkpointLocation.position;
        StartFinalSequence();
    }

    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit();
    }
}

