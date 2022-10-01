using UnityEngine;

namespace Collectables
{
    // Parent class.
    public abstract class Collectable : MonoBehaviour
    {
        public float value;

        public Sprite sprite;
        protected SpriteRenderer sr;
        protected Rigidbody2D rb;
        protected Player.Player player;

        protected abstract void OnCollectablePickup();

        // Start is called before the first frame update
        void Start()
        {
            player = FindObjectOfType<Player.Player>();
            sr = GetComponent<SpriteRenderer>();
            rb = GetComponent<Rigidbody2D>();
        }

        private void OnCollisionEnter2D(Collision2D col)
        {
            if (col.gameObject.CompareTag("Player"))
            {
                OnCollectablePickup();
            }
        }
    }
}