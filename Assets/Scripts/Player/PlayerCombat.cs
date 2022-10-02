using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerCombat : MonoBehaviour
    {
        [Header("Unity References")]
        public Animator animator;

        public PlayerInput playerInput;
        private PlayerWeapon _weapon;
        public AudioClip attackSound;

        /*
         * Player health.
         */
        [Header("Health")]
        [SerializeField][Tooltip("Starting player health.")]
        protected int healthInitial = 5;

        [SerializeField][Tooltip("How much player health increases per upgrade level.")]
        protected int healthGrowthPerLevel = 1;

        [SerializeField][Tooltip("How many times the player can upgrade health.")]
        protected int healthMaxLevel = 5;

        protected int healthLevel = 0;

        // Use me for calculations.
        protected int healthMax;
        protected int healthActual;

        /*
         * Player attack damage.
         */
        [Header("Attack Damage")] 
        [SerializeField][Tooltip("How much damage the player deals to enemies per swing attack.")]
        protected float attackDamageInitial = 1.0F;

        [SerializeField][Tooltip("By how much the player's attack damage increases per level.")]
        protected float attackDamageGrowthPerLevel = 0.2F;

        [SerializeField][Tooltip("How many times the player can upgrade attack damage.")]
        protected int attackDamageMaxLevel = 5;

        protected int attackDamageLevel = 0;

        // Use me for calculations.
        protected float attackDamageActual;

        /*
         * Player attack speed.
         */
        [Header("Attack Speed")]
        [SerializeField][Tooltip("How many times per second that the player can attack with their weapon.")]
        protected float attackSpeedInitial = 2.0F;

        [SerializeField][Tooltip("By how much the player's attack speed increases per level.")]
        protected float attackSpeedGrowthPerLevel = 0.667F;

        [SerializeField][Tooltip("How many times the player can upgrade attack speed.")]
        protected int attackSpeedMaxLevel = 5;

        protected int attackSpeedLevel = 0;

        // Use me for calculations.
        protected float attackSpeedActual;

        // Used to calculate how long it has been since the last attack.
        protected float timeOfLastAttack = 0.0F;

        /*
         * Player attack range.
         */
        [Header("Attack Range")]
        [SerializeField][Tooltip("How far in game units that the player can reach enemies with their weapon.")]
        protected float attackRangeInitial = 100.0F;

        [SerializeField][Tooltip("By how much the player's attack range increases per level.")]
        protected float attackRangeGrowthPerLevel = 12.0F;

        [SerializeField][Tooltip("How many times the player can upgrade attack range.")]
        protected int attackRangeMaxLevel = 5;

        protected int attackRangeLevel = 0;

        // Use me for calculations.
        protected float attackRangeActual;

        private bool _playing = true;

        // True if the player is trying to attack.
        protected bool attacking = false;

        // True if the player can't attack because they have recently attacked.
        protected bool attackOnCooldown = false;

        public enum FacingDirection
        {
            Up,
            Down,
            Left,
            Right,
        }

        private FacingDirection facingDirection;

        public FacingDirection GetFacingDirection()
        {
            return facingDirection;
        }

        // Start is called before the first frame update
        private void Start()
        {
            if (!animator) GetComponent<Animator>();
            _weapon = gameObject.GetComponentInChildren<PlayerWeapon>();
            if (!playerInput) playerInput = GetComponent<PlayerInput>();

            RecalculateStats();
        }

        // Update is called once per frame
        private void Update()
        {
            if (Controllers.GameController.IsPlayerInputEnabled)
            {
                // If user is left-clicking.
                // TODO Replace this check for analog 2.
                if (Controllers.InputController.isMobile) //Mobile Controls
                {
                    Vector2 playerAttack = playerInput.actions["Attack"].ReadValue<Vector2>();

                    //Only if stick is in use
                    if (playerAttack != Vector2.zero)
                    {
                        //Face direction and Attack!
                        //playerAttack <-- Use this Vector2 for player-to-enemy direction
                        attacking = true;
                    }
                }
                else //keyboard controls
                {
                    //Attack is pressed
                    if (playerInput.actions["Attack"].IsPressed())
                    {
                        //Attack in direction of the mouse
                        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                        Vector2 attackDirection = mousePosition - (Vector2)transform.position;
                        //Final attack direction to face player to mouse
                        attackDirection = transform.position + (Vector3)attackDirection;

                        //Attack!
                        attacking = true;
                    }
                }

                // Update the animator.
                animator.SetBool("Attacking", attacking);
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
            attackOnCooldown = true;
            _weapon.DoAttack();
            StartCoroutine(ResetAttackCooldown());
            attacking = false;
        }

        // This function resets the attack cooldown after the cooldown period ends.
        IEnumerator ResetAttackCooldown()
        {
            yield return new WaitForSeconds(1 / attackSpeedActual);
            attackOnCooldown = false;
        }

        /*
         * Recalculate the -Actual variables of this classes based on (primarily) levels. Also resets health between levels.
         */
        private void RecalculateStats()
        {
            healthMax = healthInitial + (healthLevel * healthGrowthPerLevel);
            healthActual = healthMax;
            attackDamageActual = attackDamageInitial + (attackDamageLevel * attackDamageGrowthPerLevel);
            attackSpeedActual = attackSpeedInitial + (attackSpeedLevel * attackSpeedGrowthPerLevel);
            attackRangeActual = attackRangeInitial + (attackRangeLevel * attackRangeGrowthPerLevel);
            animator.ResetTrigger("Dead");
            _weapon.RecalculateStats();
        }

        public int GetPlayerHealth()
        {
            return healthActual;
        }

        public void HealPlayer(int healing)
        {
            if (healthActual < healthMax)
            {
                healthActual += healing;
                // Prevent over-healing.
                if (healthActual >= healthMax)
                    healthActual = healthMax;
            }
        }

        public void DamagePlayer(int damage)
        {
            healthActual -= damage;
            // TODO Give visual indication?
            // TODO Update HUD?
            if (healthActual <= 0)
            {
                // Make sure that health doesn't go negative.
                healthActual = 0;
                // Stop the game. TODO Hook into the controllers later.
                _playing = false;
                // Trigger the death animation for the player.
                animator.SetTrigger("Dead");
            }
        }

        /*
         * Getters and setters for instance variables.
         */
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
            return healthLevel;
        }

        public void IncreaseHealthLevel()
        {
            if (healthLevel < healthMaxLevel)
                healthLevel++;
            RecalculateStats();
        }

        public int GetAttackDamageLevel()
        {
            return attackDamageLevel;
        }

        public void IncreaseAttackDamageLevel()
        {
            if (attackDamageLevel < attackDamageMaxLevel)
                attackDamageLevel++;
            RecalculateStats();
        }

        public int GetAttackSpeedLevel()
        {
            return attackSpeedLevel;
        }

        public void IncreaseAttackSpeedLevel()
        {
            if (attackSpeedLevel < attackSpeedMaxLevel)
                attackSpeedLevel++;
            RecalculateStats();
        }

        public int GetAttackRangeLevel()
        {
            return attackRangeLevel;
        }

        public void IncreaseAttackRange()
        {
            if (attackRangeLevel < attackRangeMaxLevel)
                attackRangeLevel++;
            RecalculateStats();
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, attackRangeActual);
        }
    }
}