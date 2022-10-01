using UnityEngine;

namespace Player
{
    public class Player : MonoBehaviour
    {
        public PlayerCombat playerCombat;
        public PlayerMovement playerMovement;
        public PlayerStats playerStats;

        private void Awake()
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
}