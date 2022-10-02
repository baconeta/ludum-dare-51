using System.Collections;
using UnityEngine;

namespace Collectables
{
    // Parent class.
    public abstract class Collectable : MonoBehaviour
    {
        public float value;
        private bool isInteractable;
        [Tooltip("Timer before the item is interactable (by the player)")]
        public float pickupTimer = 0;
        
        protected Player.Player player;

        protected abstract void OnCollectablePickup();

        // Start is called before the first frame update
        void Start()
        {
            player = FindObjectOfType<Player.Player>();

            if (pickupTimer > 0)
            {
                StartCoroutine(InteractableDelay());
            }
        }

        IEnumerator InteractableDelay()
        {
            //Disable interactable
            isInteractable = false;
            GetComponent<CircleCollider2D>().enabled = isInteractable;
            
            yield return new WaitForSeconds(pickupTimer);
            
            //Re-enable Interactable
            isInteractable = true;
            GetComponent<CircleCollider2D>().enabled = isInteractable;
        }
        
        private void OnTriggerEnter2D(Collider2D col)
        {
            if (isInteractable)
            {
                if (col.gameObject.CompareTag("Player"))
                {
                    //TODO Needs to check if within player pickup radius
                    OnCollectablePickup();
                }
            }
        }
    }
}