using UnityEngine;

public class ConfirmKnifeButton : MonoBehaviour
{
    public AudioSource correctSound;
    public AudioSource wrongSound;
    public HandUIController handUI;

    public void ConfirmSelection()
    {
        var knife = KnifeSelectionManager.Instance.currentKnife;

        if (knife == null)
            return;

        knife.ToggleKnife();

        if (knife.isCorrectKnife)
        {
            correctSound.Play();
        }
        else
        {
            wrongSound.Play();
            handUI.LoseFinger();
        }
    }
}