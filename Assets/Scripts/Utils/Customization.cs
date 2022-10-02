using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Player;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class Customization : MonoBehaviour
{
    [Header("Word Lists")]
    [SerializeField][Tooltip("The comma-separated list of descriptors to generate names from.")]
    private TextAsset listOfAdjectives;
    [SerializeField][Tooltip("The comma-separated list of nouns to generate names from.")]
    private TextAsset listOfNouns;
    
    [Header("UI Input Fields")]
    [SerializeField][Tooltip("The input field for names.")]
    private TMP_InputField playerNameInput;

    [Header("Settings")]
    [SerializeField][Tooltip("Generate a random name IMMEDIATELY.")]
    private bool generatePlaceholdersImmediately = false;
    [SerializeField][Tooltip("The default text to show as a placeholder. Won't be used as a name. Ignored if the above is set to true.")]
    private string defaultPlaceholderText = "Name";

    private static string cachedName = "";

    private void Start()
    {
        // This object will destroy itself once it finds a player to off-load stats to.
        DontDestroyOnLoad(gameObject);
        // Setup the initial placeholder text name.
        if (generatePlaceholdersImmediately)
        {
            GenerateRandomPlaceholderName();
        }
        else
        {
            UpdatePlaceholderName(defaultPlaceholderText);
        }
    }

    // We run this on FixedUpdate to reduce the amount of checks per second.
    private void FixedUpdate()
    {
        // Once we are in the main scene (instead of the start screen).
        if (SceneManager.GetActiveScene().name == "MainScene")
        {
            // Save the currently-stored changes.
            SaveCacheToPlayer();
        
            // Unity version of "destroy this".
            Destroy(gameObject);
        }
    }

    public void GenerateRandomPlaceholderName()
    {
        UpdatePlaceholderName(GeneratePlayerName());
    }

    public void UpdatePlaceholderName(string newName)
    {
        // Set placeholder text to the new name.
        playerNameInput.placeholder.GetComponent<TextMeshProUGUI>().text = newName;
        
        // Clear any text from the user input field.
        if (playerNameInput.text is not "")
            playerNameInput.text = "";
    }

    public string GeneratePlayerName()
    {
        // Get random name
        StringBuilder generatedName = new StringBuilder();
        generatedName.Append(GetRandomAdjectiveFromFile());
        generatedName.Append(GetRandomNounFromFile());

        return generatedName.ToString();
    }

    private string GetRandomAdjectiveFromFile()
    {
        string[] words = listOfAdjectives.text.Split(",");
        return words[Random.Range(0, words.Length)];
    }

    private string GetRandomNounFromFile()
    {
        string[] words = listOfNouns.text.Split(",");
        return words[Random.Range(0, words.Length)];
    }

    public string GetCurrentName()
    {
        // Check if the text field is empty.
        if (string.IsNullOrWhiteSpace(playerNameInput.text))
        {
            // Get placeholder name
            var placeholderName = playerNameInput.placeholder.GetComponent<TextMeshProUGUI>().text;
            // If no placeholder - Get random placeholder
            return (placeholderName == defaultPlaceholderText) ? GeneratePlayerName() : placeholderName;
        }
        // Else, use player choice.
        return playerNameInput.text;
    }

    public void CacheCustomization()
    {
        cachedName = GetCurrentName();
    }

    // Finds the player stat tracker, and copies vital information across.
    public void SaveCacheToPlayer()
    {
        FindObjectOfType<PlayerStats>().SetName(cachedName);
    }
}