using UnityEngine;

namespace Objects
{
    public class Projectile : MonoBehaviour
    {
        public float lifespan = 5;
        public Sprite projectileSprite;

        //Given by initializer
        private Vector3 _target;
        private GameObject _source;
        private float _speed;
        private float _damage;
        private float _timeOfSpawn;


        private Rigidbody2D _rigidbody2D;
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

        public void ShootTarget(Vector3 newTargetPosition, GameObject newSource, float newSpeed, float newDamage)
        {
            //Save variables
            _target = newTargetPosition;
            _source = newSource;
            _speed = newSpeed;
            _damage = newDamage;

            //Move to spawn
            transform.position = newSource.transform.position;

            //Get direction to shoot
            Vector3 shootDirection = (newTargetPosition - _source.transform.position).normalized;
            //Get speed/direction of bullet
            Vector3 fireVelocity = shootDirection * _speed;
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
}