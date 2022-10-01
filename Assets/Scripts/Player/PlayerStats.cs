using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    
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
}
