using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Collider2D))]
public class HighlightFunction : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Image imageToHighlight;
    private Color originalColor;
    private Collider2D col;

    private bool isPointerInside = false;

    void Start()
    {
        if (imageToHighlight != null)
            originalColor = imageToHighlight.color;

        col = GetComponent<Collider2D>();
    }

    void Update()
    {
        if (isPointerInside && col != null)
        {
            // Convert mouse position to world space
            Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if (col.OverlapPoint(mouseWorldPos))
            {
                imageToHighlight.color = Color.blue;   // Highlight
                Debug.Log($"{gameObject.name}: Pointer INSIDE collider");
            }
            else
            {
                imageToHighlight.color = originalColor; // Reset
                Debug.Log($"{gameObject.name}: Pointer OUTSIDE collider (still in UI element range)");
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isPointerInside = true;
        Debug.Log($"{gameObject.name}: Pointer ENTERED image UI range");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isPointerInside = false;
        imageToHighlight.color = originalColor;
        Debug.Log($"{gameObject.name}: Pointer EXITED image UI range");
    }
}
