using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Controllers;

public class Hourglass : MonoBehaviour
{
    [SerializeField] Image FillTop;
    [SerializeField] Image FillBottom;
    [SerializeField] TextMeshProUGUI TimeText;

    GameController gameController;

    // Start is called before the first frame update
    void Start()
    {
        gameController = FindObjectOfType<GameController>();

        if (gameController != null)
        {
            gameController.timer.OnPhaseChange.AddListener(reset);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (gameController != null)
        {
            FillTop.fillAmount = gameController.timer.GetTime() / 10;
            FillBottom.fillAmount = 1 - FillTop.fillAmount;

            int currentime = (int)gameController.timer.GetTime();
            TimeText.text = currentime.ToString();
        }
    }

    private void reset(EWorldPhase worldPhase)
    {
        transform.rotation = Quaternion.Euler(Vector3.zero);
        FillTop.fillAmount = 1f;
        FillBottom.fillAmount = 0f;
    }
}