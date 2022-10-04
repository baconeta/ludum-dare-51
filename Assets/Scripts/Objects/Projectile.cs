using System;
using Player;
using UnityEngine;

namespace Objects
{
    public class Projectile : MonoBehaviour
    {
        public float lifespan = 5;
        public Sprite projectileSprite;

        //Given by initializer
        protected Vector3 _target;
        protected GameObject _source;
        protected float _speed;
        protected float _damage;
        private float _timeOfSpawn;


        protected Rigidbody2D _rigidbody2D;
        private SpriteRenderer _sr;

        private void Awake()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _sr = GetComponent<SpriteRenderer>();
        }

        private void Start()
        {
            _timeOfSpawn = Time.time;
        }

        private void Update()
        {
            //Bullet timer - Destroy after duration
            if (Time.time > _timeOfSpawn + lifespan)
            {
                DestroyProjectile();
            }
        }

        public virtual void ShootTarget(Vector3 newTargetPosition, GameObject newSource, float newSpeed,
            float newDamage, Vector3 offset = default)
        {
            //Save variables
            _target = newTargetPosition;
            _source = newSource;
            _speed = newSpeed;
            _damage = newDamage;


            //Move to spawn
            transform.position = newSource.transform.position + offset;

            //Get direction to shoot
            Vector3 shootDirection = (newTargetPosition - _source.transform.position).normalized;
            //Get speed/direction of bullet
            Vector3 fireVelocity = shootDirection * _speed;
            //Set velocity
            _rigidbody2D.velocity = fireVelocity;

            //Rotate so that it faces the correct direction
            Quaternion angle = Quaternion.LookRotation(Vector3.forward, shootDirection);
            transform.rotation = angle;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            //It hits player!

            if (other.CompareTag("Player"))
            {
                //Get enemy damage
                if (_source != null)
                {
                    int enemyDamage = _source.GetComponent<Entities.Enemy>().attackDamage; //Player takes damage
                    other.GetComponent<PlayerCombat>().DamagePlayer(enemyDamage);

                    DestroyProjectile();
                }
            }
        }

        protected void DestroyProjectile()
        {
            Destroy(gameObject);
        }
    }
}