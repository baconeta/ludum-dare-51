using UnityEngine;

namespace Collectables
{
    // This could be a parent for the different types of loot that we can pickup, or it could be the end-all class.
    public class Loot : Collectable
    {
        protected override void OnCollectablePickup()
        {
            //Give player value
            if (player == default)
            {
                player = FindObjectOfType<Player.Player>();
            }

            AudioSource.PlayClipAtPoint(pickupSound, transform.position, 0.15f);

            player.GetPlayerStats().AddCurrency(value);
            Destroy(gameObject);
        }
    }
}