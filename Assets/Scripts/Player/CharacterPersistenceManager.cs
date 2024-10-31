using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterPersistenceManager : MonoBehaviour
{
    private static CharacterPersistenceManager instance;
    public static CharacterPersistenceManager Instance
    {
        get
        {
            if (instance == null)
            {
                var go = new GameObject("CharacterPersistenceManager");
                instance = go.AddComponent<CharacterPersistenceManager>();
                DontDestroyOnLoad(go);
            }
            return instance;
        }
    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SaveCharacterSelection(int characterIndex)
    {
        PlayerPrefs.SetInt("SelectedCharacterIndex", characterIndex);
        PlayerPrefs.Save();
    }

    public int LoadCharacterSelection()
    {
        return PlayerPrefs.GetInt("SelectedCharacterIndex", 0);
    }
}
