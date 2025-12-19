using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [TextArea]
    public string message;

    public void OnPointerEnter(PointerEventData eventData)
    {
        TooltipManager.Instance.SetAndShowToolTip(message);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        TooltipManager.Instance.HideTooltip();
    }

    // Optional: for non-UI objects
    private void OnMouseEnter()
    {
        if (GetComponent<Collider2D>() != null || GetComponent<Collider>() != null)
            TooltipManager.Instance.SetAndShowToolTip(message);
    }

    private void OnMouseExit()
    {
        if (GetComponent<Collider2D>() != null || GetComponent<Collider>() != null)
            TooltipManager.Instance.HideTooltip();
    }
}
