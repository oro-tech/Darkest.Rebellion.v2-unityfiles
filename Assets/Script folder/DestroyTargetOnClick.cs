using UnityEngine;
using UnityEngine.UI;

public class DestroyTargetUI : MonoBehaviour
{
    public GameObject targetObject;   // The object you want to destroy
    public Button triggerButton;      // The UI Button

    private void Start()
    {
        if (triggerButton != null)
        {
            triggerButton.onClick.AddListener(DestroyTarget);
        }
    }

    void DestroyTarget()
    {
        if (targetObject != null)
        {
            targetObject.SetActive(false);
        }
    }
}
