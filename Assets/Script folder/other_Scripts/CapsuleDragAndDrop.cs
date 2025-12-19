using UnityEngine;

public class CapsuleDragAndDrop2D : MonoBehaviour
{
    private bool isDragging = false;
    private Vector3 offset;
    private Camera mainCamera;
    private Transform currentDropZone = null;
    private Vector3 originalPosition;
    private Animator animator;

    void Start()
    {
        mainCamera = Camera.main;
        originalPosition = transform.position;
        animator = GetComponent<Animator>(); // Get the Animator component
        animator.Play("IDLE"); // Start with IDLE animation
    }

    void OnMouseDown()
    {
        isDragging = true;
        Vector3 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        offset = transform.position - new Vector3(mousePos.x, mousePos.y, transform.position.z);

        // Optional: Add logic for a drag animation later
    }

    void OnMouseUp()
    {
        isDragging = false;

        if (currentDropZone != null)
        {
            transform.position = currentDropZone.position;
            Debug.Log("Dropped in a valid drop zone!");
        }
        else
        {
            transform.position = originalPosition;
            Debug.Log("Returned to original position.");
        }

        // Play IDLE animation again after dropping
        animator.Play("IDLE");
    }

    void Update()
    {
        if (isDragging)
        {
            Vector3 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector3(mousePos.x, mousePos.y, transform.position.z) + offset;

            // You can trigger a DRAG animation here later, if available
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("DropZone"))
        {
            currentDropZone = other.transform;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("DropZone") && currentDropZone == other.transform)
        {
            currentDropZone = null;
        }
    }
}
