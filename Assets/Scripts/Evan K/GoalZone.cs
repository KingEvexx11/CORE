using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class GoalZone : MonoBehaviour
{
    //[SerializeField] private GameObject Portal;
    public GameObject portalObject;
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Core"))
        {
            Debug.Log("Core entered the trigger zone!");
            portalObject.SetActive(true);
        }
    }
}
