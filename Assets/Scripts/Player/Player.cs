using UnityEngine;
using UnityEngine.InputSystem;


namespace Player
{
    public class Player : MonoBehaviour
    {
        public PlayerCombat playerCombat;
        public PlayerMovement playerMovement;
        public PlayerStats playerStats;
        public PlayerInput playerInput;

        private void Awake()
        {
            playerCombat = GetComponent<PlayerCombat>();
            playerMovement = GetComponent<PlayerMovement>();
            playerStats = GetComponent<PlayerStats>();
            playerInput = GetComponent<PlayerInput>();
        }

        public PlayerInput GetPlayerInput()
        {
            return playerInput;
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