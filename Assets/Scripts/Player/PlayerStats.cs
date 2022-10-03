using UnityEngine;

namespace Player
{
    public class PlayerStats : MonoBehaviour
    {
        private string _playerName;
        private int _currency = 0;
        private int _score = 0;

        private GameUI _gameUI;

        private void Start()
        {
            _gameUI = GetComponent<Player>().gameUI;
        }

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
            _gameUI.UpdateCurrency();
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
        public void CalculateScore()
        {
            // puffball, mushroom, seed need counter that increases on kill
            // puffball score 10
            // seed score 12
            // mushroom score 15
            // boss score 50
            // minus overall time to complete game
            
            //var puffballScore = puffballsKilled+puffsCollected*10;
            //var seedScore = seedsKilled+seedsCollected*12;
            //var mushroomScore = mushroomsKilled+seedsCollected*15;
            //var bossScore = bossKilled*50;
            //var overallTime = timetoComplete;
            
            
            
            //var overallScore = puffballScore + seedScore + mushroomScore + bossScore - overallTime;
            // AddScore(overallScore);
        }
    }
}