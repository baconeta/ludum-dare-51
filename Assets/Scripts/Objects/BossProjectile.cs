using System;
using System.Collections;
using Player;
using UnityEngine;
using UnityEngine.PlayerLoop;
using Random = UnityEngine.Random;

namespace Objects
{
    public class BossProjectile : Projectile
    {
        [SerializeField] private Animator animator;
        public float distanceToStartPopping = 2f;
        private bool keepMoving;
        private bool popping;
        private static readonly int Pop = Animator.StringToHash("Pop");
        [SerializeField] private AudioClip[] popSounds;
        [SerializeField] private AudioSource audioSource;

        public override void ShootTarget(Vector3 newTargetPosition, GameObject newSource, float newSpeed,
            float newDamage)
        {
            // Make sure you set the projectile pos using instantiation before calling this function

            //Save variables
            _target = newTargetPosition;
            _speed = newSpeed;
            _source = newSource;
            _damage = newDamage;
            keepMoving = true;
            popping = false;
            GetComponent<CircleCollider2D>().enabled = false;
        }

        private void FixedUpdate()
        {
            // TODO could make this way prettier with a curve oh well
            if (!keepMoving) return;

            Vector3 shootVector = _target - transform.position;
            if (!popping && shootVector.magnitude <= distanceToStartPopping)
            {
                animator.SetTrigger(Pop);
                popping = true;
                GetComponent<CircleCollider2D>().enabled = true;
                foreach (AudioClip popSound in popSounds)
                {
                    StartCoroutine(PlayPop(popSound));
                }
            }

            if (shootVector.magnitude <= 0.5f)
            {
                keepMoving = false;
            }
            else
            {
                _rigidbody2D.MovePosition(
                    Vector3.MoveTowards(transform.position, _target, _speed * Time.fixedDeltaTime));
            }
        }

        private IEnumerator PlayPop(AudioClip popSound)
        {
            yield return new WaitForSeconds(Random.Range(0f, 0.3f));
            audioSource.PlayOneShot(popSound);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            //It hits player! // TODO make this only occur if the corn has popped
            if (other.CompareTag("Player"))
            {
                //Get enemy damage
                if (_source != null)
                {
                    int enemyDamage = _source.GetComponent<Entities.Boss>().attackDamage; //Player takes damage
                    other.GetComponent<PlayerCombat>().DamagePlayer(enemyDamage);

                    DestroyProjectile();
                }
            }
        }
    }
}