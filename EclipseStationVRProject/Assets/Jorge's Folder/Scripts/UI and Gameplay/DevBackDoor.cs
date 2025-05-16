/*using System.Collections.Generic;
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
}*/

/*using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class DevBackdoor : MonoBehaviour
{
    public float holdDuration = 2.5f; // Time to hold Y button
    private float holdTimer = 0f;

    void Update()
    {
        // Get left hand device
        var leftHand = InputSystem.GetDevice<UnityEngine.InputSystem.XR.XRController>(CommonUsages.LeftHand);

        if (leftHand != null && leftHand.TryGetFeatureValue(CommonUsages.secondaryButton, out bool isPressed))
        {
            if (isPressed)
            {
                holdTimer += Time.deltaTime;

                if (holdTimer >= holdDuration)
                {
                    SceneManager.LoadScene(0); // Replace with your menu scene index or name
                }
            }
            else
            {
                holdTimer = 0f;
            }
        }
    }
}*/

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR;

public class DevBackdoor : MonoBehaviour
{
    public float holdDuration = 3f;
    private float holdTimer = 0f;
    private bool isHolding = false;

    private InputDevice leftController;

    void Start()
    {
        // Get the left hand XR controller
        var leftHandedControllers = new List<InputDevice>();
        InputDevices.GetDevicesAtXRNode(XRNode.LeftHand, leftHandedControllers);

        if (leftHandedControllers.Count > 0)
        {
            leftController = leftHandedControllers[0];
        }
        else
        {
            Debug.LogWarning("Left controller not found!");
        }
    }

    void Update()
    {
        if (!leftController.isValid)
            return;

        bool yPressed = false;

        // Check for the Y button (secondary button on left controller)
        if (leftController.TryGetFeatureValue(CommonUsages.secondaryButton, out yPressed) && yPressed)
        {
            if (!isHolding)
            {
                isHolding = true;
                holdTimer = 0f;
            }

            holdTimer += Time.deltaTime;

            if (holdTimer >= holdDuration)
            {
                Debug.Log("Y button held long enough. Loading menu scene...");
                SceneManager.LoadScene(0);
            }
        }
        else
        {
            isHolding = false;
            holdTimer = 0f;
        }
    }
}
