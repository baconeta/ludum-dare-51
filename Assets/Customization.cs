using System;
using System.Collections;
using System.Collections.Generic;
using Player;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Customization : MonoBehaviour
{
    [Header("Player Inputs")]
    public string playerName;
    public string favouriteAnimal;

    public TMP_InputField playerNameInput;
    public TMP_InputField favouriteAnimalInput;


    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        
    }

    public void ConfirmChoices()
    {
        //If custom text box is empty
        if (string.IsNullOrWhiteSpace(playerNameInput.text))
        {
            //Get placeholder name
            string placeholderName = playerNameInput.placeholder.GetComponent<TextMeshProUGUI>().text;
            playerName = placeholderName;
        }
        else //Use player choice
        {
            playerName = playerNameInput.text;
        }
        
        //If custom text box is empty
        if (string.IsNullOrWhiteSpace(favouriteAnimalInput.text))
        {
            //Get placeholder name
            string placeholderAnimal = favouriteAnimalInput.placeholder.GetComponent<TextMeshProUGUI>().text;
            favouriteAnimal = placeholderAnimal;
        }
        else //Use player choice
        {
            favouriteAnimal = favouriteAnimalInput.text;
        }

    }

    private void FixedUpdate()
    {
        //If Main scene check
        if (SceneManager.GetActiveScene().name == "MainScene")
        {
            SaveCustomizationToPlayer();
        }
        
    }

    //Finds player stats and copies vital information across
    public void SaveCustomizationToPlayer()
    {
        //Save variables to player stats
        FindObjectOfType<PlayerStats>().SetPlayerInfo(playerName, favouriteAnimal);
        
        //Destroy this
        Destroy(gameObject);
        
    }
}
