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
        /*
         * Upgrade Costs.
         */
        [Header("Upgrade Costs")]
        [SerializeField]
        [Tooltip("How much currency it costs to upgrade from level 0 to level 1")]
        public int firstUpgradeCost = 3;

        [SerializeField] [Tooltip("How much currency it costs to upgrade from level 1 to level 2")]
        public int secondUpgradeCost = 7;

        [SerializeField] [Tooltip("How much currency it costs to upgrade from level 2 to level 3")]
        public int thirdUpgradeCost = 12;

        [SerializeField] [Tooltip("How much currency it costs to upgrade from level 3 to level 4")]
        public int fourthUpgradeCost = 18;

        [SerializeField] [Tooltip("How much currency it costs to upgrade from level 4 to level 5")]
        public int fifthUpgradeCost = 25;

        [Header("Game references")]
        public GameObject sanctuaryUI;
        public GameController gameController;

        [Header("Stats References")]
        public TextMeshProUGUI scoreText;
        public TextMeshProUGUI currencyText;
        public TextMeshProUGUI nameText;
        public Player.Player player;
        private PlayerCombat _playerCombat;
        private PlayerStats _playerStats;
        
        [Header("Upgrade Button References")]
        public Button upgradeMaxHealthButton;
        public Button upgradeWeaponDamageButton;
        public Button upgradeWeaponRangeButton;
        public Button upgradeWeaponSpeedButton;

        [Header("Upgrade Cost References")]
        public TextMeshProUGUI upgradeMaxHealthCost;
        public TextMeshProUGUI upgradeWeaponDamageCost;
        public TextMeshProUGUI upgradeWeaponRangeCost;
        public TextMeshProUGUI upgradeWeaponSpeedCost;

        private void Start()
        {
            if (!player)
                player = FindObjectOfType<Player.Player>();

            _playerCombat = player.playerCombat;
            _playerStats = player.playerStats;
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
            UpdateUpgradesUI();
        }

        public void BuyUpgradeMaxHealth()
        {
            if (!CanBuyMaxHealthUpgrade())
                return;

            var upgradeCost = GetUpgradeCost(_playerCombat.GetHealthLevel());
            _playerStats.SpendCurrency(upgradeCost);
            _playerCombat.IncreaseHealthLevel();

            UpdateUpgradesUI();

            Debug.Log("Bought max health upgrade! New Level: " + _playerCombat.GetHealthLevel());
        }

        private bool CanBuyMaxHealthUpgrade()
        {
            if (!_playerCombat.CanIncreaseHealthLevel())
                return false;

            var upgradeCost = GetUpgradeCost(_playerCombat.GetHealthLevel());
            return CanAfford(upgradeCost);
        }

        public void BuyUpgradeWeaponDamage()
        {
            if (!CanBuyWeaponDamageUpgrade())
                return;

            var upgradeCost = GetUpgradeCost(_playerCombat.GetAttackDamageLevel());
            _playerStats.SpendCurrency(upgradeCost);
            _playerCombat.IncreaseAttackDamageLevel();

            UpdateUpgradesUI();

            Debug.Log("Bought weapon damage upgrade! New Level: " + _playerCombat.GetAttackDamageLevel());
        }

        private bool CanBuyWeaponDamageUpgrade()
        {
            if (!_playerCombat.CanIncreaseAttackDamageLevel())
                return false;

            var upgradeCost = GetUpgradeCost(_playerCombat.GetAttackDamageLevel());
            return CanAfford(upgradeCost);
        }

        public void BuyUpgradeWeaponRange()
        {
            if (!CanBuyWeaponRangeUpgrade())
                return;

            var upgradeCost = GetUpgradeCost(_playerCombat.GetAttackRangeLevel());
            _playerStats.SpendCurrency(upgradeCost);
            _playerCombat.IncreaseAttackRangeLevel();

            UpdateUpgradesUI();

            Debug.Log("Bought weapon range upgrade! New Level: " + _playerCombat.GetAttackRangeLevel());
        }

        private bool CanBuyWeaponRangeUpgrade()
        {
            if (!_playerCombat.CanIncreaseAttackRangeLevel())
                return false;

            var upgradeCost = GetUpgradeCost(_playerCombat.GetAttackRangeLevel());
            return CanAfford(upgradeCost);
        }

        public void BuyUpgradeWeaponSpeed()
        {
            if (!CanBuyWeaponSpeedUpgrade())
                return;

            var upgradeCost = GetUpgradeCost(_playerCombat.GetAttackSpeedLevel());
            _playerStats.SpendCurrency(upgradeCost);
            _playerCombat.IncreaseAttackSpeedLevel();

            UpdateUpgradesUI();

            Debug.Log("Bought weapon speed upgrade! New Level: " + _playerCombat.GetAttackSpeedLevel());
        }

        private bool CanBuyWeaponSpeedUpgrade()
        {
            if (!_playerCombat.CanIncreaseAttackSpeedLevel())
                return false;

            var upgradeCost = GetUpgradeCost(_playerCombat.GetAttackSpeedLevel());
            return CanAfford(upgradeCost);
        }

        private int GetUpgradeCost(int currentLevel) =>
            currentLevel switch
            {
                0 => firstUpgradeCost,
                1 => secondUpgradeCost,
                2 => thirdUpgradeCost,
                3 => fourthUpgradeCost,
                4 => fifthUpgradeCost,
                _ => 999
            };

        private bool CanAfford(int cost) => cost <= _playerStats.GetCurrency();

        private void UpdateUpgradesUI()
        {
            //Update score and currency HUD text
            SetScoreText(_playerStats.GetScore().ToString());
            SetCurrencyText(_playerStats.GetCurrency().ToString());
            SetNameText(_playerStats.GetName());

            UpdateUpgradeCostsText();
            UpdateUpgradeButtons();
        }

        private void UpdateUpgradeCostsText()
        {
            upgradeMaxHealthCost.text = "Cost: " + GetUpgradeCost(_playerCombat.GetHealthLevel()).ToString();
            upgradeWeaponDamageCost.text = "Cost: " + GetUpgradeCost(_playerCombat.GetAttackDamageLevel()).ToString();
            upgradeWeaponRangeCost.text = "Cost: " + GetUpgradeCost(_playerCombat.GetAttackRangeLevel()).ToString();
            upgradeWeaponSpeedCost.text = "Cost: " + GetUpgradeCost(_playerCombat.GetAttackSpeedLevel()).ToString();
        }

        private void UpdateUpgradeButtons()
        {
            upgradeMaxHealthButton.interactable = CanBuyMaxHealthUpgrade();
            upgradeWeaponRangeButton.interactable = CanBuyWeaponRangeUpgrade();
            upgradeWeaponDamageButton.interactable = CanBuyWeaponDamageUpgrade();
            upgradeWeaponSpeedButton.interactable = CanBuyWeaponSpeedUpgrade();
        }

        private void SetNameText(string newName)
        {
            nameText.SetText(newName);
        }

        private void SetScoreText(string text)
        {
            scoreText.SetText("Score: " + text);
        }

        private void SetCurrencyText(string text)
        {
            currencyText.SetText("Loot: " + text);
        }
    }
}