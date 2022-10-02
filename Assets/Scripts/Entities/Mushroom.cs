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
    
        private float _timeOfLastJump;
        private Vector3 _positionOfLastJump;
        private bool _isJumping;
        
        
        //Only runs in "Dark Mode"
        protected override void EnemyMovement()
        {
            if (!_isJumping)
            {
                //Jump every jumpCooldown seconds
                if (Time.time > _timeOfLastJump + jumpCooldown)
                {
                    StartCoroutine(Jump());
                }
            }
        }
    
        //Melee Attack
        protected override void Attack()
        {
            //Player take damage
        }
        
        private IEnumerator Jump()
        {
            _isJumping = true;
        
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
        
            _isJumping = false;
            NotifyAnimator(Vector3.zero);
        
            yield return null;
        }
    }
}
