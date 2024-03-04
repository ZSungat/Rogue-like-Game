using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerStatUpgradeDisplay : MonoBehaviour
{
    public TMP_Text ValueText, CostText;

    public Button UpgradeButton;


    public void UpdateDisplay(int Cost, float OldValue, float NewValue)
    {
        ValueText.text = OldValue.ToString("F1") + "->" + NewValue.ToString("F1");
        CostText.text = "" + Cost;
        if (Cost <= CoinController.instance.CurrentCoins)
        {
            UpgradeButton.interactable = true;
        }
        else
        {
            UpgradeButton.interactable = false;
        }
    }

    public void ShowMaxLevel()
    {
        ValueText.color = Color.red;
        ValueText.text = "Max Level";

        CostText.color = Color.red;
        CostText.text = "Max Level";

        UpgradeButton.interactable = false;
    }
}
