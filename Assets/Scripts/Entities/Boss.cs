namespace Entities
{
    public class Boss : Enemy
    {
        // Start is called before the first frame update
        protected override void EnemyMovement()
        {
            // We do not move
        }

        protected override void Attack()
        {
            // if we attack then we should cry
        }

        protected override void UpdateAnimator()
        {
            // do nothing bro
        }

        public override void Die(bool isDespawning)
        {
            // Boss died so we win the game
        }
    }
}