using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections.Generic;
using System;

[AddComponentMenu("Game/Character/Character Manager")]
public class CharacterManager : MonoBehaviour
{
    [System.Serializable]
    public class Character
    {
        public string name;
        public bool isLocked = false;
        public RuntimeAnimatorController animatorController;
        public Sprite[] sprites;
        public Sprite uiSprite;
    }

    [System.Serializable]
    public class CharacterChangedEvent : UnityEvent<Character> { }

    [Header("Mode Settings")]
    [SerializeField] private bool uiModeOnly = false;
    [SerializeField] private bool persistCharacterSelection = true;

    [Header("Character Settings")]
    [SerializeField] private List<Character> characters = new List<Character>();
    [SerializeField] private Image characterUIImage;
    [SerializeField] private bool allowLockedCharacters = false;

    [Header("Game Character Components")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Animator animator;
    [SerializeField] private new Rigidbody2D rigidbody2D;

    [Header("Events")]
    public CharacterChangedEvent onCharacterChanged;
    public UnityEvent onCharacterSelectionFailed;

    // Properties
    public int CurrentCharacterIndex
    {
        get => currentCharacterIndex;
        private set
        {
            currentCharacterIndex = value;
            if (persistCharacterSelection) SaveCharacter();
        }
    }
    public Character CurrentCharacter => characters[CurrentCharacterIndex];
    public int CharacterCount => characters.Count;

    private int currentCharacterIndex = 0;
    private const string CharacterIndexKey = "CharacterIndex";
    private const string CharacterUnlockPrefix = "CharacterUnlock_";

    private bool IsInitialized => (uiModeOnly || AreComponentsValid()) &&
                                 characters != null &&
                                 characters.Count > 0;

    private void Awake()
    {
        Initialize();
        if (persistCharacterSelection)
        {
            LoadCharacter();
        }
    }

    private void Start()
    {
        ApplyCharacter();
    }

    private void Initialize()
    {
        if (!uiModeOnly)
        {
            TryGetComponents();
        }

        ValidateSetup();
        LoadCharacterUnlockStates();
    }

    private void ValidateSetup()
    {
        if (characters == null || characters.Count == 0)
        {
            Debug.LogError($"[CharacterManager] No characters defined in {gameObject.name}");
            enabled = false;
            return;
        }

        if (characterUIImage == null)
        {
            Debug.LogWarning($"[CharacterManager] Character UI Image not assigned on {gameObject.name}");
        }
    }

    private void TryGetComponents()
    {
        spriteRenderer ??= GetComponent<SpriteRenderer>();
        animator ??= GetComponent<Animator>();
        rigidbody2D ??= GetComponent<Rigidbody2D>();
    }

    public bool TrySelectCharacter(int index)
    {
        if (!IsInitialized || index < 0 || index >= characters.Count) return false;

        Character targetCharacter = characters[index];
        if (!allowLockedCharacters && targetCharacter.isLocked)
        {
            onCharacterSelectionFailed?.Invoke();
            return false;
        }

        CurrentCharacterIndex = index;
        ApplyCharacter();
        UpdateCharacterUI();
        onCharacterChanged?.Invoke(CurrentCharacter);
        return true;
    }

    public void NextCharacter()
    {
        int nextIndex = CurrentCharacterIndex;
        int attempts = 0;

        do
        {
            nextIndex = (nextIndex + 1) % characters.Count;
            attempts++;

            if (attempts >= characters.Count)
            {
                onCharacterSelectionFailed?.Invoke();
                return;
            }
        }
        while (!allowLockedCharacters && characters[nextIndex].isLocked);

        TrySelectCharacter(nextIndex);
    }

    public void PreviousCharacter()
    {
        int prevIndex = CurrentCharacterIndex;
        int attempts = 0;

        do
        {
            prevIndex = (prevIndex - 1 + characters.Count) % characters.Count;
            attempts++;

            if (attempts >= characters.Count)
            {
                onCharacterSelectionFailed?.Invoke();
                return;
            }
        }
        while (!allowLockedCharacters && characters[prevIndex].isLocked);

        TrySelectCharacter(prevIndex);
    }

    public void UnlockCharacter(int index)
    {
        if (index >= 0 && index < characters.Count)
        {
            characters[index].isLocked = false;
            SaveCharacterUnlockState(index);
        }
    }

    public void LockCharacter(int index)
    {
        if (index >= 0 && index < characters.Count)
        {
            characters[index].isLocked = true;
            SaveCharacterUnlockState(index);
        }
    }

    public void ApplyCharacter()
    {
        if (!IsInitialized) return;

        Character currentCharacter = characters[CurrentCharacterIndex];

        if (!uiModeOnly && AreComponentsValid())
        {
            if (animator != null)
            {
                animator.runtimeAnimatorController = currentCharacter.animatorController;
            }

            if (spriteRenderer != null && currentCharacter.sprites != null && currentCharacter.sprites.Length > 0)
            {
                spriteRenderer.sprite = currentCharacter.sprites[0];
            }
        }

        UpdateCharacterUI();
    }

    public void UpdateCharacterUI()
    {
        if (characterUIImage != null && IsInitialized && characters.Count > CurrentCharacterIndex)
        {
            characterUIImage.sprite = characters[CurrentCharacterIndex].uiSprite;
            LayoutRebuilder.ForceRebuildLayoutImmediate(characterUIImage.rectTransform);
        }
    }

    private bool AreComponentsValid()
    {
        if (uiModeOnly) return true;

        bool isValid = true;
        List<string> missingComponents = new List<string>();

        if (spriteRenderer == null) { isValid = false; missingComponents.Add("SpriteRenderer"); }
        if (animator == null) { isValid = false; missingComponents.Add("Animator"); }
        if (rigidbody2D == null) { isValid = false; missingComponents.Add("Rigidbody2D"); }

        if (!isValid)
        {
            Debug.LogWarning($"[CharacterManager] Missing components on {gameObject.name}: {string.Join(", ", missingComponents)}");
        }

        return isValid;
    }

    public void SaveCharacter()
    {
        CharacterPersistenceManager.Instance.SaveCharacterSelection(CurrentCharacterIndex);
        PlayerPrefs.SetInt(CharacterIndexKey, CurrentCharacterIndex);
        PlayerPrefs.Save();
    }

    public void LoadCharacter()
    {
        CurrentCharacterIndex = CharacterPersistenceManager.Instance.LoadCharacterSelection();
        CurrentCharacterIndex = Mathf.Clamp(CurrentCharacterIndex, 0, characters.Count - 1);
        CurrentCharacterIndex = PlayerPrefs.GetInt(CharacterIndexKey, 0);
        CurrentCharacterIndex = Mathf.Clamp(CurrentCharacterIndex, 0, characters.Count - 1);
    }

    public void SaveCharacterUnlockState(int index)
    {
        PlayerPrefs.SetInt($"{CharacterUnlockPrefix}{index}", characters[index].isLocked ? 0 : 1);
        PlayerPrefs.Save();
    }

    public void LoadCharacterUnlockStates()
    {
        for (int i = 0; i < characters.Count; i++)
        {
            bool isUnlocked = PlayerPrefs.GetInt($"{CharacterUnlockPrefix}{i}", characters[i].isLocked ? 0 : 1) == 1;
            characters[i].isLocked = !isUnlocked;
        }
    }

    // Public getters for character information
    public string GetCurrentCharacterName() => CurrentCharacter.name;
    public bool IsCharacterLocked(int index) =>
        index >= 0 && index < characters.Count && characters[index].isLocked;
}