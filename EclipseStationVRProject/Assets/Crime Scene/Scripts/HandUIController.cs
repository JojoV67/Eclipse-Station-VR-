using UnityEngine;
using UnityEngine.UI;

public class HandUIController : MonoBehaviour
{
    public Image[] fingerImages; // order them left → right
    private int fingersLost = 0;

    public void LoseFinger()
    {
        if (fingersLost >= fingerImages.Length)
            return;

        fingerImages[fingersLost].enabled = false;
        fingersLost++;
    }
}
