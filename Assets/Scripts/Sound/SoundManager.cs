using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    private static SoundManager instance;

    public AudioMixerGroup musicMixerGroup;
    public AudioMixerGroup soundEffetcsMixerGroup;

    public Sound[] sounds;

    private void Awake()
    {
        instance = this;
        foreach(Sound s in sounds)
        {
            s.audioSource = gameObject.AddComponent<AudioSource>();
            s.audioSource.clip = s.audioClip;
            s.audioSource.volume = s.volume;
            s.audioSource.pitch = s.pitch;
            s.audioSource.loop = s.loop;

            switch(s.audioType)
            {
                case Sound.AudioTypes.SoundEffect:
                    s.audioSource.outputAudioMixerGroup = soundEffetcsMixerGroup;
                    break;
                case Sound.AudioTypes.Music:
                    s.audioSource.outputAudioMixerGroup = musicMixerGroup;
                    break;
            }
        }
    }

    private void playSound(string name)
    {
        //print("SoundManager.cs<< play sound: " + name);
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if(s == null)
        {
            Debug.LogWarning("SoundManager.cs<< Sound " + name + " not found!");
            return;
        }
        s.audioSource.PlayOneShot(s.audioClip);
    }

    public static SoundManager GetInstance() { return instance; }

    
    public void PlayBackgroundMusic()
    {
        playSound("BackgroundMusic");
    }

    public void PlayShootingSound()
    {
        playSound("ShootingSound");
    }

    public void PlayBoomSound()
    {
        playSound("BoomSound");
    }

    // UI sounds
    public void PlayScoreGoesUpSound()
    {
        playSound("ScoreGoesUpSound");
    }

    public void PlayHealthGoesDownSound()
    {
        playSound("HealthGoesDownSound");
    }

    public void PlayLevelUpSound()
    {
        playSound("LevelUpSound");
    }

    public void PlayGameOverSound()
    {
        playSound("GameOverSound");
    }

    public void PlayChallengeSound()
    {
        playSound("ChallengeSound");
    }
    
    public void PlayHighscoreSound()
    {
        playSound("HighscoreSound");
    }

    public void PlayClickSound()
    {
        playSound("ClickSound");
    }




}
