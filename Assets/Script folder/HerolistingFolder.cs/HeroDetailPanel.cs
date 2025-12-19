using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HeroDetailPanel : MonoBehaviour
{
    public static HeroDetailPanel Instance;  // Singleton instance

    public Image portraitImage;
    public GameObject[] itemSlots;  // Array of item slots in the UI where GameObjects will be placed
    public TMP_Text nameText;
    public TMP_Text roleText;
    public TMP_Text maxHPText;
    public TMP_Text damageBonusText;
    public TMP_Text armorText;
    public TMP_Text healingBonusText;
    public TMP_Text speedText;

    public GameObject storyOverlay; // Reference to the scrollable overlay for the hero story
    public Transform storyContainer; // Container for dynamically placing the hero story

    private GameObject instantiatedBook; // Track the instantiated book (book icon)

    void Awake()
    {
        // Singleton pattern to ensure only one instance of HeroDetailPanel
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);  // Destroy this instance if one already exists
        }
    }

    // Display Hero data in the UI
    public void DisplayHero(HeroData hero)
    {
        if (portraitImage != null)
            portraitImage.sprite = hero.portrait;

        // Clear previous items from the UI slots
        ClearItemSlots();
        ClearBook();  // Clear any previously instantiated book

        // Display items in the UI
        if (itemSlots != null && hero.items != null)
        {
            for (int i = 0; i < hero.items.Length && i < itemSlots.Length; i++)
            {
                var itemPrefab = hero.items[i];  // Get the item prefab

                if (itemPrefab != null)
                {
                    // Instantiate the item prefab at the item slot position
                    GameObject itemInstance = Instantiate(itemPrefab);

                    // Set the parent of the item to the corresponding slot and reset the position
                    itemInstance.transform.SetParent(itemSlots[i].transform, false);  // 'false' to preserve local position
                    itemInstance.transform.localPosition = Vector3.zero;  // Reset to match slot position
                    itemInstance.transform.localRotation = Quaternion.identity;  // Reset rotation

                    // If it's the book icon, assign the HeroData dynamically
                    if (itemInstance.name.Contains("Book"))
                    {
                        BookIconHandler bookHandler = itemInstance.GetComponent<BookIconHandler>();
                        if (bookHandler != null)
                        {
                            bookHandler.heroData = hero;  // Dynamically assign the HeroData to this book icon
                        }
                    }
                }
            }
        }

        // Instantiate the Book if it's assigned for this hero
        if (hero.Book != null)
        {
            // Clear any previous book instance
            ClearBook();

            // Instantiate the Book GameObject and set it in the desired location (spawn area)
            instantiatedBook = Instantiate(hero.Book);
            instantiatedBook.transform.SetParent(itemSlots[4].transform, false);  // Set parent to the first slot
            instantiatedBook.transform.localPosition = Vector3.zero;  // Reset to match the position
            instantiatedBook.transform.localRotation = Quaternion.identity;  // Reset rotation

            // Add the onClick listener dynamically for this instantiated book prefab
            Button bookButton = instantiatedBook.GetComponent<Button>();
         
        }

        // Display hero's stats and other info
        if (nameText != null)
            nameText.text = hero.heroName;
        if (roleText != null)
            roleText.text = hero.role.ToString(); // Convert enum to string

        if (maxHPText != null)
            maxHPText.text = $"HP: {hero.maxHealth}";
        if (damageBonusText != null)
            damageBonusText.text = $"DMG Bonus: {hero.damageMultiplier * 100}%";
        if (armorText != null)
            armorText.text = $"Armor: {hero.damageReduction * 100}%";
        if (healingBonusText != null)
            healingBonusText.text = $"Heal Bonus: {hero.healingEffectiveness * 100}%";
        if (speedText != null)
            speedText.text = $"Speed: {hero.speed}";
    }

    // Clears all items from the item slots
    void ClearItemSlots()
    {
        foreach (GameObject slot in itemSlots)
        {
            // Destroy any existing items in the slot
            foreach (Transform child in slot.transform)
            {
                Destroy(child.gameObject);
            }
        }
    }

    // Clears the previous book instance if it exists
    void ClearBook()
    {
        if (instantiatedBook != null)
        {
            Destroy(instantiatedBook);  // Destroy the previously instantiated book
            instantiatedBook = null;  // Reset the reference
        }
    }

    // Show hero story overlay
    public void ShowHeroStory(HeroData hero)
    {
        // First, clear any previous content
        ClearStoryContent();

        if (hero.heroStoryPrefab != null)
        {
            // Instantiate the hero's story prefab inside the story container
            GameObject storyInstance = Instantiate(hero.heroStoryPrefab, storyContainer);

            // Find the TextMeshProUGUI component in the instantiated prefab
            TextMeshProUGUI storyText = storyInstance.GetComponentInChildren<TextMeshProUGUI>();

            // Set the hero's story text in the UI
            if (storyText != null)
            {
                storyText.text = hero.heroStoryText;  // Update the text with the hero's story
            }

            // Optionally, activate the overlay if it's not already active
            storyOverlay.SetActive(true);  // Show the overlay
        }
    }

    // Method to clear old story content
    private void ClearStoryContent()
    {
        foreach (Transform child in storyContainer)
        {
            Destroy(child.gameObject);  // Destroy any old content in the container
        }
    }

    // Method to close the hero story overlay
    public void CloseStory()
    {
        storyOverlay.SetActive(false);  // Hide the story overlay
    }
}
