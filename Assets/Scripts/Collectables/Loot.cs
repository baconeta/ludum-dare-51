using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Collectables
{
    // This could be a parent for the different types of loot that we can pickup, or it could be the end-all class.
    public class Loot : Collectable
    {
        
        protected override void OnCollectablePickup()
        {
            //Give player value
            _player.AddCurrency((int)value);
            Destroy(gameObject);
        }
    }
}
