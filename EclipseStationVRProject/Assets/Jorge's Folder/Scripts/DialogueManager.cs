using UnityEngine;
using System.Collections;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    public TMP_Text subtitleText;
    public Canvas subtitleCanvas;
    public AudioSource audioSource;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void StartDialogue(DialogueLine[] lines)
    {
        StartCoroutine(PlayDialogue(lines));
    }

    private IEnumerator PlayDialogue(DialogueLine[] lines)
    {
        subtitleCanvas.enabled = true;

        foreach (var line in lines)
        {
            subtitleText.text = line.subtitleText;

            if (line.voiceClip != null)
            {
                audioSource.clip = line.voiceClip;
                audioSource.Play();
                yield return new WaitForSeconds(line.voiceClip.length);
            }
            else
            {
                yield return new WaitForSeconds(2f); //Default wait time if no audio
            }
        }
        subtitleText.text = "";
        subtitleCanvas.enabled = false;
    }
}
