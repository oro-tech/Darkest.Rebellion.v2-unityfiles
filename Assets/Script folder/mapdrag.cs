using UnityEngine;
using UnityEngine.EventSystems;

[DisallowMultipleComponent]
public class MapDrag : MonoBehaviour, IBeginDragHandler, IDragHandler
{
    [Header("References")]
    public Canvas canvas;                  // Parent canvas
    public RectTransform viewport;         // The visible frame (UI object with RectMask2D)
    [Tooltip("Leave empty to auto-use this object’s RectTransform")]
    public RectTransform map;              // The map RectTransform

    // Internals
    private Vector2 dragStartPointerLocal; // pointer position in map's local space at drag start
    private Vector2 dragStartAnchored;     // map anchoredPosition at drag start

    void Reset()
    {
        if (!map) map = GetComponent<RectTransform>();
        if (!canvas) canvas = GetComponentInParent<Canvas>();
    }

    void Awake()
    {
        if (!map) map = GetComponent<RectTransform>();
        if (!canvas) canvas = GetComponentInParent<Canvas>();

        // Helpful warnings
        if (!viewport) Debug.LogWarning("[MapDrag] Viewport is not assigned. Clamping will be disabled.", this);
        if (!GetComponent<UnityEngine.UI.Graphic>())
        {
            Debug.LogWarning("[MapDrag] Add an Image/RawImage (Raycast Target ON) so it can receive drag events.", this);
        }
    }

    public void IInitializePotentialDragHandler(PointerEventData eventData)
    {
        // make sure ScrollView-like inertia won't interfere (safety for nested UI)
        eventData.useDragThreshold = false;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        var cam = NeedCamera() ? canvas.worldCamera : null;

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            map, eventData.position, cam, out dragStartPointerLocal))
        {
            dragStartAnchored = map.anchoredPosition;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        var cam = NeedCamera() ? canvas.worldCamera : null;

        if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(
            map, eventData.position, cam, out var currentLocal))
            return;

        // Move by local delta (UI-correct; no screen/world mixing)
        Vector2 delta = currentLocal - dragStartPointerLocal;
        Vector2 target = dragStartAnchored + delta;

        map.anchoredPosition = ClampToViewport(target);
    }

    private bool NeedCamera()
    {
        return canvas && (canvas.renderMode == RenderMode.ScreenSpaceCamera || canvas.renderMode == RenderMode.WorldSpace);
    }

    private Vector2 ClampToViewport(Vector2 targetAnchored)
    {
        if (!viewport) return targetAnchored;

        // Sizes in local UI space
        Vector2 viewSize = viewport.rect.size;
        Vector2 mapSize = map.rect.size;

        // Allow travel only by the "extra" beyond the viewport
        float halfExtraX = Mathf.Max(0f, (mapSize.x - viewSize.x) * 0.5f);
        float halfExtraY = Mathf.Max(0f, (mapSize.y - viewSize.y) * 0.5f);

        // If the map is smaller than the viewport on an axis, keep it centered (no drag on that axis)
        if (mapSize.x <= viewSize.x) targetAnchored.x = 0f;
        else targetAnchored.x = Mathf.Clamp(targetAnchored.x, -halfExtraX, halfExtraX);

        if (mapSize.y <= viewSize.y) targetAnchored.y = 0f;
        else targetAnchored.y = Mathf.Clamp(targetAnchored.y, -halfExtraY, halfExtraY);

        return targetAnchored;
    }
}
