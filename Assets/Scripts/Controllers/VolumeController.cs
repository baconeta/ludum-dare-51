using UnityEngine;

namespace Controllers
{
    public class VolumeController : MonoBehaviour
    {
        [SerializeField] private MuteController muteController;

        // Start is called before the first frame update
        void Start()
        {
        }

        public void OnValueChanged(float sliderValue)
        {
            if (muteController.globalMute)
            {
                return;
            }

            AudioListener.volume = sliderValue;
        }
    }
}