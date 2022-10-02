using TMPro;
using UnityEngine;

public class GameUI : MonoBehaviour
{
    public GameObject gameUI;
    public TextMeshProUGUI currencyText;
    public TextMeshProUGUI healthText;
    public Player.Player player;
    public GameObject mobileUI;

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
        currencyText.SetText("Currency: " + player.GetPlayerStats().GetCurrency());
        healthText.SetText("Health: " + player.GetPlayerCombat().GetPlayerHealth());
    }
}