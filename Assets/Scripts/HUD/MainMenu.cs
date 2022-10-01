using UnityEngine;
using UnityEngine.SceneManagement;

namespace HUD
{
    public class MainMenu : MonoBehaviour
    {
        public string playGameScene;

        public void NewGame()
        {
            Destroy(GameObject.Find("MainCamera"));
            SceneManager.LoadScene("Scenes/MainScene");
        }
    }
}