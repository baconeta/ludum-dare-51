using UnityEngine;

namespace Collectables
{
    // Item that heals the player upon pickup.
    public class Healing : Collectable
    {
        protected override void OnCollectablePickup()
        {
            Debug.Log(value + " health healed!");
            FindObjectOfType<Player.Player>().GetPlayerCombat().HealPlayer(value);
            Destroy(gameObject);
        }
    }
}