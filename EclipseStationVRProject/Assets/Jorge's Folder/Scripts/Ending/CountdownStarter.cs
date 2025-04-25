using UnityEngine;
using TMPro;

public class CountdownStarter : MonoBehaviour
{
    public float countdownTime = 120f;
    private float currentTime;
    private bool timerActive = false;

    public TextMeshProUGUI countdownText;

    // Update is called once per frame
    void Update()
    {
        if (timerActive)
        {
            currentTime -= Time.deltaTime;
            if (currentTime < 0f)
            {
                currentTime = 0f;
                timerActive = false;
                countdownText.text = "Reboot Sequence Failed. Self-destruct sequence enabled.";
            }
            else
            {
                int minutes = Mathf.FloorToInt(currentTime / 60f);
                int seconds = Mathf.FloorToInt(currentTime % 60f);
                countdownText.text = $"{minutes:00}:{seconds:00}";
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !timerActive)
        {
            Debug.Log("Player entered the countdown trigger!");
            currentTime = countdownTime;
            timerActive = true;
            gameObject.SetActive(false);
        }
    }
}
