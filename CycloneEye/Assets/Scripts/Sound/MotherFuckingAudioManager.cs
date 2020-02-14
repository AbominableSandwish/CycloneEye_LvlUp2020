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
     
    }

    public enum AlertList
    {
        CLICK
    }

    //public void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.Alpha0))
    //    {
    //        PlayAlert(AlertList.CLICK);
    //    }

    //    if (Input.GetKeyDown(KeyCode.Alpha1))
    //    {
    //        PlaySound(SoundList.SNOW);
    //    }

    //    if (Input.GetKeyDown(KeyCode.Alpha2))
    //    {
    //        PlaySound(SoundList.ROCK);
    //    }

    //    if (Input.GetKeyDown(KeyCode.Alpha3))
    //    {
    //        PlaySound(SoundList.TORNADO);
    //    }

    //    if (Input.GetKeyDown(KeyCode.Alpha4))
    //    {
    //        PlaySound(SoundList.UPGRADE);
    //    }

    //    if (Input.GetKeyDown(KeyCode.Alpha5))
    //    {
    //        PlaySound(SoundList.REACTION_HAPPY);
    //    }

    //    if (Input.GetKeyDown(KeyCode.Alpha6))
    //    {
    //        PlaySound(SoundList.REACTION_SAD);
    //    }

    //    if (Input.GetKeyDown(KeyCode.Alpha7))
    //    {
    //        PlaySound(SoundList.COINS);
    //    }
    //}

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
    [Header("Example")]
    [SerializeField] private AudioClip[] reactionHappy;
    [SerializeField] private AudioClip[] reactionSad;

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
                //case AlertList.CLICK:
                //    emitterAvailable.clip = click;
                //    emitterAvailable.outputAudioMixerGroup = AudioConfig.Instance.alert;
                //    break;
            }

            emitterAvailable.Play();
        }
        else
        {
            Debug.Log("no emitter available");
        }
    }

    public void PlaySound(SoundList sound)
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
            
            switch (sound)
            {
                //case SoundList.REACTION_SAD:
                //    emitterAvailable.clip = reactionSad[Random.Range(0, reactionSad.Length)];
                //    emitterAvailable.outputAudioMixerGroup = AudioConfig.Instance.sound;
                //    break;
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
