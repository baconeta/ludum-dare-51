using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public PlayerCombat playerCombat;
    public PlayerMovement playerMovement;
    public PlayerStats playerStats;
    
    // Start is called before the first frame update
    void Start()
    {
        playerCombat = GetComponent<PlayerCombat>();
        playerMovement = GetComponent<PlayerMovement>();
        playerStats = GetComponent<PlayerStats>();
    }


    public PlayerCombat GetPlayerCombat()
    {
        return playerCombat;
    }

    public PlayerMovement GetPlayerMovement()
    {
        return playerMovement;
    }

    public PlayerStats GetPlayerStats()
    {
        return playerStats;
    }
}
