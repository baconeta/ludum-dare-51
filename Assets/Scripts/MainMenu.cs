using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour, IPointerClickHandler
{
    public string playGameScene;
    public GameObject playButton;
    public GameObject creditsButton;
    public GameObject howtoButton;
    public GameObject quitButton;

    public GameObject howtoImage;
    public bool howtoImageShowing = false;
    
    public void NewGame()
    {
        // SceneManager.LoadScene(playGameScene);
        if (playGameScene == "Start")
        {
            Destroy (GameObject.Find("MainCamera"));
            SceneManager.LoadScene("MainScene");
        }

        if (playGameScene == "BossBattle")
        {
            // enter boss battle scene ?
        }
    }

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
            if (howtoImageShowing == false)
            {
                howtoImageShowing = true;
            }
            else
            {
                howtoImageShowing = false;
            }
            
        }
    }
}
