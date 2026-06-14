using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EconomyManager : Singleton<EconomyManager>
{
    private TMP_Text goldText;
    private int currentGold = 0;

    const string COIN_AMOUNT_TEXT = "Gold Amount Text";

    public void UpdateCurrentGold() {
        currentGold += 1;

        if (goldText == null) {
            GameObject obj = GameObject.Find(COIN_AMOUNT_TEXT);

            if (obj == null)
            {
                Debug.LogError("Khong thay GameObject: " + COIN_AMOUNT_TEXT);
                return;
            }
            goldText = GameObject.Find(COIN_AMOUNT_TEXT).GetComponent<TMP_Text>();
        }

        goldText.text = currentGold.ToString("D3");
    }
}
