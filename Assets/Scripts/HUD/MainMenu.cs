using UnityEngine;
using UnityEngine.SceneManagement;

namespace HUD
{
    public class MainMenu : MonoBehaviour
    {
        public string playGameScene;
        public Customization customization;

        public void NewGame()
        {
            Destroy(GameObject.Find("MainCamera"));
            customization.CacheCustomization();
            SceneManager.LoadScene("Scenes/MainScene");
        }
    }
}