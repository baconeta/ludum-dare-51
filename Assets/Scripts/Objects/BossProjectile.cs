using Player;
using UnityEngine;

namespace Objects
{
    public class BossProjectile : Projectile
    {
        public override void ShootTarget(Vector3 newTargetPosition, GameObject newSource, float newSpeed,
            float newDamage)
        {
            // Make sure you set the projectile pos using instantiation before calling this function

            //Save variables
            _target = newTargetPosition;
            _speed = newSpeed;
            _source = newSource;
            _damage = newDamage;

            //Get direction to shoot
            Vector3 shootDirection = (newTargetPosition - transform.position).normalized;
            //Get speed/direction of bullet
            Vector3 fireVelocity = shootDirection * _speed;
            //Set velocity
            _rigidbody2D.velocity = fireVelocity;

            //Rotate so that it faces the correct direction // TODO make it spin or something??
            Quaternion angle = Quaternion.LookRotation(Vector3.forward, shootDirection);
            transform.rotation = angle;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            //It hits player! // TODO make this only occur if the corn has popped
            if (other.CompareTag("Player"))
            {
                //Get enemy damage
                if (_source != null)
                {
                    int enemyDamage = _source.GetComponent<Entities.Boss>().attackDamage; //Player takes damage
                    other.GetComponent<PlayerCombat>().DamagePlayer(enemyDamage);

                    DestroyProjectile();
                }
            }
        }
    }
}