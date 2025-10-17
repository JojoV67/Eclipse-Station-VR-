using UnityEngine;
using TMPro;

public class PlantManager : MonoBehaviour
{
    public static PlantManager Instance;
    public TMP_Text uiText;
    private int totalPlants;
    private int collected = 0;

    void Awake()
    {
        Instance = this;
        totalPlants = FindObjectsByType<CollectiblePlant>(FindObjectsSortMode.None).Length;
        UpdateUI();
    }

    public void CollectPlant()
    {
        collected++;
        UpdateUI();
    }

    void UpdateUI()
    {
        if (uiText)
            uiText.text = $"{collected}/{totalPlants}";
    }
}
