using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Player;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class Customization : MonoBehaviour
{
    [Header("Placeholder Lists")] 
    public TextAsset namePlaceholders;
    public TextAsset animalPlaceholders;
    
    [Header("Player Inputs")]
    public string playerName;
    public string favouriteAnimal;

    public TMP_InputField playerNameInput;
    public TMP_InputField favouriteAnimalInput;


    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        RandomAnimalPlaceholder();
        RandomPlayerNamePlaceholder();
    }
    
    public void RandomAnimalPlaceholder()
    {
        //Get random animal
        string randomName = GetRandomFromFile("animal");
        
        //Get placeholder text to random animal
        favouriteAnimalInput.placeholder.GetComponent<TextMeshProUGUI>().text = randomName;
        
        //Clear any existing inputted text
        if(favouriteAnimalInput.text is not "")
            favouriteAnimalInput.text = "";
    }

    public void RandomPlayerNamePlaceholder()
    {
        //Get random name
        string randomName = GetRandomFromFile("playername");
        
        //Get placeholder text to random name
        playerNameInput.placeholder.GetComponent<TextMeshProUGUI>().text = randomName;
        
        //Clear any existing inputted text
        if(playerNameInput.text is not "")
            playerNameInput.text = "";
    }

    private string GetRandomFromFile(string type)
    {
        string[] placeholderText;
        string result = "";
        switch (type)
        {
            case "playername": // Get random player name
                placeholderText = namePlaceholders.text.Split(",");
                result = placeholderText[Random.Range(0, placeholderText.Length)];
                break;
            case "animal": // Get random animal name
                //Split CSV
                placeholderText = animalPlaceholders.text.Split(",");
                //Get random from CSV list
                result = placeholderText[Random.Range(0, placeholderText.Length)];
                break;
                
        }

        return result;
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
