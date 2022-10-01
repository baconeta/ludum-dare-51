using Controllers;
using UnityEngine;

namespace Entities
{
    public abstract class Enemy : MonoBehaviour
    {
        //Enemy collectable
        public GameObject lootOnDeath;

        [Header("Enemy Stats")] public string enemyName;

        public float moveSpeed = 1;
        public float maxHealth = 1;
        private float _currentHealth;
        public float aggravationRange = 10;
        private bool _isAggravated = false;
        private bool _isDarkMode = true;

        [Header("Attack Stats")] public float attackRadius = 1;
        public float attackSpeed = 1;
        public float attackDamage = 1;
        private float _timeOfLastAttack;

        //Components
        protected Player.Player player;
        protected Rigidbody2D _rigidbody2D;
        private GameController _gameController;

        protected abstract void EnemyMovement();
        protected abstract void Attack();

        // Start is called before the first frame update
        protected virtual void Start()
        {
            if (!_rigidbody2D)
            {
                _rigidbody2D = GetComponent<Rigidbody2D>();
            }

            if (!player)
            {
                player = FindObjectOfType<Player.Player>();
            }

            if (!_gameController)
            {
                _gameController = FindObjectOfType<GameController>();
            }

            SetupEventSubscriptions();

            //Set health to max
            _currentHealth = maxHealth;
        }

        // Update is called once per frame
        protected virtual void Update()
        {
        }


        protected virtual void FixedUpdate()
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

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
            if (!_isDarkMode)
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

        public void TakeDamage(float damage)
        {
            _currentHealth -= damage;
            //Die check
            if (_currentHealth <= 0)
            {
                Die();
            }
        }

        //Despawning variable for if no rewards should be given
        public void Die(bool isDespawning = false)
        {
            if (!isDespawning)
            {
                //Rewards
                SpawnLoot();
            }

            RemoveEventSubscriptions();
            Destroy(gameObject);
        }

        private void SpawnLoot()
        {
            //Spawn loot
            GameObject newLoot = Instantiate(lootOnDeath);
            //Move loot to die position
            newLoot.transform.position = transform.position;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackRadius);

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
            _isDarkMode = newPhase == EWorldPhase.DARK;
        }
    }
}