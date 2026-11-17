using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class TriggerEvents2d : MonoBehaviour
{

    public UnityEvent onEnter;
    public UnityEvent onStay;
    public UnityEvent OnExit;

    // Triggered when an object has entered the collider
    private void OnTriggerEnter2D(Collider2D collision)
    {
        onEnter.Invoke();
    }

    // Triggered when an object is inside the collider
    private void OnTriggerStay2D(Collider2D collision)
    {
        onStay.Invoke();
    }


    // Triggered when an object has exited the collider
    private void OnTriggerExit2D(Collider2D collision)
    {
        OnExit.Invoke();
    }


}