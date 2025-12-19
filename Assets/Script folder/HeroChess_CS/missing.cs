using UnityEngine;

public class missing: MonoBehaviour
{
    void Start()
    {
        // Find all GameObjects in the scene
        GameObject[] allGameObjects = GameObject.FindObjectsOfType<GameObject>();

        foreach (GameObject go in allGameObjects)
        {
            // Get all components attached to the GameObject
            var components = go.GetComponents<Component>();

            foreach (var component in components)
            {
                // If the component is missing (MonoScript), it will show as null
                if (component == null)
                {
                    Debug.LogWarning($"Missing script on GameObject: {go.name}", go);
                }
            }
        }
    }
}
