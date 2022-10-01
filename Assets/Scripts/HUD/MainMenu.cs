using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace HUD
{
    public class MainMenu : MonoBehaviour, IPointerClickHandler
    {
        public string playGameScene;
        public GameObject playButton;
        public GameObject creditsButton;
        public GameObject howtoButton;
        public GameObject quitButton;

        public void NewGame()
        {
            // SceneManager.LoadScene(playGameScene);
            if (playGameScene == "Start")
            {
                Destroy(GameObject.Find("MainCamera"));
                SceneManager.LoadScene("MainScene");
            }
        }

        // This probably should just be set to functions instead of pointer clicks that are called on button presses
        public void OnPointerClick(PointerEventData menuEvent)
        {
            // if play button clicked destroy, load scene 
            if (menuEvent.pointerClick.gameObject == playButton)
            {
                NewGame();
            }

            // if quit button clicked quit application
            if (menuEvent.pointerClick.gameObject == quitButton)
            {
                Application.Quit();
            }

            if (menuEvent.pointerClick.gameObject == creditsButton)
            {
                // run credits
            }

            if (menuEvent.pointerClick.gameObject == howtoButton)
            {
                // pop up art image of how to play
            }
        }
    }
}