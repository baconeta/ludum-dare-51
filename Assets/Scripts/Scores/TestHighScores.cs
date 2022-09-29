using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

namespace Scores
{
    public class TestHighScores : MonoBehaviour
    {
        private const string Uri = "ADD_URI_HERE";

        public void ConsoleHighScores()
        {
            StartCoroutine(PingHighScores());
        }

        // For testing only - may be useful later
        public static Dictionary<string, object> Parse(byte[] json)
        {
            var jsonStr = Encoding.UTF8.GetString(json);
            return JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonStr);
        }

        private static IEnumerator PingHighScores()
        {
            using UnityWebRequest ping = UnityWebRequest.Get(Uri);
            yield return ping.SendWebRequest();

            switch (ping.result)
            {
                case UnityWebRequest.Result.Success:
                    Debug.Log("It works lol");
                    var data = ping.downloadHandler.text;
                    Debug.Log(data);
                    break;
                case UnityWebRequest.Result.InProgress:
                    Debug.Log("In progress");
                    break;
                case UnityWebRequest.Result.ConnectionError:
                    Debug.Log(ping.error);
                    Debug.Log(ping.responseCode);
                    Debug.Log("CX error");
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.Log(ping.error);
                    Debug.Log(ping.responseCode);
                    Debug.Log("Protocol error");
                    break;
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.Log("Data processing error");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}