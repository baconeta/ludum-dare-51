using System;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class GlobalScoreManager : MonoBehaviour
{
    [Header("Text Fields")]
    [Tooltip("Highscore name column")] [SerializeField]
    private Text nameColumn;
    [Tooltip("Highscore score column")] [SerializeField]
    private Text scoreColumn;
    
    private const string SubmitScoreUri = "https://ludum-dare-51-score-server.herokuapp.com/api/scores?user={0}&score={1}";
    private const string RetrieveScoresUri = "https://ludum-dare-51-score-server.herokuapp.com/api/scores";

    private string _defaultText =
        $"Loading...{Environment.NewLine}Loading...{Environment.NewLine}Loading...{Environment.NewLine}Loading...{Environment.NewLine}" +
        $"Loading...{Environment.NewLine}Loading...{Environment.NewLine}Loading...{Environment.NewLine}Loading...{Environment.NewLine}" +
        $"Loading...{Environment.NewLine}Loading...";

    // The callback used to update text with global score information that we retrieve from the server.
    private delegate void Callback(ScoreEntryList entryList);

    void Start()
    {
        // Set the text of the columns to display "Loading..." on each line.
        nameColumn.text = _defaultText;
        scoreColumn.text = _defaultText;
        // Request global score information from the server, and provide a callback for when we get that information.
        Callback callback = new Callback(UpdateTextFields);
        StartCoroutine(GetGlobalScoresRequest(callback));
    }

    // Update the text fields with the information that we received from the score server.
    private void UpdateTextFields(ScoreEntryList entryList)
    {
        StringBuilder nameBuilder = new StringBuilder();
        StringBuilder scoreBuilder = new StringBuilder();

        foreach (ScoreEntry entry in entryList.entries)
        {
            nameBuilder.AppendLine(entry.user);
            scoreBuilder.AppendLine(entry.score.ToString());
        }

        nameColumn.text = nameBuilder.ToString();
        scoreColumn.text = scoreBuilder.ToString();
    }
    
    
    // Get a list of scores from the server.
    private static IEnumerator GetGlobalScoresRequest(Callback callback)
    {
        using UnityWebRequest ping = UnityWebRequest.Get(RetrieveScoresUri);
        yield return ping.SendWebRequest();

        switch (ping.result)
        {
            case UnityWebRequest.Result.Success:
                var data = ping.downloadHandler.text;
                // Query succeeded. Convert from JSON string to objects, and then execute the callback.
                ScoreEntryList entryList = JsonUtility.FromJson<ScoreEntryList>("{\"entries\": " + data + "}");
                callback.Invoke(entryList);
                break;
            case UnityWebRequest.Result.InProgress:
                Debug.Log("Query is in progress.");
                break;
            case UnityWebRequest.Result.ConnectionError:
                Debug.Log("A connection error occurred.");
                Debug.Log(ping.responseCode);
                Debug.Log(ping.error);
                break;
            case UnityWebRequest.Result.ProtocolError:
                Debug.Log("A protocol error occurred.");
                Debug.Log(ping.responseCode);
                Debug.Log(ping.error);
                break;
            case UnityWebRequest.Result.DataProcessingError:
                Debug.Log("A data processing error occurred.");
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    
    // Post a user's score to the server.
    public void SubmitScore(string user, int score)
    {
        StartCoroutine(SubmitScoreCoroutine(user, score));
    }
    private static IEnumerator SubmitScoreCoroutine(string user, int score)
    {
        using UnityWebRequest ping = UnityWebRequest.Post(String.Format(SubmitScoreUri, user, score), "");
        yield return ping.SendWebRequest();

        switch (ping.result)
        {
            case UnityWebRequest.Result.Success:
                var data = ping.downloadHandler.text;
                // Query succeeded. Convert from JSON string to objects, and then execute the callback.
                Debug.Log(data);
                break;
            case UnityWebRequest.Result.InProgress:
                Debug.Log("Query is in progress.");
                break;
            case UnityWebRequest.Result.ConnectionError:
                Debug.Log("A connection error occurred.");
                Debug.Log(ping.responseCode);
                Debug.Log(ping.error);
                break;
            case UnityWebRequest.Result.ProtocolError:
                Debug.Log("A protocol error occurred.");
                Debug.Log(ping.responseCode);
                Debug.Log(ping.error);
                break;
            case UnityWebRequest.Result.DataProcessingError:
                Debug.Log("A data processing error occurred.");
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}

// Class objects for JSON deserialization.

[Serializable]
public class ScoreEntryList
{
    public ScoreEntry[] entries;
}

[Serializable]
public class ScoreEntry
{
    public string user;
    public int score;
}