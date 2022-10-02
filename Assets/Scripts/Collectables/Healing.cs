using System.Collections;
using Controllers;
using UnityEngine;

namespace Collectables
{
    // Item that heals the player upon pickup.
    public class Healing : Collectable
    {
        private GameController _gameController;
        
        //Ambient Sound for when heart is dark
        public AudioClip darkHeartSound;

        protected override void OnCollectablePickup()
        {
            //Play pick up sound
            AudioSource.PlayClipAtPoint(pickupSound, transform.position);
            
            FindObjectOfType<Player.Player>().GetPlayerCombat().HealPlayer(value);
            
            Destroy(gameObject);
        }

        private void Awake()
        {
            _gameController = FindObjectOfType<GameController>();
            if (_gameController != null)
            {
                _gameController.timer.OnPhaseChange.AddListener(SetPhaseMode);
                _gameController.timer.OnTimerStart.AddListener(SetPhaseMode);
            }

            SetPhaseMode(_gameController.timer.GetWorldPhase());
        }

        private void SetPhaseMode(EWorldPhase worldPhase)
        {
            isInteractable = worldPhase switch
            {
                EWorldPhase.LIGHT => true,
                EWorldPhase.DARK => false,
                _ => isInteractable
            };
        }


        protected override IEnumerator InteractableDelay()
        {
            isInteractable = false;
            
            yield return null;
        }
    }
}