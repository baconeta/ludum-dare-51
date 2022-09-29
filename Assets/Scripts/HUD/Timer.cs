using UnityEngine;
using UnityEngine.UI;

namespace HUD
{
    public class Timer : MonoBehaviour
    {
        public Text timerValue;
        // private StatsController _statsController;

        private void OnEnable()
        {
            Invoke(nameof(GrabStatsController), 0.5f);
        }

        private void GrabStatsController()
        {
            // _statsController = FindObjectOfType<StatsController>();
        }

        private void Update()
        {
            // if (_statsController != default)
            // {
            //     TimerValue.text = _statsController.FormatTime(_statsController.time);
            // }
        }
    }
}