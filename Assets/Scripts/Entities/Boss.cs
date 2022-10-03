using Objects;
using UnityEngine;

namespace Entities
{
    public class Boss : Enemy
    {
        public Projectile projectile;
        public float projectileSpeed = 3;
        public Vector3 cornSpitOffset;
        public float timeAttackTakes;

        // Start is called before the first frame update
        protected override void EnemyMovement()
        {
            // We do not move
        }

        protected override void Attack()
        {
            // Attacking should start the attacking animation, spawn a kernel and fire it at the players location

            // TODO add curve to projectile motion

            Invoke(nameof(SpawnProjectile), timeAttackTakes);
        }

        private void SpawnProjectile()
        {
            GameObject o = gameObject;
            Vector3 instantiationLocation = o.transform.position;
            instantiationLocation += cornSpitOffset;
            Projectile newProjectile = Instantiate(projectile, instantiationLocation, o.transform.rotation);
            newProjectile.ShootTarget(player.transform.position, gameObject, projectileSpeed, attackDamage);
        }

        protected override void UpdateAnimator()
        {
            // We don't need to update the animator every frame like the other enemies
        }

        public override void Die(bool isDespawning = false)
        {
            // Boss died so we win the game
            Debug.Log("we killed the boss");
        }
    }
}