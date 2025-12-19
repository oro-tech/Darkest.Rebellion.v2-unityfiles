using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ForwardDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private ScrollRect scrollRect;

    private void Awake()
    {
        // Find the ScrollRect in parent hierarchy
        scrollRect = GetComponentInParent<ScrollRect>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (scrollRect != null)
            scrollRect.OnBeginDrag(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (scrollRect != null)
            scrollRect.OnDrag(eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (scrollRect != null)
            scrollRect.OnEndDrag(eventData);
    }
}
