using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CrystalManager : MonoBehaviour
{
    public static CrystalManager instance;

    public int totalCrystals = 3;
    private int collected = 0;

    [Header("UI")]
    public TMP_Text crystalText;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        UpdateUI();
    }

    public void CollectCrystal()
    {
        collected++;
        UpdateUI();

        if (collected >= totalCrystals)
        {
            Debug.Log("All crystals collected! Level Complete!");
            // TODO: trigger win sequence here (sound, fade, next scene, etc.)
        }
    }

    private void UpdateUI()
    {
        if (crystalText != null)
            crystalText.text = $"{collected}/{totalCrystals}";
    }
}
