using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelUpSelectionButton : MonoBehaviour
{
    public TMP_Text UpgradeDescText, NameWeaponText, LevelWeaponText;
    public Image WeaponIcons, DescriptionImage;
    private Weapon assignedWeapon;


    public void UpdateButtonDisplay(Weapon TheWeapon)
    {
        if (TheWeapon.gameObject.activeSelf == true)
        {
            UpgradeDescText.text = TheWeapon.Stats[TheWeapon.WeaponLevel].UpgradeText;
            WeaponIcons.sprite = TheWeapon.icon;
            DescriptionImage.sprite = TheWeapon.icon;

            NameWeaponText.text = TheWeapon.name;
            LevelWeaponText.text = "LEVEL " + TheWeapon.WeaponLevel;
        }
        else
        {
            UpgradeDescText.text = "Unlock " + TheWeapon.name;
            WeaponIcons.sprite = TheWeapon.icon;
            DescriptionImage.sprite = TheWeapon.icon;

            NameWeaponText.text = TheWeapon.name;
        }
        assignedWeapon = TheWeapon;
    }
    public void SelectUpgrade()
    {
        if (assignedWeapon != null)
        {
            if (assignedWeapon.gameObject.activeSelf == true)
            {
                assignedWeapon.LevelUp();
            }
            else
            {
                PlayerMovement.instance.AddWeapon(assignedWeapon);
            }

            UIController.instance.LevelUpPanel.SetActive(false);
            Time.timeScale = 1f;
        }
    }
}