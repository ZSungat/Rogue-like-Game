using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public static SFXManager instance;
    public AudioSource[] soundEffects;
    public AudioSource[] Music;
    public AudioSource[] SpecificMusic;
    public int currentIndex = -1; // Store the index of the currently playing music
    public bool isMusicStopped = false; // Flag to track if music is currently stopped

    private void Awake()
    {
        instance = this;
    }

    //================================= START  SFX =================================
    public void PlaySFX(int sfxToPlay)
    {
        soundEffects[sfxToPlay].Stop();
        soundEffects[sfxToPlay].Play();
    }

    public void PlaySFXPitched(int sfxToPlay)
    {
        soundEffects[sfxToPlay].pitch = Random.Range(.8f, 1.2f);
        PlaySFX(sfxToPlay);
    }

    public void StopSFX(int sfxToStop)
    {
        soundEffects[sfxToStop].Stop();
    }
    //================================= END  SFX =================================


    //================================= START  MUSIC =================================
    public void PlayMusic(int musicToPlay)
    {
        // Ensure that the index is valid
        if (musicToPlay >= 0 && musicToPlay < Music.Length)
        {
            // Check if the music was previously stopped
            if (isMusicStopped)
            {
                // Resume playing the music from the current index
                Music[currentIndex].Play();
                ScheduleNextRandomMusic(Music[currentIndex].clip.length);
                isMusicStopped = false;
            }
            else
            {
                // Stop any currently playing music if it's not already stopped
                if (!isMusicStopped)
                {
                    StopMusic();
                }

                // Play the specified music track
                Music[musicToPlay].Play();
                ScheduleNextRandomMusic(Music[musicToPlay].clip.length);

                // Update the current index
                currentIndex = musicToPlay;

                // Reset the flag indicating music is stopped
                isMusicStopped = false;
            }
        }
        else
        {
            Debug.LogError("Invalid music index: " + musicToPlay);
        }
    }

    public void StopMusic()
    {
        // Check if currentIndex is valid and the music at currentIndex is playing
        if (currentIndex != -1 && currentIndex < Music.Length && Music[currentIndex].isPlaying)
        {
            Music[currentIndex].Stop(); // Stop the music at currentIndex
            isMusicStopped = true; // Set the flag indicating music is stopped
        }
    }

    // Skip the currently playing music
    public void SkipMusic()
    {
        if (currentIndex != -1)
        {
            StopMusic(); // Stop any currently playing music
            PlayMusic(currentIndex); // Resume playing the music from the current index
        }
        else
        {
            PlayRandomMusic(); // If no music is playing, play a new random music track
        }
    }

    public void PlayRandomMusic()
    {
        if (Music != null && Music.Length > 0)
        {
            int randomIndex = Random.Range(0, Music.Length);

            while (randomIndex == currentIndex && Music.Length > 1)
            {
                randomIndex = Random.Range(0, Music.Length);
            }

            StopMusic();
            Music[randomIndex].Play();
            ScheduleNextRandomMusic(Music[randomIndex].clip.length);
            currentIndex = randomIndex;
        }
        else
        {
            Debug.LogError("Music array is empty or null.");
        }
    }

    private void ScheduleNextRandomMusic(float clipLength)
    {
        CancelInvoke("PlayRandomMusic");
        Invoke("PlayRandomMusic", clipLength);
    }
    //================================= END  MUSIC =================================
}
