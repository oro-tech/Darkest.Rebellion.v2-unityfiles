using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deactivemaanger : MonoBehaviour
{
    // Drag the target object in the Inspector
    public GameObject targetObject;

    // Call this method when pressing a button
    public void DeactivateTarget()
    {
        if (targetObject != null)
        {
            targetObject.SetActive(false);
            Debug.Log(targetObject.name + " has been deactivated!");
        }
    }
}
