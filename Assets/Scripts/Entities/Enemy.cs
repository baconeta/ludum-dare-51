using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    //Enemy collectable
    public GameObject droppable;

    [Header("Enemy Stats")] 
    public string enemyName;

    public float moveSpeed = 1;
    public float maxHealth = 1;
    
    private float _currentHealth;
    private bool _isDarkMode = true;
    
    [Header("Attack Stats")]
    public float attackRadius = 1;
    public float attackSpeed = 1;
    public float attackDamage = 1;
    private float timeOfLastAttack;

    //Components
    protected Player _player;
    protected Rigidbody2D _rigidbody2D;

    protected abstract void EnemyMovement();
    protected abstract void Attack();
    
    // Start is called before the first frame update
    protected virtual void Start()
    {
        if (!_rigidbody2D)
        {
            
            _rigidbody2D = GetComponent<Rigidbody2D>();
            
        }

        if (!_player)
        {
            _player = FindObjectOfType<Player>();
        }
        
        //Set health to max
        _currentHealth = maxHealth;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        //Die check
        if (_currentHealth <= 0)
        {
            Die();
        }
        
    }
    
    
    protected virtual void FixedUpdate()
    {
        //If in dark mode, move.
        if (_isDarkMode)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, _player.transform.position);
            
            //If in range
            if (distanceToPlayer <= attackRadius)
            {
                if (Time.time > timeOfLastAttack + attackSpeed)
                {
                    timeOfLastAttack = Time.time;
                    Attack();
                }
            }
            else //(Out of range)
            {
                EnemyMovement();
            }
            
        }
        
        // Else in light mode!

        
    }

    private void Die()
    {
        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }
}
