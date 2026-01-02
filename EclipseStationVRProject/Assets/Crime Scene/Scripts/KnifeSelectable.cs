using UnityEngine;

public class KnifeSelectable : MonoBehaviour
{
    public bool isCorrectKnife = false;

    private bool toggled = false;
    private Vector3 originalPos;
    private Vector3 raisedPos;

    private Vector3 originalEuler;
    private Vector3 raisedEuler;

    void Start()
    {
        originalPos = transform.position;
        originalEuler = transform.eulerAngles;

        raisedPos = originalPos + new Vector3(0f, 0.48f, 0f);
        raisedEuler = new Vector3(
            originalEuler.x,
            originalEuler.y,
            originalEuler.z - 90f
        );
    }

    public void ToggleKnife()
    {
        if (!toggled)
        {
            transform.position = raisedPos;
            transform.eulerAngles = raisedEuler;
        }
        else
        {
            transform.position = originalPos;
            transform.eulerAngles = originalEuler;
        }

        toggled = !toggled;
    }
}
