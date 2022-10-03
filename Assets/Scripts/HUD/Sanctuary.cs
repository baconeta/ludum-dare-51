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
        public GameObject upgradeMaxHealthButton;
        public GameObject upgradeWeaponDamageButton;
        public GameObject upgradeWeaponSpeedButton;
        public GameObject upgradeWeaponRangeButton;

        [Header("Story references")]
        public Image narrativeUI;
        public List<Sprite> storylets;
        
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
            //Update narrative image to current round
            narrativeUI.sprite = storylets[currentRound - 1];

            UpdateLabels();
        }

        private void UpdateLabels()
        {
            //Update score and currency HUD text
            SetScoreText(_playerStats.GetScore().ToString());
            SetCurrencyText(_playerStats.GetCurrency().ToString());
            SetNameText(_playerStats.GetName());
        }

        public void BuyUpgradeMaxHealth()
        {
            if (!CanBuyMaxHealthUpgrade())
                return;

            var upgradeCost = GetUpgradeCost(_playerCombat.GetHealthLevel());
            _playerStats.SpendCurrency(upgradeCost);
            _playerCombat.IncreaseHealthLevel();

            UpdateLabels();

            Debug.Log("Bought max health upgrade! New Level: " + _playerCombat.GetHealthLevel());
        }

        private bool CanBuyMaxHealthUpgrade()
        {
            if (!_playerCombat.CanIncreaseHealthLevel())
                return true;

            var upgradeCost = GetUpgradeCost(_playerCombat.GetHealthLevel());
            return upgradeCost <= _playerStats.GetCurrency();
        }

        private int GetUpgradeCost(int currentLevel) =>
            currentLevel switch
            {
                0 => firstUpgradeCost,
                1 => secondUpgradeCost,
                2 => thirdUpgradeCost,
                3 => fourthUpgradeCost,
                4 => fifthUpgradeCost,
                _ => int.MaxValue
            };

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
            currencyText.SetText("Loot: " + text);
        }
    }
}