/*using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FadeController : MonoBehaviour
{
    public Image fadeImage; // Fullscreen white image (alpha 0 at start)
    public float fadeDuration = 2f;

    private void Start()
    {
        // Make sure it's fully transparent at start
        fadeImage.color = new Color(1, 1, 1, 0);
    }

    public IEnumerator FadeToWhite()
    {
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            float alpha = Mathf.Lerp(0, 1, elapsedTime / fadeDuration);
            fadeImage.color = new Color(1, 1, 1, alpha);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        fadeImage.color = new Color(1, 1, 1, 1); // Ensure fully white
    }

    public IEnumerator FadeToBlack()
    {
        float duration = 2f; // 2 seconds fade
        float timer = 0f;
        Color startColor = fadeImage.color;
        Color endColor = Color.black;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            fadeImage.color = Color.Lerp(startColor, endColor, timer / duration);
            yield return null;
        }
    }
}*/

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FadeController : MonoBehaviour
{
    public Image fadeImage; // Fullscreen UI Image

    public IEnumerator FadeToWhite()
    {
        yield return Fade(Color.clear, Color.white, 2f);
    }

    public IEnumerator FadeToBlack()
    {
        yield return Fade(Color.clear, Color.black, 2f);
    }

    public IEnumerator FadeFromBlack()
    {
        yield return Fade(Color.black, Color.clear, 2f);
    }

    private IEnumerator Fade(Color fromColor, Color toColor, float duration)
    {
        float timer = 0f;
        fadeImage.color = fromColor;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            fadeImage.color = Color.Lerp(fromColor, toColor, timer / duration);
            yield return null;
        }

        fadeImage.color = toColor;
    }
}


