using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float lifespan = 5;
    public Sprite projectileSprite;
    
    //Given by initializer
    private Vector3 target;
    private GameObject source;
    private float speed;
    private float damage;
    private float timeOfSpawn;
    
    
    private Rigidbody2D _rigidbody2D;
    private SpriteRenderer _sr;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _sr = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        timeOfSpawn = Time.time;

    }

    private void Update()
    {
        //Bullet timer - Destroy after duration
        if (Time.time > timeOfSpawn + lifespan)
        {
            DestroyProjectile();
        }
    }

    public void ShootTarget(Vector3 newTargetPosition, GameObject newSource, float newSpeed, float newDamage)
    {
        //Save variables
        target = newTargetPosition;
        source = newSource;
        speed = newSpeed;
        damage = newDamage;

        //Move to spawn
        transform.position = newSource.transform.position;
        //Get direction to shoot
        Vector3 shootDirection = (newTargetPosition - source.transform.position).normalized;
        //Get speed/direction of bullet
        Vector3 fireVelocity = shootDirection * speed;
        //Set velocity
        _rigidbody2D.velocity = fireVelocity;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            //Player takes damage
            DestroyProjectile();
        }
    }

    private void DestroyProjectile()
    {
        Destroy(gameObject);
    }
}
