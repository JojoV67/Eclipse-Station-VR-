using UnityEngine;
using System.IO.Ports; // Requires .NET Framework setting

public class ArduinoConnector : MonoBehaviour
{
    SerialPort stream = new SerialPort("COM3", 9600); // Change COM3 to your Arduino port

    void Start()
    {
        stream.Open();
    }

    public void DropKnife(int knifeIndex)
    {
        if (stream.IsOpen)
        {
            // Send the index of the knife to drop (e.g., "1", "2", "3")
            stream.Write(knifeIndex.ToString());
            Debug.Log("Signal sent to Arduino: " + knifeIndex);
        }
    }

    void OnApplicationQuit()
    {
        stream.Close();
    }
}