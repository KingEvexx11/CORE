using UnityEngine;
using UnityEngine.ImputSystem

public class movement : MonoBehaviour
{
    public float moveSpeed;
    public float jumpHeight;

    private Rigidbody2D rb2d;

    private float _movement;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    {
        rb2d.linearVelocityX = _movement;
    }

    {
        
    }
}