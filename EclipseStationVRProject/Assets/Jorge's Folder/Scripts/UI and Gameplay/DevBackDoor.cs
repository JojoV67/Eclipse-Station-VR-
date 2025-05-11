using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR;

public class DevBackdoor : MonoBehaviour
{
    public float holdTime = 3f;
    private float timer = 0f;
    private bool isHolding = false;

    private InputDevice leftController;

    void Start()
    {
        // Get the left-hand controller (menu button is usually on the left)
        var inputDevices = new List<InputDevice>();
        InputDevices.GetDevicesAtXRNode(XRNode.LeftHand, inputDevices);
        if (inputDevices.Count > 0)
        {
            leftController = inputDevices[0];
        }
    }

    void Update()
    {
        if (!leftController.isValid)
            return;

        bool menuButtonPressed = false;
        if (leftController.TryGetFeatureValue(CommonUsages.menuButton, out menuButtonPressed) && menuButtonPressed)
        {
            if (!isHolding)
            {
                isHolding = true;
                timer = 0f;
            }

            timer += Time.deltaTime;

            if (timer >= holdTime)
            {
                Debug.Log("Menu button held long enough. Returning to main menu.");
                SceneManager.LoadScene(0);
            }
        }
        else
        {
            isHolding = false;
            timer = 0f;
        }
    }
}
