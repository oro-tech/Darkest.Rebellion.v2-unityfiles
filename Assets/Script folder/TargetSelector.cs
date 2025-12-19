using UnityEngine;

public class Targetselector : MonoBehaviour
{
    public Character character;

    void OnMouseDown()
    {
        // Use TurnManager's Singleton to access SetSelectedTarget method
        if (TurnManager.Instance != null)
        {
      //s      TurnManager.Instance.SetSelectedTarget(character);  // Set the selected target
        }
        else
        {
            Debug.LogError("TurnManager.Instance is null. Cannot set target.");
        }
    }


    

}
