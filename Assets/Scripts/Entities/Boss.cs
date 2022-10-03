using Objects;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace Entities
{
    public class Boss : Enemy
    {
        private GameObject _healthBar;
        private Slider _healthBarSlider;
        public Projectile projectile;
        public float projectileSpeed = 3;
        public Vector3 cornSpitOffset;
        public float timeAttackTakes;
        public AudioClip[] spitSounds;
        public AudioSource audioSource;

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
            audioSource.PlayOneShot(spitSounds.ChooseRandom());
        }

        protected override void UpdateAnimator()
        {
            // We don't need to update the animator every frame like the other enemies
        }

        public override void TakeDamage(float damage)
        {
            base.TakeDamage(damage);
            // Update the boss's health bar value to reflect the new current health.
            if (!_healthBar) _healthBar = FindObjectOfType<BossHealthBarHoist>().bossHealthBar;
            if (!_healthBarSlider) _healthBarSlider = _healthBar.GetComponent<Slider>();
            _healthBarSlider.value = _currentHealth / maxHealth;
        }

        public override void Die(bool isDespawning = false)
        {
            // Boss died so we win the game
            Debug.Log("we killed the boss");
            // Hide the boss's health bar.
            if (!_healthBar) _healthBar = FindObjectOfType<BossHealthBarHoist>().bossHealthBar;
            _healthBar.SetActive(false);
        }
    }
}