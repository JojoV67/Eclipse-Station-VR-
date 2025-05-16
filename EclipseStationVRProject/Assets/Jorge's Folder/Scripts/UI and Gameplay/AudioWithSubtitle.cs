/*using UnityEngine;
using TMPro;
using System.Collections;

public class AudioWithSubtitle : MonoBehaviour
{
    public AudioSource audioSource;
    public string subtitleText;
    public float subtitleDuration = 3f;
    public TextMeshProUGUI subtitleTMP;

    public void PlayLine()
    {
        if (audioSource != null && subtitleTMP != null)
        {
            audioSource.Play();
            StartCoroutine(ShowSubtitles());
        }
    }

    private IEnumerator ShowSubtitles()
    {
        subtitleTMP.text = subtitleText;
        subtitleTMP.gameObject.SetActive(true);
        yield return new WaitForSeconds(subtitleDuration);
        subtitleTMP.gameObject.SetActive(false);
    }
}*/

using UnityEngine;
using TMPro;
using System.Collections;

public class AudioWithSubtitle : MonoBehaviour
{
    public AudioSource audioSource;
    public TextMeshProUGUI subtitleText;
    [TextArea]
    public string subtitleLine;
    public float extraDisplayTime = 1f;

    private void Start()
    {
        subtitleText.text = ""; // Clear text at start
        //PlayAudioWithSubtitles(); Auto play when the scene starts
    }

    public void PlayAudioWithSubtitles()
    {
        StartCoroutine(PlayAudioAndShowSubtitles());
    }

    private IEnumerator PlayAudioAndShowSubtitles()
    {
        if (audioSource && subtitleText)
        {
            subtitleText.text = subtitleLine;
            audioSource.Play();

            yield return new WaitForSeconds(audioSource.clip.length + extraDisplayTime);

            subtitleText.text = "";
        }
    }
}

