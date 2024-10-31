using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlatformChoice : MonoBehaviour
{

    [Header("Menu Screens")]
    [SerializeField] private GameObject MainCanvas;
    [SerializeField] private GameObject LoadingScreenCanvas;

    public void LoadChosenScene(string SceneToLoad)
    {

        LoadingScreenCanvas.SetActive(true);
        if (MainCanvas.activeInHierarchy)
        {
            MainCanvas.SetActive(false);
        }

        Debug.Log($"Loading '{SceneToLoad}' scene");
        StartCoroutine(LoadSceneASync(SceneToLoad));
    }

    IEnumerator LoadSceneASync(string SceneToLoad)
    {
        AsyncOperation LoadOperation = SceneManager.LoadSceneAsync(SceneToLoad);
        while (!LoadOperation.isDone)
        {
            float progress = LoadOperation.progress * 100;
            Debug.Log(progress + "%");
            Debug.Log(LoadOperation.progress / 0.9f);
            if (LoadOperation.progress >= .9f) { LoadOperation.allowSceneActivation = true; }

            yield return null;
        }
    }

}
