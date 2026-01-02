using UnityEngine;

public class KnifeSelectionManager : MonoBehaviour
{
    public static KnifeSelectionManager Instance;
    public KnifeSelectable currentKnife;

    void Awake()
    {
        Instance = this;
    }

    public void SelectKnife(KnifeSelectable knife)
    {
        currentKnife = knife;
    }
}
