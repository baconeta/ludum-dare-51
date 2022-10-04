using System;
using Controllers;
using Player;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Entities
{
    public abstract class Enemy : MonoBehaviour
    {
        //Enemy collectable
        public GameObject lootOnDeath;

        public GameObject HealthLoot;
        [SerializeField] [Range(0, 100)] private int HealthLootChance = 50;

        [Header("Enemy Stats")] public string enemyName;

        public float moveSpeed = 1;
        private Vector3 _lastMovement;
        [SerializeField] private float baseScoreMultiplier = 1f;

        public float maxHealth = 1;
        protected float _currentHealth;

        public float aggravationRange = 10;
        private bool _isAggravated = false;

        private EWorldPhase WorldPhase;

        [Header("Attack Stats")] public float attackRadius = 1;
        public float attackSpeed = 1;
        public int attackDamage = 1;
        private float _timeOfLastAttack;

        //Components
        protected Player.Player player;
        protected Rigidbody2D _rigidbody2D;
        private GameController _gameController;
        protected Animator animator;
        public AudioClip hitSound;
        [Range(0, 1f)] public float volume;
        [SerializeField] private Vector3 centreOffset;

        public EnemyController EcRef { get; set; }

        protected abstract void EnemyMovement();
        protected abstract void Attack();

        // Start is called before the first frame update
        protected virtual void Start()
        {
            if (!_rigidbody2D)
                _rigidbody2D = GetComponent<Rigidbody2D>();

            if (!animator)
            {
                animator = GetComponent<Animator>();
                if (!animator)
                {
                    animator = GetComponentInParent<Animator>();
                }
            }

            if (!player)
                player = FindObjectOfType<Player.Player>();

            if (!_gameController)
                _gameController = FindObjectOfType<GameController>();

            //Set health to max
            _currentHealth = maxHealth;

            SetWorldPhase(_gameController.timer.GetWorldPhase());
            SetupEventSubscriptions();
        }

        // Update is called once per frame
        protected virtual void Update()
        {
            UpdateAnimator();
        }

        protected virtual void FixedUpdate()
        {
            if (!_gameController.GameRunning) return;

            NotifyAnimator(Vector3.zero);
            float distanceToPlayer = Vector3.Distance(transform.position + centreOffset, player.transform.position);

            if (!_isAggravated)
            {
                if (distanceToPlayer < aggravationRange) Aggravate();
                return;
            }
            else
            {
                if (distanceToPlayer > aggravationRange) DeAggravate();
            }

            //If its in light mode
            if (WorldPhase == EWorldPhase.LIGHT)
            {
                return;
            }
            //Else its in dark mode


            //If in range
            if (distanceToPlayer <= attackRadius)
            {
                //If ready to attack
                if (Time.time > _timeOfLastAttack + attackSpeed)
                {
                    _timeOfLastAttack = Time.time;
                    Attack();
                    animator.SetTrigger("Attacked");
                }
            }
            else //(Out of range)
            {
                EnemyMovement();
            }
        }

        private void DeAggravate()
        {
            _isAggravated = false;
        }

        private void Aggravate()
        {
            _isAggravated = true;
        }

        public virtual void TakeDamage(float damage)
        {
            if (WorldPhase == EWorldPhase.DARK)
            {
                _currentHealth -= damage;

                //Play audio
                AudioSource.PlayClipAtPoint(hitSound, transform.position, volume);

                //Die check
                if (_currentHealth <= 0)
                {
                    Die();
                }
            }
        }

        //Despawning variable for if no rewards should be given
        public virtual void Die(bool isDespawning = false)
        {
            if (!isDespawning)
            {
                //Rewards
                SpawnLoot();
            }

            if (EcRef == default)
            {
                EcRef = FindObjectOfType<EnemyController>();
            }

            FindObjectOfType<PlayerStats>().AddScore((int) (baseScoreMultiplier * maxHealth));

            EcRef.livingEnemies.Remove(this);
            RemoveEventSubscriptions();
            Destroy(gameObject);
        }

        private void SpawnLoot()
        {
            if (lootOnDeath != null)
            {
                //Spawn loot
                GameObject newLoot = Instantiate(lootOnDeath);
                float variance = 0.4f;
                Vector3 randomOffset = new Vector3(Random.Range(-variance, variance), Random.Range(-variance, variance),
                    0f);
                //Move loot to die position
                newLoot.transform.position = transform.position + centreOffset + randomOffset;
            }
            else
            {
                Debug.LogWarning("lootOnDeath prefab ref was null, cannot instantiate");
            }

            if (HealthLoot != null)
            {
                int random = Random.Range(0, 100);
                if (random <= HealthLootChance)
                {
                    GameObject healthLoot = Instantiate(HealthLoot);
                    healthLoot.transform.position = transform.position + centreOffset;
                }
            }
            else
            {
                Debug.LogWarning("HealthLoot prefab ref was null, cannot instantiate");
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position + centreOffset, attackRadius);

            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, aggravationRange);
        }

        private void SetupEventSubscriptions()
        {
            _gameController.timer.OnPhaseChange.AddListener(SetWorldPhase);
        }

        private void RemoveEventSubscriptions()
        {
            _gameController.timer.OnPhaseChange.RemoveListener(SetWorldPhase);
        }

        private void SetWorldPhase(EWorldPhase newPhase)
        {
            WorldPhase = newPhase;
            animator.SetBool("IsDark", WorldPhase == EWorldPhase.DARK);
        }

        protected void NotifyAnimator(Vector3 movement)
        {
            _lastMovement = movement;
        }

        protected virtual void UpdateAnimator()
        {
            animator.SetFloat("Horizontal", _lastMovement.x);
            animator.SetFloat("Vertical", _lastMovement.y);
            animator.SetFloat("Velocity", _lastMovement.magnitude);
        }
    }
}