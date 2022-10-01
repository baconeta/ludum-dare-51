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

            player.GetPlayerStats().AddCurrency((int) value);
            Destroy(gameObject);
        }
    }
}