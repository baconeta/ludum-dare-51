using Controllers;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerFacing : MonoBehaviour
    {
        [SerializeField] [Tooltip("")] private Camera gameCamera;
        [SerializeField] [Tooltip("")] private GameController gameController;

        [SerializeField] [Tooltip("The animator object for the player sprite.")]
        private Animator animator;

        [SerializeField] [Tooltip("The analog control for player input.")]
        private PlayerInput playerInput;

        public enum FacingDirection
        {
            Up,
            Down,
            Left,
            Right,
        }

        private FacingDirection _facingDirection = FacingDirection.Up;
        private static readonly int Direction = Animator.StringToHash(("FacingDirection"));
        private static readonly int Attacking = Animator.StringToHash("Attacking");
        private static readonly int Walking = Animator.StringToHash("Walking");
        private static readonly int Dead = Animator.StringToHash("Dead");

        public FacingDirection GetFacingDirection()
        {
            return _facingDirection;
        }

        public void SetFacingDirection(FacingDirection dir)
        {
            _facingDirection = dir;
        }


        // Start is called before the first frame update
        void Start()
        {
            // Get components.
            if (!gameCamera) gameCamera = FindObjectOfType<Camera>();
            if (!animator) animator = GetComponent<Animator>();
            if (!playerInput) playerInput = GetComponent<PlayerInput>();
            if (!gameController) gameController = FindObjectOfType<GameController>();
        }

        // Update is called once per frame
        private void Update()
        {
            if (!gameController.GameRunning)
            {
                animator.SetTrigger(Dead);
            }

            // Check if input is enabled.
            if (!Controllers.GameController.IsPlayerInputEnabled) return;

            if (IsPlayerAttacking())
            {
                // Get the attack direction for the player, then use it later to calculate our facing direction.
                Vector2 attackDirection;
                if (Controllers.InputController.isMobile)
                {
                    attackDirection = playerInput.actions["Attack"].ReadValue<Vector2>();
                }
                else
                {
                    // Vector between mouse position and player position in-world.
                    attackDirection =
                        gameCamera.ScreenToWorldPoint(playerInput.actions["MousePos"].ReadValue<Vector2>()) -
                        transform.position;
                }

                // Check which axis has greater magnitude, then check if that axis is positive or negative.
                if (Mathf.Abs(attackDirection.x) > Mathf.Abs(attackDirection.y))
                {
                    _facingDirection = (attackDirection.x > 0) ? FacingDirection.Right : FacingDirection.Left;
                }
                else
                {
                    _facingDirection = (attackDirection.y > 0) ? FacingDirection.Up : FacingDirection.Down;
                }

                // Update the animator.
                animator.SetBool(Attacking, true);
                animator.SetBool(Walking, false);
            }
            else if (IsPlayerWalking())
            {
                // Get the attack direction for the player, then use it later to calculate our facing direction.
                Vector2 motionDirection;
                motionDirection = playerInput.actions["Move"].ReadValue<Vector2>();

                // Check which axis has greater magnitude, then check if that axis is positive or negative.
                if (Mathf.Abs(motionDirection.x) > Mathf.Abs(motionDirection.y))
                {
                    _facingDirection = (motionDirection.x > 0) ? FacingDirection.Right : FacingDirection.Left;
                }
                else
                {
                    _facingDirection = (motionDirection.y > 0) ? FacingDirection.Up : FacingDirection.Down;
                }

                // Update the animator.
                animator.SetBool(Attacking, false);
                animator.SetBool(Walking, true);
            }
            else // Idling.
            {
                // Don't change facing direction here.

                // Update the animator.
                animator.SetBool(Attacking, false);
                animator.SetBool(Walking, false);
            }


            switch (_facingDirection)
            {
                case FacingDirection.Up:
                    animator.SetFloat(Direction, 0);
                    break;
                case FacingDirection.Down:
                    animator.SetFloat(Direction, 1);
                    break;
                case FacingDirection.Left:
                    animator.SetFloat(Direction, 2);
                    break;
                case FacingDirection.Right:
                default:
                    animator.SetFloat(Direction, 3);
                    break;
            }
        }

        public bool IsPlayerAttacking()
        {
            // Touch.
            if (Controllers.InputController.isMobile)
            {
                // Get Input for player attack analog.
                Vector2 rightAnalogDirection = playerInput.actions["Attack"].ReadValue<Vector2>();
                // Check if analog stick is in use.
                return (rightAnalogDirection != Vector2.zero);
            }
            // Keyboard + Mouse.
            else
            {
                return (playerInput.actions["Attack"].IsPressed());
            }
        }

        public bool IsPlayerWalking()
        {
            // Get Input for player motion analog.
            Vector2 motionDirection = playerInput.actions["Move"].ReadValue<Vector2>();
            // Check if analog stick is in use.
            return (motionDirection != Vector2.zero);
        }
    }
}