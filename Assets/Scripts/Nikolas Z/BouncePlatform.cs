using Unity.VisualScripting;
using UnityEngine;

public class BouncePlatform : MonoBehaviour
{
    [Header("Force")]
    [Tooltip("Amount of force that is outputed to the touched object (Higher the force, the more high up the object goes)")]
    public float bounceForce;

    

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        Rigidbody2D otherRb = collision.gameObject.GetComponent<Rigidbody2D>();


        if (otherRb != null && otherRb.gravityScale != 0)
        {
            otherRb.linearVelocity = new Vector2(otherRb.linearVelocity.x, 0f);


            otherRb.AddForce(Vector2.up * bounceForce, ForceMode2D.Impulse);
        }


    }


    
}
