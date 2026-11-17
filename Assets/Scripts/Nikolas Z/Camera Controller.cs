using UnityEngine;

public class CameraController : MonoBehaviour
{

    public GameObject target;
    public float cameraSpeed = 5f;
    public Vector3 camOffset;


    

    void LateUpdate()
    {
        if (target != null)
        {
            transform.position = Vector3.Lerp(transform.position, target.transform.position + camOffset, Time.deltaTime * cameraSpeed);
        }

    }
}