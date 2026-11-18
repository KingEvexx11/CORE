using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(SpriteRenderer), typeof(CapsuleCollider2D))]


public class Portal : MonoBehaviour
{
    [SerializeField] private string DestinationScene;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            SceneManager.LoadScene(DestinationScene);
        }
    }
}
