using UnityEngine;

namespace Utils
{
    public class SpriteSwapper : MonoBehaviour
    {
        private SpriteRenderer _sr;
        [SerializeField] private Sprite lightSprite;
        [SerializeField] private Sprite darkSprite;

        private delegate void Swap(bool swap);

        private Swap _useLightMode;

        public SpriteSwapper()
        {
            _useLightMode = SetLightMode;
        }

        private void SetLightMode(bool useLight)
        {
            _sr.sprite = useLight ? lightSprite : darkSprite;
        }

        public void Start()
        {
            _sr = GetComponent<SpriteRenderer>();
            SetLightMode(true);
        }
    }
}