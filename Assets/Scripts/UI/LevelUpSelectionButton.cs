using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelUpSelectionButton : MonoBehaviour
{
    public Image WeaponImage, DescriptionImage;
    public TMP_Text WeaponNameText, WeaponDescription, WeaponLevelText;
    private Weapon assignedWeapon;

    public void UpdateButtonDisplay(Weapon TheWeapon)
    {
        if (TheWeapon == null)
        {
            Debug.LogError("TheWeapon is null in UpdateButtonDisplay!");
            return;
        }

        if (TheWeapon.gameObject.activeSelf)
        {
            WeaponDescription.text = TheWeapon.Stats[TheWeapon.WeaponLevel].UpgradeText;
            WeaponImage.sprite = TheWeapon.icon;
            DescriptionImage.sprite = TheWeapon.icon;

            WeaponNameText.text = TheWeapon.name;
            WeaponLevelText.text = "LEVEL " + (TheWeapon.WeaponLevel + 1);
        }
        else
        {
            WeaponDescription.text = TheWeapon.Stats[TheWeapon.WeaponLevel].UpgradeText;
            WeaponImage.sprite = TheWeapon.icon;
            DescriptionImage.sprite = TheWeapon.icon;

            WeaponNameText.text = TheWeapon.name;
            WeaponLevelText.text = "UNLOCK " + (TheWeapon.WeaponLevel + 1); // Updated to display the correct unlock level
        }
        assignedWeapon = TheWeapon;
    }

    public void SelectUpgrade()
    {
        if (assignedWeapon != null)
        {
            if (assignedWeapon.gameObject.activeSelf)
            {
                assignedWeapon.LevelUp();
            }
            else
            {
                PlayerController.instance.AddWeapon(assignedWeapon);
            }

            UIController.instance.LevelUpPanel.SetActive(false);
            Time.timeScale = 1f;
        }
        else
        {
            Debug.LogError("assignedWeapon is null in SelectUpgrade!");
        }
    }

    public void ConfirmUpgrade()
    {
        for (int i = 0; i < UIController.instance.LevelUpButtons.Length; i++)
        {
            if (UIController.instance.DescriptionButton[i].gameObject.activeInHierarchy)
            {
                SFXManager.instance.PlaySFX(13);
                UIController.instance.LevelUpButtons[i].SelectUpgrade();
                return;
            }
        }
    }
}
