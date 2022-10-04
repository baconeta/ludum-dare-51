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
        private CircleCollider2D _circleCollider;
        [SerializeField] private ContactFilter2D filter;

        [Tooltip("Per swing, how long should the weapon be \"hot\" for, as a percentage.")] [SerializeField]
        private float weaponIsDamagingDurationPercentage = 70.0F;

        private float _weaponIsDamagingDurationActual;
        private Player _player;

        public GameObject weaponGraphic;

        [Header("Audio")] public AudioSource weaponAudioSource;
        public List<AudioClip> hitSounds;
        [Range(-1f, 0f)] public float hitSoundPitchShiftMin = 0f;
        [Range(0f, 1f)] public float hitSoundPitchShiftMax = 0f;


        // True if collisions with the weapon will damage enemies.
        protected bool weaponIsDamaging = false;

        private void Awake()
        {
            _playerCombat = FindObjectOfType<PlayerCombat>();
            _circleCollider = GetComponent<CircleCollider2D>();
            _player = FindObjectOfType<Player>();

            if (_circleCollider != null)
            {
                _circleCollider.enabled = false;
            }

            RecalculateStats();
        }

        private void Update()
        {
            transform.position = _player.transform.position;
        }

        public void RecalculateStats()
        {
            // Attack Period (how long a full attack rotation takes) * "Hot" percentage (how long the weapon is hot for).
            _weaponIsDamagingDurationActual =
                (1 / _playerCombat.GetAttackSpeed()) * (weaponIsDamagingDurationPercentage / 100F);

            //Set range
            _circleCollider.radius = _playerCombat.GetAttackRange();
        }

        public void DoAttack(Vector2 attackDirection)
        {
            weaponIsDamaging = true;

            if (_circleCollider is not null)
                _circleCollider.enabled = true;

            //Audio Trigger
            if (!weaponAudioSource.isPlaying) //Is audio current playing?
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
            StartCoroutine(ShowWeaponGraphic(attackDirection));

            StartCoroutine(DisableMeleeDamage());

            SweepCollider(attackDirection);
        }

        private IEnumerator ShowWeaponGraphic(Vector2 direction)
        {
            //Show graphic
            weaponGraphic.SetActive(true);

            //Rotate so that it faces the correct direction
            Quaternion angle = Quaternion.LookRotation(Vector3.forward, -direction);
            weaponGraphic.transform.rotation = angle;

            // Make graphic match attack radius
            weaponGraphic.transform.localScale = Vector3.one * _circleCollider.radius / 5;

            //Wait until attack is finished
            yield return new WaitForSeconds(_weaponIsDamagingDurationActual);

            //Hide graphic
            weaponGraphic.SetActive(false);
            yield return null;
        }


        // This function disables the weapon after the swing has finished.
        IEnumerator DisableMeleeDamage()
        {
            yield return new WaitForSeconds(_weaponIsDamagingDurationActual);
            weaponIsDamaging = false;

            if (_circleCollider is not null)
                _circleCollider.enabled = false;
        }

        private void SweepCollider(Vector2 attackDirection)
        {
            if (_circleCollider is null)
            {
                Debug.LogError("CircleCollider ref was null");
                return;
            }


            List<Collider2D> results = new List<Collider2D>();
            _circleCollider.OverlapCollider(filter, results);

            //For each object the weapon collider overlaps
            foreach (Collider2D result in results) //TODO might need to change to a regular for loop
            {
                if (!weaponIsDamaging) return;

                Vector2 rawDirection = result.transform.position - transform.position;

                //Direction player to Enemy
                Vector2 dirNormalized = rawDirection.normalized;

                //Enemy is within a "Pie Slice" of the player.
                //0 is 90 degrees to the click angle.
                //1 is facing directly towards the enemy.
                //-1 is directly the opposite direction.
                var dotProd = Vector2.Dot(dirNormalized, attackDirection);

                if (dotProd > 0.7f || (dotProd > 0 && rawDirection.magnitude < 1f))
                {
                    //Damage enemy
                    result.GetComponent<Enemy>().TakeDamage(_playerCombat.GetAttackDamage());
                }
            }
        }
    }
}