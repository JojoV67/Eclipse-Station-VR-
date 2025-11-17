using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LevelPromptTrigger : MonoBehaviour
{
    public GameObject promptUIPrefab;
    public Transform popupSpawnPoint;
    public FadeScreen fadeScreen; // assign in Inspector
    public AudioClip startSound;
    public string nextSceneName = "FierySwampScene";
    public float delayBeforeSceneLoad = 5f;

    private GameObject currentPopup;

    private void OnTriggerEnter(Collider other)
    {
        // Detect touch (your XR hand/controller should have this tag)
        if (other.CompareTag("PlayerHand"))
        {
            if (currentPopup == null)
            {
                currentPopup = Instantiate(promptUIPrefab, popupSpawnPoint.position, popupSpawnPoint.rotation);
                currentPopup.GetComponent<LevelPromptUI>().Initialize(this);
            }
        }
    }

    public void StartLevel()
    {
        StartCoroutine(FadeAndLoad());
    }

    private IEnumerator FadeAndLoad()
    {
        if (fadeScreen != null)
            fadeScreen.FadeOut(); // same as PlantManager

        if (startSound != null)
            AudioSource.PlayClipAtPoint(startSound, Camera.main.transform.position);

        yield return new WaitForSeconds(delayBeforeSceneLoad);
        SceneManager.LoadScene(nextSceneName);
    }

    public void Cancel()
    {
        if (currentPopup != null)
        {
            Destroy(currentPopup);
        }
    }
}
