using UnityEngine;

public class CamMovement : MonoBehaviour
{
    public Transform target;          // Assign your player or object here
    public float smoothSpeed = 0.125f;
    public Vector3 offset = new Vector3(0, 0, -10); // Keeps camera behind the scene

    void LateUpdate()
    {
        if (target != null)
        {
            Vector3 desiredPosition = target.position + offset;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;

            // Prevent any accidental rotation
            transform.rotation = Quaternion.identity;
        }
    }
}
