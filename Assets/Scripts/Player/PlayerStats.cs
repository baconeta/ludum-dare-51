using UnityEngine;

namespace Player
{
    public class PlayerStats : MonoBehaviour
    {
        private string _playerName;
        private int _currency = 0;
        private int _score = 0;

        public string GetName()
        {
            return _playerName;
        }

        public void SetName(string newName)
        {
            Debug.Log(newName);
            _playerName = newName;
        }
        
        public int GetCurrency()
        {
            return _currency;
        }

        public void AddCurrency(int value)
        {
            _currency += value;
            Debug.Log(value + " added to player wallet!");
        }

        public bool SpendCurrency(int value)
        {
            //Check has enough currency
            if (_currency - value < 0) return false;

            //Spend currency
            _currency -= value;
            return true;
        }

        public void ResetCurrency()
        {
            _currency = 0;
        }

        public int GetScore()
        {
            return _score;
        }

        public void AddScore(int scoreToAdd)
        {
            _score += scoreToAdd;
        }

        public void ResetScore()
        {
            _score = 0;
        }
    }
}