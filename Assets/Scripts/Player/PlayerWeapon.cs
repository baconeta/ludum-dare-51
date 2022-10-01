using System.Collections;
using Entities;
using UnityEngine;

namespace Player
{
    public class PlayerWeapon : MonoBehaviour
    {
        public PlayerCombat pc;

        [Tooltip("Per swing, how long should the weapon be \"hot\" for, as a percentage.")] [SerializeField]
        private float weaponIsDamagingDurationPercentage = 70.0F;

        private float _weaponIsDamagingDurationActual;

        // True if collisions with the weapon will damage enemies.
        protected bool weaponIsDamaging = false;

        void Start()
        {
            RecalculateStats();
        }


        public void RecalculateStats()
        {
            // Attack Period (how long a full attack rotation takes) * "Hot" percentage (how long the weapon is hot for).
            _weaponIsDamagingDurationActual = (1 / pc.GetAttackSpeed()) * (weaponIsDamagingDurationPercentage / 100F);
        }

        public void DoAttack()
        {
            weaponIsDamaging = true;
            // TODO Trigger animation/visibility here.
            StartCoroutine(DisableMeleeDamage());
        }

        // This function disables the weapon after the swing has finished.
        IEnumerator DisableMeleeDamage()
        {
            yield return new WaitForSeconds(_weaponIsDamagingDurationActual);
            weaponIsDamaging = false;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (weaponIsDamaging && other.CompareTag("Enemy"))
            {
                other.GetComponent<Enemy>().TakeDamage(pc.GetAttackDamage());
                // Optional, for if we get injure animations for enemies.
                //other.GetComponent<Animator>().SetTrigger("Hit");
            }
        }
    }
}