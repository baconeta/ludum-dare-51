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

        [SerializeField] [Tooltip("The analog control for player input.")]
        private PlayerInput _playerInput;

        [SerializeField] [Tooltip("The weapon object that the player uses to perform attacks with.")]
        private PlayerWeapon _weapon;

        [SerializeField] [Tooltip("The sound to be played when the player attacks.")]
        private AudioClip _attackSound;


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
         * Upgrade Costs.
         */
        [Header("Upgrade Costs")]
        [SerializeField]
        [Tooltip("How much currency it costs to upgrade from level 0 to level 1")]
        public int firstUpgradeCost = 3;

        [SerializeField] [Tooltip("How much currency it costs to upgrade from level 1 to level 2")]
        public int secondUpgradeCost = 7;

        [SerializeField] [Tooltip("How much currency it costs to upgrade from level 2 to level 3")]
        public int thirdUpgradeCost = 12;

        [SerializeField] [Tooltip("How much currency it costs to upgrade from level 3 to level 4")]
        public int fourthUpgradeCost = 18;

        [SerializeField] [Tooltip("How much currency it costs to upgrade from level 4 to level 5")]
        public int fifthUpgradeCost = 25;


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
        public int healthActual;

        private int HealthActual
        {
            get => healthActual;
            set
            {
                healthActual = value;
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

        // True if the player is trying to attack.
        protected bool attacking = false;

        // True if the player can't attack because they have recently attacked.
        protected bool attackOnCooldown = false;

        //Direction of the attack
        protected Vector2 playerAttackDirection = Vector2.zero;

        private bool isDead = false;
        private GameUI _gameUI;

        public enum FacingDirection
        {
            Up,
            Down,
            Left,
            Right,
        }

        private FacingDirection facingDirection = FacingDirection.Up;

        public FacingDirection GetFacingDirection()
        {
            return facingDirection;
        }

        // Start is called before the first frame update
        private void Start()
        {
            if (!_animator) GetComponent<Animator>();
            _weapon = gameObject.GetComponentInChildren<PlayerWeapon>();
            if (!_playerInput) _playerInput = GetComponent<PlayerInput>();
            _gameUI = GetComponent<Player>().gameUI;

            RecalculateStats();
        }

        // Update is called once per frame
        private void Update()
        {
            //For testing mostly
            if (healthActual <= 0)
            {
                if (!isDead) Die();
            }

            // Check if the player can be moved.
            if (Controllers.GameController.IsPlayerInputEnabled)
            {
                playerAttackDirection = Vector2.zero;

                if (Controllers.InputController.isMobile) //Mobile Controls
                {
                    //Get Input for playerAttack joystick
                    playerAttackDirection = _playerInput.actions["Attack"].ReadValue<Vector2>();

                    //Only if stick is in use
                    //Player is attacking
                    if (playerAttackDirection != Vector2.zero)
                    {
                        //Face direction and Attack!
                        _animator.SetFloat("Horizontal", playerAttackDirection.x);
                        _animator.SetFloat("Vertical", playerAttackDirection.y);

                        //Horizontal
                        if (playerAttackDirection.x < 0) facingDirection = FacingDirection.Left;
                        else facingDirection = FacingDirection.Right;

                        //Vertical
                        if (playerAttackDirection.y < 0) facingDirection = FacingDirection.Down;
                        else facingDirection = FacingDirection.Up;

                        attacking = true;
                        // facingDirection = CalculateFacingDirection(playerAttackDirection);
                    }
                }
                // Keyboard controls
                else
                {
                    // Attack is pressed
                    if (_playerInput.actions["Attack"].IsPressed())
                    {
                        // Attack in direction of the mouse
                        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                        playerAttackDirection = (mousePosition - (Vector2) transform.position).normalized;

                        //Attack!
                        attacking = true;
                    }
                }

                // Update the animator.
                _animator.SetBool("Attacking", attacking);
                if (attacking)
                {
                    _animator.SetFloat("Horizontal", playerAttackDirection.x);
                    _animator.SetFloat("Vertical", playerAttackDirection.y);
                }

            }
        }

        private void FixedUpdate()
        {
            // Only update if the game is in play.
            if (!_playing) return;

            // If the player is trying to attack, and the attack isn't on cooldown, initiate an attack.
            if (attacking && !attackOnCooldown)
            {
                Attack();
            }
        }

        // Declare an attack.
        private void Attack()
        {
            attacking = false;
            attackOnCooldown = true;
            _weapon.DoAttack(playerAttackDirection);
            StartCoroutine(ResetAttackCooldown());
        }

        // This function resets the attack cooldown after the cooldown period ends.
        IEnumerator ResetAttackCooldown()
        {
            yield return new WaitForSeconds(1 / attackSpeedActual);
            attackOnCooldown = false;
        }

        private static FacingDirection CalculateFacingDirection(Vector2 direction)
        {
            // If the absolute value of X is larger than Y.
            if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
            {
                // If X is positive, the facing direction is Right. Otherwise it is Left.
                return (direction.x > 0F ? FacingDirection.Right : FacingDirection.Left);
            }
            // else, the absolute value of Y is larger.
            else
            {
                // If Y is positive, the facing direction is Up. Otherwise it is Down.
                return (direction.y > 0F ? FacingDirection.Up : FacingDirection.Down);
            }
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

        public int GetHealthLevel()
        {
            return _healthLevel;
        }

        public void IncreaseHealthLevel()
        {
            if (_healthLevel < healthMaxLevel)
                _healthLevel++;
            RecalculateStats();
        }

        public int GetAttackDamageLevel()
        {
            return _attackDamageLevel;
        }

        public void IncreaseAttackDamageLevel()
        {
            if (_attackDamageLevel < attackDamageMaxLevel)
                _attackDamageLevel++;
            RecalculateStats();
        }

        public int GetAttackSpeedLevel()
        {
            return _attackSpeedLevel;
        }

        public void IncreaseAttackSpeedLevel()
        {
            if (_attackSpeedLevel < attackSpeedMaxLevel)
                _attackSpeedLevel++;
            RecalculateStats();
        }

        public int GetAttackRangeLevel()
        {
            return _attackRangeLevel;
        }

        public void IncreaseAttackRange()
        {
            if (_attackRangeLevel < attackRangeMaxLevel)
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