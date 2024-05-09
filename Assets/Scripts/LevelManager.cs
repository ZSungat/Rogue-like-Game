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

    public float WaitToShowEndScreen = 1f;

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


            // SFXManager.instance.StopAllAudio();
            // SFXManager.instance.currentPlayingMusic.Stop();
            // SFXManager.instance.PlayRandomMusic();
        }
    }

    public void EndLevel()
    {
        GameActive = false;

        StartCoroutine(EndLevelCo());
        SFXManager.instance.PlaySFX(12);
    }

    IEnumerator EndLevelCo()
    {
        yield return new WaitForSeconds(WaitToShowEndScreen);

        float minutes = Mathf.FloorToInt(Timer / 60f);
        float seconds = Mathf.FloorToInt(Timer % 60);

        UIController.instance.EndTimeText.text = minutes.ToString() + " mins " + seconds.ToString("00" + " secs");
        UIController.instance.LevelEndScreen.SetActive(true);
    }
}
