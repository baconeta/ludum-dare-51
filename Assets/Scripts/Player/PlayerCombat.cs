using System;
using System.Collections;
using Controllers;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerCombat : MonoBehaviour
    {
        /*
         * Unity References
         */
        [Header("Unity References")] [SerializeField] [Tooltip("The animator object for the player sprite.")]
        private Animator _animator;

        [SerializeField] [Tooltip("")] private PlayerFacing _playerFacing;

        [SerializeField] [Tooltip("The weapon object that the player uses to perform attacks with.")]
        private PlayerWeapon _weapon;

        public AudioClip _hitSound;
        private AudioSource _playerAudioSource;


        /*
         * Player health.
         */
        [Header("Player Health")] [SerializeField] [Tooltip("Starting player health.")]
        protected int healthInitial = 5;

        [SerializeField] [Tooltip("How much player health increases per upgrade level.")]
        protected int healthGrowthPerLevel = 1;

        [SerializeField] [Tooltip("How many times the player can upgrade health.")]
        protected int healthMaxLevel = 5;


        /*
         * Player attack damage.
         */
        [Header("Attack Damage")]
        [SerializeField]
        [Tooltip("How much damage the player deals to enemies per swing attack.")]
        protected float attackDamageInitial = 1.0F;

        [SerializeField] [Tooltip("By how much the player's attack damage increases per level.")]
        protected float attackDamageGrowthPerLevel = 0.2F;

        [SerializeField] [Tooltip("How many times the player can upgrade attack damage.")]
        protected int attackDamageMaxLevel = 5;


        /*
         * Player attack speed.
         */
        [Header("Attack Speed")]
        [SerializeField]
        [Tooltip("How many times per second that the player can attack with their weapon.")]
        protected float attackSpeedInitial = 2.0F;

        [SerializeField] [Tooltip("By how much the player's attack speed increases per level.")]
        protected float attackSpeedGrowthPerLevel = 0.667F;

        [SerializeField] [Tooltip("How many times the player can upgrade attack speed.")]
        protected int attackSpeedMaxLevel = 5;


        /*
         * Player attack range.
         */
        [Header("Attack Range")]
        [SerializeField]
        [Tooltip("How far in game units that the player can reach enemies with their weapon.")]
        protected float attackRangeInitial = 100.0F;

        [SerializeField] [Tooltip("By how much the player's attack range increases per level.")]
        protected float attackRangeGrowthPerLevel = 12.0F;

        [SerializeField] [Tooltip("How many times the player can upgrade attack range.")]
        protected int attackRangeMaxLevel = 5;

        /*
         * Player stat levels.
         */
        private int _healthLevel = 0;
        private int _attackDamageLevel = 0;
        private int _attackSpeedLevel = 0;
        private int _attackRangeLevel = 0;


        /*
         * Player stat values.
         * Use us for calculations!
         */
        public int healthMax;
        // Do not directly get/set this. Use HealthActual instead.
        private int _healthActualInternal;

        private int HealthActual
        {
            get => _healthActualInternal;
            set
            {
                _healthActualInternal = value;
                _gameUI.UpdateHealth();
            }
        }

        public float attackDamageActual;
        public float attackSpeedActual;
        public float attackRangeActual;

        /*
         * Other variables.
         */
        private bool _playing = true;

        // True if the player can't attack because they have recently attacked.
        protected bool attackOnCooldown = false;

        private bool isDead = false;
        private GameUI _gameUI;

        // Start is called before the first frame update
        private void Start()
        {
            _weapon = gameObject.GetComponentInChildren<PlayerWeapon>();
            if (!_playerFacing) _playerFacing = GetComponent<PlayerFacing>();
            _gameUI = GetComponent<Player>().gameUI;

            _playerAudioSource = GetComponent<AudioSource>();

            RecalculateStats();
        }

        // Update is called once per frame
        private void Update()
        {
            //For testing mostly
            if (_healthActualInternal <= 0)
            {
                if (!isDead) Die();
            }
        }

        private void FixedUpdate()
        {
            // Only update if the game is in play.
            if (!_playing) return;

            // Check if the player can be moved.
            if (!GameController.IsPlayerInputEnabled) return;

            // If the player is trying to attack, and the attack isn't on cooldown, initiate an attack.
            if (_playerFacing.IsPlayerAttacking() && !attackOnCooldown)
            {
                Attack();
            }
        }

        // Declare an attack.
        private void Attack()
        {
            attackOnCooldown = true;
            _weapon.DoAttack(GetAttackDirection());

            //Do visual


            StartCoroutine(ResetAttackCooldown());
        }

        private Vector2 GetAttackDirection()
        {
            return _playerFacing.GetFacingDirection() switch
            {
                PlayerFacing.FacingDirection.Down => Vector2.down,
                PlayerFacing.FacingDirection.Up => Vector2.up,
                PlayerFacing.FacingDirection.Left => Vector2.left,
                PlayerFacing.FacingDirection.Right => Vector2.right,
                _ => new Vector2()
            };
        }

        // This function resets the attack cooldown after the cooldown period ends.
        IEnumerator ResetAttackCooldown()
        {
            yield return new WaitForSeconds(1 / attackSpeedActual);
            attackOnCooldown = false;
        }

        private void RecalculateStats()
        {
            healthMax = healthInitial + (_healthLevel * healthGrowthPerLevel);
            HealthActual = healthMax;
            attackDamageActual = attackDamageInitial + (_attackDamageLevel * attackDamageGrowthPerLevel);
            attackSpeedActual = attackSpeedInitial + (_attackSpeedLevel * attackSpeedGrowthPerLevel);
            attackRangeActual = attackRangeInitial + (_attackRangeLevel * attackRangeGrowthPerLevel);
            _animator.ResetTrigger("Dead");
            _weapon.RecalculateStats();
        }

        public int GetPlayerHealth()
        {
            return HealthActual;
        }

        public void HealPlayer(int healing)
        {
            if (HealthActual < healthMax)
            {
                HealthActual += healing;
                // Prevent over-healing.
                if (HealthActual >= healthMax)
                    HealthActual = healthMax;
            }
        }

        public void DamagePlayer(int damage)
        {
            //Clamp to 0
            HealthActual -= damage;
            if (HealthActual < 0)
                HealthActual = 0;

            // TODO Give visual indication?

            if (HealthActual <= 0)
            {
                if (!isDead) Die();
            }

            _playerAudioSource.PlayOneShot(_hitSound);
        }

        private void Die()
        {
            isDead = true;
            // Make sure that health doesn't go negative.
            HealthActual = 0;
            // Stop the game.
            GetComponent<Player>().gameController.EndGame(false);
            _playing = false;
            // Trigger the death animation for the player.
            _animator.SetTrigger("Dead");
        }

        public float GetAttackDamage()
        {
            return attackDamageActual;
        }

        public float GetAttackSpeed()
        {
            return attackSpeedActual;
        }

        public float GetAttackRange()
        {
            return attackRangeActual;
        }

        public int GetHealthLevel() => _healthLevel;

        public bool CanIncreaseHealthLevel() => _healthLevel < healthMaxLevel;

        public void IncreaseHealthLevel()
        {
            if (!CanIncreaseHealthLevel())
                return;

            _healthLevel++;
            RecalculateStats();
        }

        public int GetAttackDamageLevel() => _attackDamageLevel;

        public bool CanIncreaseAttackDamageLevel() => _attackDamageLevel < attackDamageMaxLevel;

        public void IncreaseAttackDamageLevel()
        {
            if (!CanIncreaseAttackDamageLevel())
                return;

            _attackDamageLevel++;
            RecalculateStats();
        }

        public int GetAttackSpeedLevel() => _attackSpeedLevel;

        public bool CanIncreaseAttackSpeedLevel() => _attackSpeedLevel < attackSpeedMaxLevel;

        public void IncreaseAttackSpeedLevel()
        {
            if (!CanIncreaseAttackSpeedLevel())
                return;

            _attackSpeedLevel++;
            RecalculateStats();
        }

        public int GetAttackRangeLevel() => _attackRangeLevel;

        public bool CanIncreaseAttackRangeLevel() => _attackRangeLevel < attackRangeMaxLevel;

        public void IncreaseAttackRangeLevel()
        {
            if (!CanIncreaseAttackRangeLevel())
                return;

            _attackRangeLevel++;
            RecalculateStats();
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, attackRangeActual);
        }
    }
}