using UnityEngine;

public class ResetButton : MonoBehaviour
{
    public FinalSequenceManager sequenceManager;

    private bool hasBeenPressed = false;

    void OnTriggerEnter(Collider other)
    {
        if(!hasBeenPressed && other.CompareTag("Player"))
        {
            hasBeenPressed = true;
            sequenceManager.RegisterReset();
        }
    }
}
