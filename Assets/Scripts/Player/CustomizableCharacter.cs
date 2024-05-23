using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterManager : MonoBehaviour
{
    [System.Serializable]
    public struct Character
    {
        public string name;
        public RuntimeAnimatorController animatorController;
        public Sprite[] sprites;
        public Sprite uiSprite; // Add a UI sprite for the character
    }

    public List<Character> characters; // List of characters
    private int currentCharacterIndex = 0;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    public Image characterUIImage; // Reference to the UI Image component

    private const string CharacterIndexKey = "CharacterIndex";

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        if (spriteRenderer == null)
        {
            Debug.LogError("SpriteRenderer component not found on this GameObject.");
        }

        if (animator == null)
        {
            Debug.LogError("Animator component not found on this GameObject.");
        }
    }

    void Start()
    {
        LoadCharacter();
        UpdateCharacterUI(); // Update the UI image on start
    }

    void Update()
    {
        // Example input for switching characters
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            NextCharacter();
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            PreviousCharacter();
        }
    }

    public void NextCharacter()
    {
        currentCharacterIndex = (currentCharacterIndex + 1) % characters.Count;
        ApplyCharacter();
        SaveCharacter();
        UpdateCharacterUI(); // Update the UI image when character changes
    }

    public void PreviousCharacter()
    {
        currentCharacterIndex--;
        if (currentCharacterIndex < 0)
        {
            currentCharacterIndex = characters.Count - 1;
        }
        ApplyCharacter();
        SaveCharacter();
        UpdateCharacterUI(); // Update the UI image when character changes
    }

    private void ApplyCharacter()
    {
        if (characters.Count == 0 || spriteRenderer == null || animator == null)
        {
            Debug.LogWarning("Characters list is empty or components are not set.");
            return;
        }

        Character currentCharacter = characters[currentCharacterIndex];
        animator.runtimeAnimatorController = currentCharacter.animatorController;

        if (spriteRenderer.sprite != null && spriteRenderer.sprite.name.Contains("Hooded_Character"))
        {
            string spriteName = spriteRenderer.sprite.name.Replace("Hooded_Character", "");
            if (int.TryParse(spriteName, out int spriteNumber))
            {
                if (spriteNumber >= 0 && spriteNumber < currentCharacter.sprites.Length)
                {
                    spriteRenderer.sprite = currentCharacter.sprites[spriteNumber];
                }
                else
                {
                    Debug.LogWarning("Sprite number out of bounds for selected character.");
                }
            }
            else
            {
                Debug.LogWarning("Failed to parse sprite number.");
            }
        }
    }

    private void SaveCharacter()
    {
        PlayerPrefs.SetInt(CharacterIndexKey, currentCharacterIndex);
        PlayerPrefs.Save();
    }

    private void LoadCharacter()
    {
        currentCharacterIndex = PlayerPrefs.GetInt(CharacterIndexKey, 0);
        ApplyCharacter();
    }

    public void UpdateCharacterUI()
    {
        if (characterUIImage != null && characters.Count > 0)
        {
            characterUIImage.sprite = characters[currentCharacterIndex].uiSprite;
        }
        else
        {
            Debug.LogWarning("Character UI Image or characters list not set.");
        }
    }
}
