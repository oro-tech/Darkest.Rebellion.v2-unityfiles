using UnityEngine;
using UnityEngine.UI;

public class BookIconHandler : MonoBehaviour
{
    public HeroData heroData;  // Reference to the HeroData for the clicked character

    // This method will be triggered when the book icon is clicked
    public void OnBookIconClicked()
    {
        if (HeroDetailPanel.Instance != null && heroData != null)
        {
            HeroDetailPanel.Instance.ShowHeroStory(heroData);  // Call the singleton to show the story
        }
    }
}
