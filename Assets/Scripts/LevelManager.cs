using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    void Awake()
    {
        instance = this;
    }
    private bool GameActive;
    public float Timer;
    // Start is called before the first frame update
    void Start()
    {
        GameActive = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameActive == true)
        {
            Timer += Time.deltaTime;
            UIController.instance.UpdateTimer(Timer);
        }
    }
    public void EndLevel()
    {
        GameActive = false;

        // StartCoroutine(EndLevelCo());
    }
}
