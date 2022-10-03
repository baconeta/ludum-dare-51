using TMPro;
using UnityEngine;

namespace HUD
{
    public class GameUI : MonoBehaviour
    {
        public GameObject gameUI;
        public TextMeshProUGUI currencyText;

        [SerializeField]
        [Tooltip("The current health element for the health bar.")]
        public UnityEngine.UI.Slider healthBar;

        public Player.Player player;
        public GameObject mobileUI;

        public GameObject RoundClearedText;

        public void Start()
        {
            player = FindObjectOfType<Player.Player>();
            ShowUI();
            if (Controllers.InputController.isMobile) ShowMobileUI();
            else HideMobileUI();
        }

        public void ShowMobileUI()
        {
            mobileUI.SetActive(true);
        }

        public void HideMobileUI()
        {
            mobileUI.SetActive(false);
        }

        public void ShowUI()
        {
            gameUI.SetActive(true);
            UpdateUI();
        }

        public void HideUI()
        {
            gameUI.SetActive(false);
        }

        public void UpdateUI()
        {
            UpdateCurrency();
            UpdateHealth();
        }

        public void UpdateCurrency()
        {
            currencyText.SetText(player.GetPlayerStats().GetCurrency().ToString());
        }

        public void ShowRoundClearedText(bool visible)
        {
            if(RoundClearedText != null)
            RoundClearedText.SetActive(visible);
        }

        public void UpdateHealth()
        {
            healthBar.value = player.GetPlayerCombat().GetPlayerHealth() / (float)player.GetPlayerCombat().healthMax;
        }
    }
}