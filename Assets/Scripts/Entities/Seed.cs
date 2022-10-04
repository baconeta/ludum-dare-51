using Objects;
using UnityEngine;

namespace Entities
{
    public class Seed : Enemy
    {
        public Projectile projectile;
        public float projectileSpeed;
        [SerializeField] private Vector3 projectileOffset;

        //Only runs in "Dark Mode"
        protected override void EnemyMovement()
        {
            //Move to linearly

            //Get direction of player
            Vector3 directionToPlayer = (player.transform.position - transform.position).normalized;

            //Move to player
            var moveValue = moveSpeed * Time.deltaTime * directionToPlayer;
            transform.position += moveValue;
            NotifyAnimator(moveValue);
        }

        //Range Attack
        protected override void Attack()
        {
            //Spawn Projectile (Damage is handled by projectile)
            Projectile newProjectile = Instantiate(projectile);
            newProjectile.ShootTarget(player.transform.position, gameObject, projectileSpeed, attackDamage,
                projectileOffset);
        }
    }
}