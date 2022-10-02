using System;
using System.Collections;
using System.Collections.Generic;
using Entities;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(CircleCollider2D))]
    public class PlayerWeapon : MonoBehaviour
    {
        private PlayerCombat _playerCombat;
        private CircleCollider2D CircleCollider;

        [Tooltip("Per swing, how long should the weapon be \"hot\" for, as a percentage.")] [SerializeField]
        private float weaponIsDamagingDurationPercentage = 70.0F;

        private float _weaponIsDamagingDurationActual;

        [SerializeField] float hitAngle = 60f;


        // True if collisions with the weapon will damage enemies.
        protected bool weaponIsDamaging = false;

        private void Awake()
        {
            _playerCombat = GetComponentInParent<PlayerCombat>();
            CircleCollider = GetComponent<CircleCollider2D>();

            if (CircleCollider != null)
            {
                CircleCollider.enabled = false;
            }

            RecalculateStats();
        }

        public void RecalculateStats()
        {
            // Attack Period (how long a full attack rotation takes) * "Hot" percentage (how long the weapon is hot for).
            _weaponIsDamagingDurationActual =
                (1 / _playerCombat.GetAttackSpeed()) * (weaponIsDamagingDurationPercentage / 100F);
            CircleCollider.radius = _playerCombat.GetAttackRange();

        }

        public void DoAttack()
        {
            weaponIsDamaging = true;

            if (CircleCollider is not null)
                CircleCollider.enabled = true;

            // TODO Trigger animation/visibility here.
            StartCoroutine(DisableMeleeDamage());

            SweepCollider();
        }

        // This function disables the weapon after the swing has finished.
        IEnumerator DisableMeleeDamage()
        {
            yield return new WaitForSeconds(_weaponIsDamagingDurationActual);
            weaponIsDamaging = false;

            if (CircleCollider is not null)
                CircleCollider.enabled = false;
        }

        private void SweepCollider()
        {
            if (CircleCollider is null)
            {
                Debug.LogError("CircleCollider ref was null");
                return;
            }

            ContactFilter2D filter = new ContactFilter2D().NoFilter();
            List<Collider2D> results = new List<Collider2D>();
            CircleCollider.OverlapCollider(filter, results);

            //For each object the weapon overlaps
            foreach (Collider2D result in results)
            {
                if (!weaponIsDamaging) return;

                //Enemies only
                if (!result.CompareTag("Enemy")) return;
                
                //Direction player to Enemy
                Vector2 dir = result.gameObject.transform.position - gameObject.transform.position;
                
                //Angle to Enemy
                float angle = Vector2.Angle(dir, GetCurrentDirection());
                
                //Distance of objects
                float distance = Vector2.Distance(gameObject.transform.position, result.gameObject.transform.position);
                
                if (angle <= hitAngle
                    || angle <= 90)
                {
                    result.GetComponent<Enemy>().TakeDamage(_playerCombat.GetAttackDamage());
                }
                
            }
        }

        private Vector2 GetCurrentDirection()
        {
            if (_playerCombat is null)
            {
                Debug.LogError("PlayerCombat ref was null");
            }

            Vector2 dir;

            switch (_playerCombat.GetFacingDirection())
            {
                case PlayerCombat.FacingDirection.Up:
                    dir = transform.up;
                    break;
                case PlayerCombat.FacingDirection.Down:
                    dir = -transform.up;
                    break;
                case PlayerCombat.FacingDirection.Left:
                    dir = -transform.right;
                    break;
                case PlayerCombat.FacingDirection.Right:
                    dir = transform.right;
                    break;
                default:
                    dir = new Vector2();
                    break;
            }

            return dir;
        }
    }
}