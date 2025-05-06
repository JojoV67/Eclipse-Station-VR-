using UnityEngine;

public class StartCountdownTrigger : MonoBehaviour
{
    /*private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CountdownStarter countdown = FindFirstObjectByType<CountdownStarter>();
            if (countdown != null)
            {
                countdown.StartCountdown();
                Debug.Log("Countdown started by trigger zone!");
            }
            gameObject.SetActive(false); // Disable trigger so it doesn't fire again
        }
    }*/
    private bool hasTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!hasTriggered && other.CompareTag("Player"))
        {
            CountdownStarter countdownStarter = FindAnyObjectByType<CountdownStarter>(); 
            if (countdownStarter != null)
            {
                countdownStarter.StartCountdown();
            }
            hasTriggered = true; // Prevents triggering multiple times
        }
    }
}

