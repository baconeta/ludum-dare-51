using System.Collections;
using UnityEngine;

namespace Entities
{
    public class Mushroom : Enemy
    {
        [Header("Jumping Movement")]
        public AnimationCurve jumpCurve;
        public float jumpDistance;
        //The duration it takes to jump from Start to End
        public float jumpSpeed;
        //How often the enemy jumps
        public float jumpCooldown;
        public float jumpCooldownRandomness;
        private float _currentJumpCooldown;

        private float _timeOfLastJump;
        private Vector3 _positionOfLastJump;
        private bool _isJumping;

        protected override void Start()
        {
            base.Start();

            _currentJumpCooldown = jumpCooldown + Random.Range(-jumpCooldownRandomness, jumpCooldownRandomness);
        }


        //Only runs in "Dark Mode"
        protected override void EnemyMovement()
        {
            if (!_isJumping)
            {
                //Jump every jumpCooldown seconds
                if (Time.time > _timeOfLastJump + _currentJumpCooldown)
                {
                    StartCoroutine(Jump());
                }
            }
        }

        //Melee Attack
        protected override void Attack()
        {
            //Player take damage
            player.GetPlayerCombat().DamagePlayer(attackDamage);
        }

        private IEnumerator Jump()
        {
            _isJumping = true;
            animator.SetFloat("JumpSpeed", 1/jumpSpeed);
            animator.SetBool("IsJumping", true);

            //Set current position (Jump start)
            _positionOfLastJump = transform.position;

            //Get direction of player
            Vector3 directionToPlayer = (player.transform.position - transform.position).normalized;

            //Get distance of player
            float distanceToPlayer = Vector3.Distance(_positionOfLastJump, player.transform.position);

            //Move to player in jumps
            Vector3 jumpTarget;

            //If jumpTarget goes past player, reduce distance
            if (distanceToPlayer < jumpDistance)
            {
                //Reduce jump distance to reach player
                jumpTarget = transform.position + directionToPlayer * distanceToPlayer;
            }
            else // Jump max distance
            {
                jumpTarget = transform.position + directionToPlayer * jumpDistance ;
            }

            //Set time to zero
            float timeElapsed = 0;
            Vector3 moveValue;

            //While traveling
            while (timeElapsed < jumpSpeed)
            {
                //Calculate Position through Lerp(a, b, t)
                float t = timeElapsed / jumpSpeed;

                //Ease out using sin
                t = Mathf.Sin(t * Mathf.PI * 0.5f);

                t = jumpCurve.Evaluate(t);

                //Move through Lerp
                moveValue = Vector3.Lerp(_positionOfLastJump, jumpTarget, t);

                timeElapsed += Time.deltaTime;

                //Update to new position
                _rigidbody2D.MovePosition(moveValue);
                NotifyAnimator(moveValue);

                yield return null;
            }

            //Arrived at position
            moveValue = jumpTarget;

            //Move enemy to new position
            _rigidbody2D.MovePosition(moveValue);
            NotifyAnimator(moveValue);


            _timeOfLastJump = Time.time;
            _currentJumpCooldown = jumpCooldown + Random.Range(-jumpCooldownRandomness, jumpCooldownRandomness);

            _isJumping = false;
            animator.SetBool("IsJumping", false);

            yield return null;
        }
    }
}
