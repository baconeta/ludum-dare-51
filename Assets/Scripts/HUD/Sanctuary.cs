using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

namespace HUD
{
    public class Sanctuary : MonoBehaviour
    {
        public GameObject sanctuaryUI;
        public TextMeshProUGUI scoreText;
        public TextMeshProUGUI currencyText;
        public Player player;

        public GameObject upgradeMaxHealthButton;
        public GameObject upgradeWeaponDamageButton;
        public GameObject upgradeWeaponSpeedButton;
        public GameObject upgradeWeaponRangeButton;

        
        
        private void Start()
        {
            if(!player) player = FindObjectOfType<Player>();
        }

        public void ShowSanctuary()
        {
            sanctuaryUI.SetActive(true);
            UpdateSanctuary();
        }

        public void CloseSanctuary()
        {
            sanctuaryUI.SetActive(false);
        }

        public void UpdateSanctuary()
        {
            //Get player stats
            PlayerStats stats = player.GetPlayerStats();
            
            //Update score and currency HUD text
            SetScoreText(stats.GetScore().ToString());
            SetCurrencyText(stats.GetCurrency().ToString());
        }

        public void BuyUpgradeMaxHealth()
        {
            //max health cost
            //player.GetPlayerCombat().UpgradeCost blah blah
            Debug.Log("Buy Max Health");
        }
        
        public void BuyUpgradeWeaponDamage()
        {
            Debug.Log("Buy WeaponDamage");

        }
        
        public void BuyUpgradeWeaponRange()
        {
            Debug.Log("Buy WeaponRange");

        }
        
        public void BuyUpgradeWeaponSpeed()
        {
            Debug.Log("Buy WeaponSpeed");

        }
        
        
        
        public void SetScoreText(string text)
        {
            scoreText.SetText("Score: " + text);
        }

        public void SetCurrencyText(string text)
        {
            currencyText.SetText("Currency: " + text);
        }
    }
}