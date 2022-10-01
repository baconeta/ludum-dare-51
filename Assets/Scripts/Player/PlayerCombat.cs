using System.Collections;
using Player;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [Header("Unity References")] public Animator animator;

    private PlayerWeapon _weapon;
    public AudioClip attackSound;

    /*
     * Player health.
     */
    [Header("Health")] [Tooltip("Starting player health.")] [SerializeField]
    protected int healthInitial = 5;

    [Tooltip("How much player health increases per upgrade level.")] [SerializeField]
    protected int healthGrowthPerLevel = 1;

    [Tooltip("How many times the player can upgrade health.")] [SerializeField]
    protected int healthMaxLevel = 5;

    protected int healthLevel = 0;

    // Use me for calculations.
    protected int healthMax;
    protected int healthActual;

    /*
     * Player attack damage.
     */
    [Header("Attack Damage")]
    [Tooltip("How much damage the player deals to enemies per swing attack.")]
    [SerializeField]
    protected float attackDamageInitial = 1.0F;

    [Tooltip("By how much the player's attack damage increases per level.")] [SerializeField]
    protected float attackDamageGrowthPerLevel = 0.2F;

    [Tooltip("How many times the player can upgrade attack damage.")] [SerializeField]
    protected int attackDamageMaxLevel = 5;

    protected int attackDamageLevel = 0;

    // Use me for calculations.
    protected float attackDamageActual;

    /*
     * Player attack speed.
     */
    [Header("Attack Speed")]
    [Tooltip("How many times per second that the player can attack with their weapon.")]
    [SerializeField]
    protected float attackSpeedInitial = 2.0F;

    [Tooltip("By how much the player's attack speed increases per level.")] [SerializeField]
    protected float attackSpeedGrowthPerLevel = 0.667F;

    [Tooltip("How many times the player can upgrade attack speed.")] [SerializeField]
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
    [Tooltip("How far in game units that the player can reach enemies with their weapon.")]
    [SerializeField]
    protected float attackRangeInitial = 100.0F;

    [Tooltip("By how much the player's attack range increases per level.")] [SerializeField]
    protected float attackRangeGrowthPerLevel = 12.0F;

    [Tooltip("How many times the player can upgrade attack range.")] [SerializeField]
    protected int attackRangeMaxLevel = 5;

    protected int attackRangeLevel = 0;

    // Use me for calculations.
    protected float attackRangeActual;

    private bool _playing = true;

    // True if the player is trying to attack.
    protected bool attacking = false;

    // True if the player can't attack because they have recently attacked.
    protected bool attackOnCooldown = false;

    protected enum FacingDirection
    {
        Up,
        Down,
        Left,
        Right,
    }

    protected FacingDirection facingDirection;

    // Start is called before the first frame update
    private void Start()
    {
        if (!animator) GetComponent<Animator>();
        _weapon = GetComponent<PlayerWeapon>();

        RecalculateStats();
    }

    // Update is called once per frame
    private void Update()
    {
        // If user is left-clicking.
        // TODO Replace this check for analog 2.
        attacking = Input.GetButton("Fire1") || Input.GetMouseButton(1);

        // Update the animator.
        animator.SetBool("Attacking", attacking);

        // Temporary function to damage the player when SPACE is pressed.
        if (Input.GetButton("Jump"))
        {
            DamagePlayer(1);
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
    }

    // This function resets the attack cooldown after the cooldown period ends.
    IEnumerator ResetAttackCooldown()
    {
        yield return new WaitForSeconds(1 / attackSpeedActual);
        attackOnCooldown = false;
    }

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