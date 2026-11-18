using UnityEngine;

public class BouncePlatform : MonoBehaviour
{

    public float bounceForce;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Rigidbody2D otherRb = collision.gameObject.GetComponent<Rigidbody2D>();


        if (otherRb != null)
        {
            otherRb.linearVelocity = new Vector2(otherRb.linearVelocity.x, 0f);


            otherRb.AddForce(Vector2.up * bounceForce, ForceMode2D.Impulse);
        }


    }


    
}
