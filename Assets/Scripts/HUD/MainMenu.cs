using UnityEngine;
using UnityEngine.SceneManagement;

namespace HUD
{
    public class MainMenu : MonoBehaviour
    {
        public string playGameScene;
        public Customization customization;
        public BossHotwire hotwire;

        void Start()
        {
            if (!hotwire) hotwire = FindObjectOfType<BossHotwire>();
        }

        public void NewGame()
        {
            Destroy(GameObject.Find("MainCamera"));
            customization.CacheCustomization();
            SceneManager.LoadScene("Scenes/MainScene");
        }

        public void NewGameStartingFromBoss()
        {
            NewGame();
            hotwire.EnableHotwire();
        }
    }
}
