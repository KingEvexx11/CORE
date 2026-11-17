using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D), typeof(PlayerInput))]
public class PlayerScript : MonoBehaviour
{


    [Header("Player Settings")]

    [Tooltip("The max speed a player can move by itself")]
    [SerializeField] private float _topSpeed;

    [Tooltip("How fast the player accelerates to top Speed")]
    [SerializeField] private float _accel;
    [Tooltip("Dictates how high the player can jump (0 means that the player cannot jump)")]
    [SerializeField] private float _jumpHeight;

    [Header("Box raycast settings")]
    [SerializeField] private Vector2 _boxOffset;
    [SerializeField] private Vector2 _boxSize;
    [SerializeField] private LayerMask _boxLayer;

    private Rigidbody2D _rb;
    private SpriteRenderer sprt;

    private float _movement;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        sprt = GetComponent<SpriteRenderer>();
    }



    // Update is called once per frame
    private void Update()
    {
        _rb.AddForceX(_movement * _accel * Time.deltaTime);
        _rb.linearVelocityX = Mathf.Clamp(_rb.linearVelocityX, -_topSpeed, _topSpeed);
    }


    public void Move(InputAction.CallbackContext ctx)
    {
        _movement = ctx.ReadValue<Vector2>().x;

        if (_movement < 0)
            sprt.flipX = true;
        else if (_movement > 0)
            sprt.flipX = false;
    }

    public void Jump(InputAction.CallbackContext ctx)
    {

        if (ctx.canceled || !IsGrounded())
            return;

        _rb.linearVelocityY = _jumpHeight;



    }

    private bool IsGrounded()
    {
        RaycastHit2D hit = Physics2D.BoxCast(transform.position + (Vector3)_boxOffset, _boxSize, 0, Vector2.zero, 0, _boxLayer);

        return hit;
    }


    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position + (Vector3)_boxOffset, _boxSize);
    }
}