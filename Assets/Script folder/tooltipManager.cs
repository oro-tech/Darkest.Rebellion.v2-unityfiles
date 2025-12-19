using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TooltipManager : MonoBehaviour
{
    public static TooltipManager Instance { get; private set; }

    [SerializeField] private RectTransform tooltipRect;
    [SerializeField] private TextMeshProUGUI tooltipText;
    [SerializeField] private Vector2 padding = new Vector2(10f, 10f);

    private Canvas canvas;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        gameObject.SetActive(false);

        canvas = GetComponentInParent<Canvas>();
        if (canvas == null)
            Debug.LogWarning("TooltipManager must be inside a Canvas!");
    }

    private void Update()
    {
        if (gameObject.activeSelf)
            FollowMouse();
    }

    public void SetAndShowToolTip(string message)
    {
        tooltipText.text = message;
        gameObject.SetActive(true);
        LayoutRebuilder.ForceRebuildLayoutImmediate(tooltipRect); // update layout immediately
        FollowMouse();
    }

    public void HideTooltip()
    {
        gameObject.SetActive(false);
        tooltipText.text = string.Empty;
    }

    private void FollowMouse()
    {
        Vector2 mousePos = Input.mousePosition;

        // Convert to canvas space
        RectTransform canvasRect = canvas.transform as RectTransform;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, mousePos, canvas.worldCamera, out Vector2 localPoint);

        // Offset tooltip a bit away from cursor
        Vector2 targetPos = localPoint + padding;

        // Get tooltip and canvas sizes
        Vector2 tooltipSize = tooltipRect.sizeDelta;
        Vector2 canvasSize = canvasRect.sizeDelta;

        // Clamp tooltip so it stays inside screen bounds
        float pivotX = 0f;
        float pivotY = 1f;

        if (localPoint.x + tooltipSize.x + padding.x > canvasSize.x / 2f)
            pivotX = 1f;
        if (localPoint.y - tooltipSize.y - padding.y < -canvasSize.y / 2f)
            pivotY = 0f;

        tooltipRect.pivot = new Vector2(pivotX, pivotY);
        tooltipRect.anchoredPosition = targetPos;
    }
}
