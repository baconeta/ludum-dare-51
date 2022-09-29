using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string playGameScene;

    public void NewGame()
    {
        SceneManager.LoadScene(playGameScene);
        if (playGameScene == "Start")
        {
            Destroy (GameObject.Find("MainCamera"));
        }
        
    }
}
