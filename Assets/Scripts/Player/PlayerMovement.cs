using UnityEngine;


public class PlayerMovement : MonoBehaviour
{
    /*
     * Player movement speed.
     */
    [Tooltip("How far in game units that the player can move per tick.")] [SerializeField]
    public float movementSpeedInitial = 2F;
    public float movementSpeedActual;

    public Rigidbody2D rb;
    public Animator animator;

    private Vector2 _movement;

    void Start()
    {
        movementSpeedActual = movementSpeedInitial;
    }

    void Update()
    {
        _movement.x = Input.GetAxisRaw("Horizontal");
        _movement.y = Input.GetAxisRaw("Vertical");

        animator.SetFloat("Horizontal", _movement.x);
        animator.SetFloat("Vertical", _movement.y);
        animator.SetFloat("Velocity", _movement.sqrMagnitude);
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + ((movementSpeedActual * Time.fixedDeltaTime) * _movement));
    }
}