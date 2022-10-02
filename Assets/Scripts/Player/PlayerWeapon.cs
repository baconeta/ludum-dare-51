using System;
using System.Collections;
using System.Collections.Generic;
using Entities;
using UnityEngine;
using Random = UnityEngine.Random;

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
        
        [Header("Audio")]
        public AudioSource weaponAudioSource;
        public List<AudioClip> hitSounds;
        [Range(-1f, 0f)]
        public float hitSoundPitchShiftMin = 0f;
        [Range(0f, 1f)]
        public float hitSoundPitchShiftMax = 0f;

        
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

        public void DoAttack(Vector2 attackDirection)
        {

            weaponIsDamaging = true;

            if (CircleCollider is not null)
                CircleCollider.enabled = true;

            //Audio Trigger
            if (!weaponAudioSource.isPlaying)
            {
                //Get & set new Pitch-shift
                float newPitch = 1 + Random.Range(hitSoundPitchShiftMin, hitSoundPitchShiftMax
                );
                weaponAudioSource.pitch = newPitch;
            
                //Get random sound
                AudioClip hitSound = hitSounds[Random.Range(0, hitSounds.Count)];
                //Play sound
                weaponAudioSource.PlayOneShot(hitSound);
            }
            
            // TODO Trigger animation/visibility here.
            StartCoroutine(DisableMeleeDamage());
            
            SweepCollider(attackDirection);
        }

        // This function disables the weapon after the swing has finished.
        IEnumerator DisableMeleeDamage()
        {
            yield return new WaitForSeconds(_weaponIsDamagingDurationActual);
            weaponIsDamaging = false;

            if (CircleCollider is not null)
                CircleCollider.enabled = false;
        }

        private void SweepCollider(Vector2 attackDirection)
        {
            if (CircleCollider is null)
            {
                Debug.LogError("CircleCollider ref was null");
                return;
            }

            ContactFilter2D filter = new ContactFilter2D().NoFilter();
            List<Collider2D> results = new List<Collider2D>();
            CircleCollider.OverlapCollider(filter, results);

            //For each object the weapon collider overlaps
            foreach (Collider2D result in results)
            {
                if (!weaponIsDamaging) return;

                //Enemies only
                if (!result.CompareTag("Enemy")) return;
                
                //Direction player to Enemy
                Vector2 dir = (result.transform.position - transform.position).normalized;
                
                //Enemy is within a "Pie Slice" of the player.
                //0 is 90 degrees to the click angle.
                //1 is facing directly towards the enemy.
                //-1 is directly the opposite direction.
                if (Vector2.Dot(dir, attackDirection) > 0.5f)
                {
                    //Damage enemy
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