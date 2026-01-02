using UnityEngine;
using System.IO.Ports; // Essential for Arduino
using System.Collections;

public class CrimeSceneManager : MonoBehaviour
{
    [Header("Arduino Configuration")]
    [Tooltip("The COM port your Arduino is connected to (Check Device Manager)")]
    public string portName = "COM3";
    public int baudRate = 9600;
    private SerialPort _serialStream;

    [Header("Game Design: Knife Indices")]
    [Tooltip("The ID of the knife that opens the box")]
    public int escapeKnifeIndex = 5;
    [Tooltip("The ID of the knife that belongs to the killer")]
    public int killerKnifeIndex = 7;

    [Header("The Identity Twist")]
    [Tooltip("The name spelled by the first letters of the clues")]
    public string correctKillerName = "HAN";
    private bool _isKillerIdentified = false;
    private string _currentGuess = "";

    [Header("Game State")]
    public int currentClueLevel = 1;
    private bool _isGameActive = true;

    void Start()
    {
        InitializeSerial();
    }

    private void InitializeSerial()
    {
        try
        {
            _serialStream = new SerialPort(portName, baudRate);
            _serialStream.ReadTimeout = 50;
            _serialStream.Open();
            Debug.Log("Successfully connected to Arduino on " + portName);
        }
        catch (System.Exception e)
        {
            Debug.LogError("Could not open Serial Port: " + e.Message);
        }
    }

    /// <summary>
    /// This is called when the VR player interacts with a knife.
    /// Link this to your XR Grab Interactable's "Select Entered" event.
    /// </summary>
    public void OnKnifeSelected(int selectedID)
    {
        if (!_isGameActive) return;

        Debug.Log("Player selected Knife #" + selectedID);

        // 1. Check if they found the 'Escape' knife
        if (selectedID == escapeKnifeIndex)
        {
            ResolveGame();
        }
        // 2. Check if they found the 'Killer' knife (for the twist)
        else if (selectedID == killerKnifeIndex)
        {
            Debug.Log("You found the killer's weapon... but is the name solved?");
            // Optionally trigger a visual cue in VR that this knife is special
            SendToArduino(selectedID); // Still drop it physically as a penalty for not picking the escape one? 
                                       // Or keep it held up? Depends on your design.
        }
        // 3. Wrong Choice
        else
        {
            Debug.Log("Wrong knife! Physical prop dropping...");
            SendToArduino(selectedID);
            AdvanceClueLevel();
        }
    }

    /// <summary>
    /// Call this from a VR Keyboard or UI when the player thinks they know the name.
    /// </summary>
    public void SubmitKillerName(string nameInput)
    {
        if (nameInput.ToUpper() == correctKillerName.ToUpper())
        {
            _isKillerIdentified = true;
            Debug.Log("KILLER IDENTIFIED: " + correctKillerName);
            // Trigger a sound or visual success in VR
        }
    }

    private void AdvanceClueLevel()
    {
        if (currentClueLevel < 3)
        {
            currentClueLevel++;
            Debug.Log("The Killer sends another clue... Level: " + currentClueLevel);
        }
    }

    private void ResolveGame()
    {
        _isGameActive = false;

        if (_isKillerIdentified)
        {
            Debug.Log("TRUE ENDING: You escaped and unmasked the killer!");
            // Send special code '99' to Arduino to release the hand-trap motor
            SendToArduino(99);
        }
        else
        {
            Debug.Log("STANDARD ENDING: You escaped, but the killer is still out there...");
            SendToArduino(99);
        }
    }

    private void SendToArduino(int value)
    {
        if (_serialStream != null && _serialStream.IsOpen)
        {
            _serialStream.Write(value.ToString());
            Debug.Log("Sent to Arduino: " + value);
        }
    }

    private void OnApplicationQuit()
    {
        if (_serialStream != null && _serialStream.IsOpen)
        {
            _serialStream.Close();
        }
    }
}