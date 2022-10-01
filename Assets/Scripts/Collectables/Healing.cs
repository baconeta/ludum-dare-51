using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Collectables
{
    // Item that heals the player upon pickup.
    public class Healing : Collectable
    {
        protected override void OnCollectablePickup()
        {
            Debug.Log(value + " health healed!");
            _player.GetPlayerCombat();//.Heal(healValue);
            Destroy(gameObject);
        }
    }
}