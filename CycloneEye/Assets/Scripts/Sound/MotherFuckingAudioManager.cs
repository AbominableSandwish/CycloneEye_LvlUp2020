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
        SCORE
    }

    public enum SoundList
    {
        PLAYER_FALL,
        WIND,
        WOOSH,
        HIT,
        WALL_DESTROYED,
        PARADE
    }

    public enum AlertList
    {
        BTN_VALIDATION,
        GOLD_HAMMER,
        POINT_LVL1,
        POINT_LVL2,
        POINT_LVL3,
        EXIT_GAME

    }

    MusicList currentMusicPlaying = MusicList.NONE;
    [Header("Emmiters")]
    [SerializeField] private int soundEmitterNumber;
    [SerializeField] private GameObject emitterPrefab;
    [SerializeField] private AudioSource[] musicEmitters;

    [Header("Music")]
    [SerializeField] private AudioClip mainMusic;
    [SerializeField] private AudioClip menuMusic;
    [SerializeField] private AudioClip ScoreMusic;
    [Header("Sound")]
    [SerializeField] private AudioClip playeFall;
    [SerializeField] private AudioClip Wind;
    [SerializeField] private AudioClip[] hits;
    [SerializeField] private AudioClip[] wooshs;
    [SerializeField] private AudioClip parade;

    [Header("Alert")]
    [SerializeField] private AudioClip ButtonValidation;

    [SerializeField] private AudioClip Winner;
    [SerializeField] private AudioClip PointLvl1;
    [SerializeField] private AudioClip PointLvl2;
    [SerializeField] private AudioClip PointLvl3;
    [SerializeField] private AudioClip ExitGame;

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

    public void StopAllMusic()
    {
        foreach (AudioSource emitter in musicEmitters)
        {
            if (emitter.isPlaying)
            {
                if (emitter.outputAudioMixerGroup == AudioConfig.Instance.music)
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
        
    }

    private void Start()
    {
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
                emitterAvailable.volume = 1.0f;
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
                case AlertList.GOLD_HAMMER:
                    emitterAvailable.volume = 0.8f;
                    emitterAvailable.clip = Winner;
                    emitterAvailable.outputAudioMixerGroup = AudioConfig.Instance.alert;
                    break;
                case AlertList.POINT_LVL1:
                    emitterAvailable.clip = PointLvl1;
                    emitterAvailable.outputAudioMixerGroup = AudioConfig.Instance.alert;
                    break;
                case AlertList.POINT_LVL2:
                    emitterAvailable.clip = PointLvl2;
                    emitterAvailable.outputAudioMixerGroup = AudioConfig.Instance.alert;
                    break;
                case AlertList.POINT_LVL3:
                    emitterAvailable.clip = PointLvl3;
                    emitterAvailable.outputAudioMixerGroup = AudioConfig.Instance.alert;
                    break;
                case AlertList.EXIT_GAME:
                    emitterAvailable.clip = ExitGame;
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
                    emitterAvailable.volume = 0.5f;
                    emitterAvailable.clip = Wind;
                    emitterAvailable.outputAudioMixerGroup = AudioConfig.Instance.sound;
                    break;
                case SoundList.WOOSH:
                    emitterAvailable.clip = wooshs[Random.Range(0, hits.Length)];
                    emitterAvailable.outputAudioMixerGroup = AudioConfig.Instance.sound;
                    break;
                case SoundList.HIT:
                    emitterAvailable.clip = hits[Random.Range(0, hits.Length)];
                    emitterAvailable.outputAudioMixerGroup = AudioConfig.Instance.sound;
                    break;
                case SoundList.WALL_DESTROYED:
                    emitterAvailable.clip = Wind;
                    emitterAvailable.outputAudioMixerGroup = AudioConfig.Instance.sound;
                    break;
                case SoundList.PARADE:
                    emitterAvailable.clip = parade;
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
                        emitterAvailable.outputAudioMixerGroup = AudioConfig.Instance.music;
                        emitterAvailable.clip = mainMusic;
                        emitterAvailable.Play();
                        break;
                    case MusicList.MENU:
                        emitterAvailable.outputAudioMixerGroup = AudioConfig.Instance.music;
                        emitterAvailable.clip = menuMusic;
                        emitterAvailable.Play();
                        break;
                    case MusicList.SCORE:
                        emitterAvailable.outputAudioMixerGroup = AudioConfig.Instance.music;
                        emitterAvailable.volume = 0.4f;
                        emitterAvailable.clip = ScoreMusic;
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
        for (float ft = 0f; ft <= 10f; ft += 0.5f)
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



    public void SetVolumeMusic(float volume, bool fade = false)
    {
        musicEmitters[0].volume = volume;
        musicEmitters[1].volume = volume;
    }
}
