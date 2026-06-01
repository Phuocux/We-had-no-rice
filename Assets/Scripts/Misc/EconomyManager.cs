using TMPro;
using UnityEngine;

public class EconomyManager : Singleton<EconomyManager>
{
    [SerializeField] private TMP_Text goldText;
    private int currentGold = 0;

    public void UpdateCurrentGold()
    {
        currentGold += 1;
        if (goldText != null)
            goldText.text = currentGold.ToString("D3");
    }
}
