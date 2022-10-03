using System.Collections.Generic;
using Controllers;
using Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace HUD
{
    public class Sanctuary : MonoBehaviour
    {
        [Header("Game references")]
        public GameObject sanctuaryUI;
        public GameController gameController;
        
        [Header("Stats References")]
        public TextMeshProUGUI scoreText;
        public TextMeshProUGUI currencyText;
        public TextMeshProUGUI nameText;
        public Player.Player player;


        [Header("Upgrade Button References")]
        public GameObject upgradeMaxHealthButton;
        public GameObject upgradeWeaponDamageButton;
        public GameObject upgradeWeaponSpeedButton;
        public GameObject upgradeWeaponRangeButton;

        [Header("Story references")]
        public Image narrativeUI;
        public List<Sprite> storylets;
        
        private void Start()
        {
            if (!player) player = FindObjectOfType<Player.Player>();
        }

        public void ShowSanctuary(int currentRound)
        {
            sanctuaryUI.SetActive(true);
            UpdateSanctuary(currentRound);
        }

        public void CloseSanctuary()
        {
            gameController.Continue();
            sanctuaryUI.SetActive(false);
        }

        private void UpdateSanctuary(int currentRound)
        {
            //Get player stats
            PlayerStats stats = player.GetPlayerStats();

            //Update score and currency HUD text
            SetScoreText(stats.GetScore().ToString());
            SetCurrencyText(stats.GetCurrency().ToString());
            SetNameText(stats.GetName());
            
            //Update narrative image to current round
            narrativeUI.sprite = storylets[currentRound - 1];
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


        public void SetNameText(string newName)
        {
            nameText.SetText(newName);
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