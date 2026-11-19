using UnityEngine;
using UnityEngine.InputSystem;


[RequireComponent(typeof(LineRenderer))]

public class PickUpScript : MonoBehaviour
{

    public Transform anchorPoint;
    public float maxRadius;


    [Header("Following speed/Drag")]
    [Range(1f, 15f)]
    public float followspeed;

    public float heldLinearDamping;


    [Header("Colors")]
    public Color normalLineColor;
    public Color maxStretchLineColor;



    [Header("Layer(s)")]
    public LayerMask targetLayer;

    

    private Camera mainCamera;
    private Rigidbody2D grabbedRb;
    private Vector2 graboffset;

    private float originalLinearDamping;
    private float originalGravityScale;

    private LineRenderer lineRenderer;







    private void Awake()
    {
        mainCamera = Camera.main;


        if(!TryGetComponent(out lineRenderer))
        {
            lineRenderer = gameObject.AddComponent<LineRenderer>();
        }

        lineRenderer.positionCount = 2;
        lineRenderer.useWorldSpace = true;
        lineRenderer.enabled = false;
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


    private void LateUpdate()
    {
        updateVisualLine();
    }



    // Checks to see if the object can be grabbed
    // PreCondition: Object must have the same target layer
    private void tryGrabObject()
    {
        Vector2 MouseWorldPos = GetMouseWorldPosition();

        if (anchorPoint != null && Vector2.Distance(anchorPoint.position, MouseWorldPos) > maxRadius)
        {
            return;
        }

        Collider2D hitCollider = Physics2D.OverlapPoint(MouseWorldPos, targetLayer);


        // "Is the object grabbable?"
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




        if (anchorPoint != null)
        {
            lineRenderer.enabled = true;
            updateVisualLine();
        }

    }


    // Allows for the player to move the object
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

    // Draws a visual line that connects to the player and the grabbed object 
    private void updateVisualLine()
    {
        if (lineRenderer == null) return;


        if(grabbedRb == null || anchorPoint == null)
        {
            lineRenderer.enabled = false;
            return;
        }


        lineRenderer.enabled = true;

        Vector3 startPos = anchorPoint.position;
        Vector3 endPos = grabbedRb.transform.position;
        startPos.z = 0f;
        endPos.z = 0f;
            

        lineRenderer.SetPosition(0, startPos);
        lineRenderer.SetPosition(1, endPos);





        float stretchRaito = maxRadius > 0f
            ? Vector2.Distance(anchorPoint.position, grabbedRb.position) / maxRadius
            : 0f;

        Color currentColor = Color.Lerp(normalLineColor, maxStretchLineColor, stretchRaito);
        lineRenderer.startColor = currentColor;
        lineRenderer.endColor = currentColor;



    }


    // Releases the grabbed object from the player
    private void ReleaseObj()
    {
        if (grabbedRb != null)
        {
            grabbedRb.linearDamping = originalLinearDamping;
            grabbedRb.gravityScale = originalGravityScale;
            grabbedRb = null;
        }


        lineRenderer.enabled = false;
    }



    // Gets the mouse's Position within the game, to check and see if the mouse is hovering over a grabbable object
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
