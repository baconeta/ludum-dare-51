using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerMovement : MonoBehaviour
    { 
        
        
        /*
     * Player movement speed.
     */
        [Tooltip("How far in game units that the player can move per tick.")] [SerializeField]
        public float movementSpeedInitial = 2F;

        public float movementSpeedActual;

        public PlayerInput playerInput;
        public Rigidbody2D rb;
        public Animator animator;

        private Vector2 _movement;

        [Header("Collision Handling")]
        // A small offset (standoff) from colliders to ensure we don't try to get too close.
        // Moving too close can mean we get hits when moving tangential to a surface which results
        // in the controller not being able to move.
        public float mContactOffset = 0.05f;

        // A method to ensure we only iterate a certain number of times. Depending on the geometry,
        // this can be increased but for most purposes 2 or 3 will suffice.
        public int mMaxIterations = 2;

        // Controls what the controller considers hits.
        // Note: This allows us to control not only layers but potentially collision normal angles etc.
        public ContactFilter2D mMovementFilter;

        private List<RaycastHit2D> _mMovementHits = new(1);

        private void Start()
        {
            movementSpeedActual = movementSpeedInitial;

            //If missing components
            if (!rb) rb = GetComponent<Rigidbody2D>();
            if (!animator) animator = GetComponent<Animator>();
            if (!playerInput) playerInput = GetComponent<PlayerInput>();
        }

        void Update()
        {
            if (Controllers.GameController.IsPlayerInputEnabled)
            {
                Vector2 playerMovement = playerInput.actions["Move"].ReadValue<Vector2>();
                _movement.x = playerMovement.x;
                _movement.y = playerMovement.y;

                animator.SetFloat("Horizontal", _movement.x);
                animator.SetFloat("Vertical", _movement.y);
                animator.SetFloat("Velocity", _movement.sqrMagnitude);
            }
        }

        void FixedUpdate()
        {
            // Don't perform any work if no movement is required.
            if (_movement.sqrMagnitude <= Mathf.Epsilon)
                return;

            // Grab the input movement unit direction.
            var movementDirection = _movement.normalized;

            // Calculate how much distance we'd like to cover this update.
            var distanceRemaining = Mathf.Min(movementSpeedInitial * Time.fixedDeltaTime, _movement.magnitude);

            var startPosition = rb.position;

            var maxIterations = mMaxIterations;
            // Iterate up to a capped iteration limit or until we have no distance to move or we've clamped the direction of motion to zero.
            const float epsilon = 0.005f;
            while (
                maxIterations-- > 0 &&
                distanceRemaining > epsilon &&
                movementDirection.sqrMagnitude > epsilon
            )
            {
                var distance = distanceRemaining;

                // Perform a cast in the current movement direction using the colliders on the Rigidbody.
                // Note: A potentially better way of doing this is to do an arbitrary shape cast such as Physics2D.CapsuleCast/BoxCast etc.
                // At least when performing a specific shape query, we have no need to reposition the Rigidbody2D before each query.
                var hitCount = rb.Cast(movementDirection, mMovementFilter, _mMovementHits, distance);

                // Did we have any hits?
                if (hitCount > 0)
                {
                    // Yes, so for this controller we're only interested in the first results which is the first hit.
                    RaycastHit2D hit = _mMovementHits[0];

                    // We're only interested in movement if it's beyond the contact offset.
                    if (hit.distance > mContactOffset)
                    {
                        // Calculate the distance we'd like to move.
                        distance = hit.distance - mContactOffset;

                        // Reposition the Rigidbody2D to the hit point.
                        // NOTE: Again, this can be avoided by a different choice of query.
                        rb.position += movementDirection * distance;
                    }
                    else
                    {
                        // We had a hit but it resulted in us touching or being inside the contact offset.
                        distance = 0f;
                    }

                    // Clamp the movement direction.
                    // NOTE: This is effectively how we iterate and change direction for the queries.
                    movementDirection -= hit.normal * Vector2.Dot(movementDirection, hit.normal);
                }
                else
                {
                    // No hit so move by the whole distance.
                    rb.position += movementDirection * distance;
                }

                // Remove the distance we ended up moving from the remaining.
                distanceRemaining -= distance;
            }


            // Reset the input movement.
            _movement = Vector2.zero;

            // Reset the Rigidbody2D position due to changes during querying.
            // NOTE: We can avoid this setting of the Rigidbody2D position by a different choice of query.
            var targetPosition = rb.position;
            rb.position = startPosition;

            rb.MovePosition(targetPosition);
        }
    }
}