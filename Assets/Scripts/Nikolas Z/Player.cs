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

    [Header("Alignment Settings/References")]
    [Tooltip("How fast the player rotates when transitioning to a slope")]
    [SerializeField] private float rotationSpeed;

    [Tooltip("The Player's target Sprite to be used for aligning with the ground")]
    [SerializeField] private Transform spriteTransform;




    private Rigidbody2D _rb;
    private SpriteRenderer sprt;
    private Animator anim;

    private float _movement;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprt = GetComponent<SpriteRenderer>();
    }



    // Update is called once per frame
    private void Update()
    {
        RaycastHit2D hit = Physics2D.BoxCast(transform.position + (Vector3)_boxOffset, _boxSize, 0, Vector2.zero, 0, _boxLayer);


        

        _rb.AddForceX(_movement * _accel * Time.deltaTime);
        _rb.linearVelocityX = Mathf.Clamp(_rb.linearVelocityX, -_topSpeed, _topSpeed);
        anim.SetFloat("Speed", _movement / 5);


        if (hit.collider != null)
        {

            // Rotates the player on the Z-axis to match with the floor (Z = Z axis value of the floor)
            float targetAngle = Mathf.Atan2(hit.normal.y, hit.normal.x) * Mathf.Rad2Deg - 90f;

            Quaternion targetRotation = Quaternion.Euler(0f, 0f, targetAngle);
            spriteTransform.rotation = Quaternion.Lerp(spriteTransform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
        else
        {
            // Reset back to original orientation if the floor is flat or while in the air (Z = 0)
            Quaternion flatRotation = Quaternion.Euler(0f, 0f, 0f);
            spriteTransform.rotation = Quaternion.Lerp(spriteTransform.rotation, flatRotation, rotationSpeed * Time.deltaTime);
        }
       
        


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
        anim.SetTrigger("Jump");


    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        anim.SetBool("IsGrounded", true);
    }


    private void OnCollisionExit2D(Collision2D collision)
    {
        anim.SetBool("IsGrounded", false);
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