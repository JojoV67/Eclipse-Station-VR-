using UnityEngine;
using UnityEngine.UI;

public class LevelPromptUI : MonoBehaviour
{
    public Button startButton;
    public Button cancelButton;
    private LevelPromptTrigger triggerRef;

    public void Initialize(LevelPromptTrigger trigger)
    {
        triggerRef = trigger;
        startButton.onClick.AddListener(triggerRef.StartLevel);
        cancelButton.onClick.AddListener(triggerRef.Cancel);
    }
}