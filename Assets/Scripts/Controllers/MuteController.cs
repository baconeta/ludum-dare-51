using UnityEngine;
using UnityEngine.UI;

namespace Controllers
{
    public class MuteController : MonoBehaviour
    {
        public Sprite unmutedImage;
        public Sprite mutedImage;
        public Toggle isMuted;
        public bool globalMute;

        [SerializeField] private Slider volumeSlider;

        private SpriteRenderer _spriteRenderer;

        // Start is called before the first frame update
        private void Start()
        {
            isMuted = GetComponent<Toggle>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            isMuted.isOn = false;
        }

        public void MuteButton()
        {
            if (isMuted.isOn)
            {
                globalMute = true;
                _spriteRenderer.sprite = mutedImage;
                AudioListener.volume = 0;
            }
            else
            {
                globalMute = false;
                _spriteRenderer.sprite = unmutedImage;
                AudioListener.volume = volumeSlider.value;
            }
        }
    }
}