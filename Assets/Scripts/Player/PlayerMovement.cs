using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerMovement : MonoBehaviour
    { 
        
        
        /*
     * Player movement speed.
     */
        [Tooltip("How far in game units that the player can move per tick.")]
        [SerializeField]
        public float movementSpeedInitial = 2F;

        public float movementSpeedActual;

        public PlayerInput playerInput;
        public Rigidbody2D rb;
        public Animator animator;

        private Vector2 _movement;

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
                Debug.Log(playerMovement);
                _movement.x = playerMovement.x;
                _movement.y = playerMovement.y;

                animator.SetFloat("Horizontal", _movement.x);
                animator.SetFloat("Vertical", _movement.y);
                animator.SetFloat("Velocity", _movement.magnitude);
            }

        }

        void FixedUpdate()
        {
            Vector2 distanceToMove = _movement * (movementSpeedActual * Time.fixedDeltaTime);
            rb.MovePosition(rb.position + distanceToMove);
        }
    }
}
