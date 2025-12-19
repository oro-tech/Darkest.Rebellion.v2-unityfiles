using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.UI;

public class NewBehaviourScript : MonoBehaviour
{
    public Button toggleButton;    // assign your UI button in Inspector
    public GameObject targetObject; // assign the object you want to toggle

    void Start()
    {
        toggleButton.onClick.AddListener(ToggleActive);
    }

    void ToggleActive()
    {
        if (targetObject != null)
        {
            // flips between active and inactive
            targetObject.SetActive(!targetObject.activeSelf);
        }
    }
}