using UnityEngine;
using UnityEngine.InputSystem;

public class PickUpScript : MonoBehaviour
{

    public Transform anchorPoint;
    public float maxRadius;


    [Range(1f, 25f)]
    public float followspeed;

    public float heldLinearDamping;

    public LayerMask targetLayer;


    private Camera mainCamera;
    private Rigidbody2D grabbedRb;
    private Vector2 graboffset;

    private float originalLinearDamping;
    private float originalGravityScale;

    







    private void Awake()
    {
        mainCamera = Camera.main;
    }



    // Start is called once before the first execution of Update after the MonoBehaviour is created
   

    // Update is called once per frame
    void Update()
    {

        if (Mouse.current == null) return;



        if (Mouse.current.leftButton.wasPressedThisFrame)
            tryGrabObject();

        else if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            ReleaseObj();
        }


        
    }


     void FixedUpdate()
    {
       if (grabbedRb != null)
        {
            MoveGrabbedObject();
        }
    }

    private void tryGrabObject()
    {
        Vector2 MouseWorldPos = GetMouseWorldPosition();

        if (anchorPoint != null && Vector2.Distance(anchorPoint.position, MouseWorldPos) > maxRadius)
        {
            return;
        }

        Collider2D hitCollider = Physics2D.OverlapPoint(MouseWorldPos, targetLayer);

        if(hitCollider != null && hitCollider.attachedRigidbody != null)
        {
            grabbedRb = hitCollider.attachedRigidbody;

            graboffset = grabbedRb.position - MouseWorldPos;

            originalLinearDamping = grabbedRb.linearDamping;
            originalGravityScale = grabbedRb.gravityScale;

            grabbedRb.linearDamping = heldLinearDamping;
            grabbedRb.gravityScale = 0f;
            grabbedRb.linearVelocity = Vector2.zero;
        }

    }



    private void MoveGrabbedObject()
    {
        Vector2 MouseWorldPos = GetMouseWorldPosition();
        Vector2 targetPosition = MouseWorldPos + graboffset;


        if(anchorPoint != null)
        {
            Vector2 anchorPos = anchorPoint.position;
            Vector2 offsetFromAnchor = targetPosition - anchorPos;

            offsetFromAnchor = Vector2.ClampMagnitude(offsetFromAnchor, maxRadius);
            targetPosition = anchorPos + offsetFromAnchor;
        }

        Vector2 smoothedPosition = Vector2.Lerp(
            grabbedRb.position,
            targetPosition,
            followspeed * Time.fixedDeltaTime
        );



        grabbedRb.MovePosition(smoothedPosition);
    }


    private void ReleaseObj()
    {
        if(grabbedRb != null)
        {
            grabbedRb.linearDamping = originalLinearDamping;
            grabbedRb.gravityScale = originalGravityScale;
            grabbedRb = null;
        }
    }
















    private Vector2 GetMouseWorldPosition()
    {
        Vector2 mouseScreenPos = Mouse.current.position.ReadValue();

        Vector3 screenPoswithDepth = new Vector3(mouseScreenPos.x, mouseScreenPos.y, -mainCamera.transform.position.z);
        return mainCamera.ScreenToWorldPoint(screenPoswithDepth);
    }



    private void OnDrawGizmosSelected()
    {
        
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(anchorPoint.position, maxRadius);
    }

}
