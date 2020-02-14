using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using Random = UnityEngine.Random;

public class MotherFuckingAudioManager : MonoBehaviour
{
    public static MotherFuckingAudioManager Instance;
    
    List<AudioSource> emitters = new List<AudioSource>();

    public enum MusicList
    {
        NONE,
        MAIN,
        MENU,
        OVER
    }

    public enum SoundList
    {
        PLAYER_FALL,
        WIND
    }

    public enum AlertList
    {
        BTN_VALIDATION
    }

    MusicList currentMusicPlaying = MusicList.NONE;
    [Header("Emmiters")]
    [SerializeField] private int soundEmitterNumber;
    [SerializeField] private GameObject emitterPrefab;
    [SerializeField] private AudioSource[] musicEmitters;

    [Header("Music")]
    [SerializeField] private AudioClip mainMusic;
    [SerializeField] private AudioClip menuMusic;
    [SerializeField] private AudioClip gameOver;
    [Header("Sound")]
    [SerializeField] private AudioClip playeFall;
    [SerializeField] private AudioClip Wind;
    [Header("Alert")]
    [SerializeField] private AudioClip ButtonValidation;

    public void StopAllSound()
    {
        foreach (AudioSource emitter in emitters)
        {
            if (emitter.isPlaying)
            {
                if (emitter.outputAudioMixerGroup == AudioConfig.Instance.alert)
                {
                    emitter.Stop();
                    emitter.clip = null;
                }
            }
        }
    }

    private void Awake()
    {
        if(Instance != this && Instance != null)
            Destroy(gameObject);
        else
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }

        for (int i = 0; i <= soundEmitterNumber; i++)
        {
            GameObject audioObject = Instantiate(emitterPrefab, emitterPrefab.transform.position, emitterPrefab.transform.rotation);
            emitters.Add(audioObject.GetComponent<AudioSource>());
            DontDestroyOnLoad(audioObject);
        }

        musicEmitters = GetComponents<AudioSource>();
        PlayMusic(MusicList.MENU);
    }

    public void PlayAlert(AlertList alert)
    {
        AudioSource emitterAvailable = null;

        foreach (AudioSource emitter in emitters)
        {
            if (!emitter.isPlaying)
            {
                emitterAvailable = emitter;
                break;
            }
        }

        if (emitterAvailable != null)
        {
            emitterAvailable.loop = false;

            switch (alert)
            {
                case AlertList.BTN_VALIDATION:
                    emitterAvailable.clip = ButtonValidation;
                    emitterAvailable.outputAudioMixerGroup = AudioConfig.Instance.alert;
                    break;
            }

            emitterAvailable.Play();
        }
        else
        {
            Debug.Log("no emitter available");
        }
    }

    public void PlaySound(SoundList sound, bool loop = false)
    {
        AudioSource emitterAvailable = null;
        
        foreach (AudioSource emitter in emitters)
        {
            if (!emitter.isPlaying)
            {
                emitterAvailable = emitter;
                break;
            }
        }

        if (emitterAvailable != null)
        {
            emitterAvailable.loop = loop;
            
            switch (sound)
            {
                case SoundList.PLAYER_FALL:
                    emitterAvailable.clip = playeFall;
                    emitterAvailable.outputAudioMixerGroup = AudioConfig.Instance.sound;
                    break;
                case SoundList.WIND:
                    emitterAvailable.clip = Wind;
                    emitterAvailable.outputAudioMixerGroup = AudioConfig.Instance.sound;
                    break;
            }

            emitterAvailable.Play();
        }
        else
        {
            Debug.Log("no emitter available");
        }
    }

    public AudioSource PlayMusic(MusicList music, bool fade = false)
    {
        AudioSource emitterAvailable = null;
        AudioSource emitterPlaying = null;

        foreach (AudioSource emitter in musicEmitters)
        {
            if (emitter.isPlaying)
            {
                emitterPlaying = emitter;
            }
            else
            {
                emitterAvailable = emitter;
            }
        }

        if (emitterAvailable != null)
        {
            if (currentMusicPlaying != music)
            {
                emitterAvailable.loop = true;

                switch (music)
                {
                    case MusicList.MAIN:
                        emitterAvailable.clip = mainMusic;
                        emitterAvailable.Play();
                        break;
                    case MusicList.MENU:
                        emitterAvailable.clip = menuMusic;
                        emitterAvailable.Play();
                        break;
                    case MusicList.OVER:
                        emitterAvailable.clip = gameOver;
                        emitterAvailable.Play();
                        break;
                }

                currentMusicPlaying = music;
                if (fade)
                {
                    emitterAvailable.volume = 0;
                    StartCoroutine(Fade(emitterAvailable, emitterPlaying));
                }
            }
        }

        return emitterAvailable;
    }

    private float LinearToDecibel(float linear)
    {
        float dB;

        if (linear != 0)
            dB = 20.0f * Mathf.Log10(linear);
        else
            dB = -144.0f;

        return dB;
    }

    IEnumerator Fade(AudioSource emitterIn, AudioSource emitterOut)
    {
        float volumeIn;
        float volumeOut;
        for (float ft = 0f; ft <= 10f; ft += 0.3f)
        {
            volumeIn = ft/10f;
            volumeOut = (10f - ft)/10f;

            emitterIn.volume = volumeIn;
            emitterOut.volume = volumeOut;

            yield return new WaitForSeconds(.1f);
        }

        emitterIn.volume = 1f;
        emitterOut.volume = 0f;
        emitterOut.Stop();
        emitterOut.clip = null;
    }
}
