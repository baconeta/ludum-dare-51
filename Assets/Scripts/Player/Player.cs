using UnityEngine;

namespace Player
{
    public class Player : MonoBehaviour
    {
        public int currency = 0;

        public PlayerCombat playerCombat;
        public PlayerMovement playerMovement;

        // Start is called before the first frame update
        private void Start()
        {
            playerCombat = GetComponent<PlayerCombat>();
            playerMovement = GetComponent<PlayerMovement>();
        }


        public void AddCurrency(int value)
        {
            currency += value;
            Debug.Log(value + " added to player wallet!");
        }

        public bool SpendCurrency(int value)
        {
            //Check has enough currency
            if (currency - value < 0) return false;

            //Spend currency
            currency -= value;
            return true;
        }

        public PlayerCombat GetPlayerCombat()
        {
            return playerCombat;
        }

        public PlayerMovement GetPlayerMovement()
        {
            return playerMovement;
        }
    }
}