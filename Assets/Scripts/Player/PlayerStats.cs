using UnityEngine;

namespace Player
{
    public class PlayerStats : MonoBehaviour
    {
        public string playerName;

        public string favouriteAnimal;
        
        public int currency = 0;

        public int score = 0;

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

        public string GetName()
        {
            return playerName;
        }
        
        public int GetCurrency()
        {
            return currency;
        }

        public int GetScore()
        {
            return score;
        }

        public void AddScore(int scoreToAdd)
        {
            score += scoreToAdd;
        }

        public void SetPlayerInfo(string newName, string animal)
        {
            playerName = newName;
            favouriteAnimal = animal;
        }
    }
}