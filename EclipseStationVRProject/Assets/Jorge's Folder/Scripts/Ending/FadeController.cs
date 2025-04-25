using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FadeController : MonoBehaviour
{
    public Image fadeOverlay; // Assign a full-screen UI Image
    public float fadeDuration = 1.5f;

    public void FadeToWhite()
    {
        StartCoroutine(Fade(Color.clear, Color.white));
    }

    public void FadeToBlack()
    {
        StartCoroutine(Fade(Color.clear, Color.black));
    }

    IEnumerator Fade(Color from, Color to)
    {
        float timer = 0f;
        fadeOverlay.gameObject.SetActive(true);

        while (timer <= fadeDuration)
        {
            fadeOverlay.color = Color.Lerp(from, to, timer / fadeDuration);
            timer += Time.deltaTime;
            yield return null;
        }

        fadeOverlay.color = to;
    }
}

