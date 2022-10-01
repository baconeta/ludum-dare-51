using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameUI : MonoBehaviour
{
    public GameObject gameUI;
    public TextMeshProUGUI currencyText;
    public TextMeshProUGUI healthText;
    public Player.Player player;

    public void Start()
    {
        player = FindObjectOfType<Player.Player>();
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
