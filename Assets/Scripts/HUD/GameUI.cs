using TMPro;
using UnityEngine;

public class GameUI : MonoBehaviour
{
    public GameObject gameUI;
    public TextMeshProUGUI currencyText;
    [SerializeField] [Tooltip("The current health element for the health bar.")]
    public UnityEngine.UI.Slider healthBar;
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
        UpdateCurrency();
        UpdateHealth();
    }

    public void UpdateCurrency()
    {
        currencyText.SetText("Currency: " + player.GetPlayerStats().GetCurrency());
    }

    public void UpdateHealth()
    {
        Debug.Log("UpdateHealth");
        healthBar.value = player.GetPlayerCombat().GetPlayerHealth() / (float) player.GetPlayerCombat().healthMax;
        Debug.Log(healthBar.value);
    }
}