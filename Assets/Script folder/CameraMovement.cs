using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public RectTransform canvasTransform; // Reference to the Canvas RectTransform
    public float moveSpeed = 0.1f; // Movement speed of the map
    public float mapWidth = 1000f; // Width of the map
    public float mapHeight = 1000f; // Height of the map

    private Vector3 lastMousePosition;

    void Update()
    {
        HandleMapMovement();
    }

    private void HandleMapMovement()
    {
        if (Input.GetMouseButtonDown(0)) // Left mouse button pressed
        {
            lastMousePosition = Input.mousePosition;
        }

        if (Input.GetMouseButton(0)) // While holding and dragging
        {
            Vector3 delta = Input.mousePosition - lastMousePosition;
            Vector3 newPosition = canvasTransform.position - new Vector3(delta.x, delta.y, 0) * moveSpeed;

            // Calculate map boundaries
            float halfWidth = canvasTransform.rect.width * canvasTransform.lossyScale.x / 2f;
            float halfHeight = canvasTransform.rect.height * canvasTransform.lossyScale.y / 2f;

            float xMin = -mapWidth / 2f + halfWidth;
            float xMax = mapWidth / 2f - halfWidth;
            float yMin = -mapHeight / 2f + halfHeight;
            float yMax = mapHeight / 2f - halfHeight;

            // Clamp position
            newPosition.x = Mathf.Clamp(newPosition.x, xMin, xMax);
            newPosition.y = Mathf.Clamp(newPosition.y, yMin, yMax);

            // Apply movement
            canvasTransform.position = newPosition;

            lastMousePosition = Input.mousePosition;
        }
    }
}
