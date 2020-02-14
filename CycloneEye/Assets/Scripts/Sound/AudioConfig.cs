using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;



public class AudioConfig : MonoBehaviour
{
    public static AudioConfig Instance;
    
    [Header("Menu Audio")]
    private float mainVolume;
    private float musicVolume;
    private float soundVolume;
    private float alertVolume;

    [SerializeField] private Slider MainSlider;
    [SerializeField] private Slider MusicSlider;
    [SerializeField] private Slider SoundSlider;
    [SerializeField] private Slider AlertSlider;

    [SerializeField] private Text TxtMain;
    [SerializeField] private Text TxtMusic;
    [SerializeField] private Text TxtSound;
    [SerializeField] private Text TxtAlert;

    [Header("Audio")]
    [SerializeField] private AudioMixer MixerAudio;

    [SerializeField] public AudioMixerGroup sound;
    [SerializeField] public AudioMixerGroup alert;

    private const int MAX_DECIBEL = 80;
    
    void Awake()
    {
        if (Instance != this && Instance != null)
            Destroy(this);
        else
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
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

    public void SetVolume()
    {
        if (MainSlider.value != mainVolume)
        {
            mainVolume = (int)MainSlider.value;
            TxtMain.text = mainVolume.ToString();
            MixerAudio.SetFloat("Master", LinearToDecibel(mainVolume/100));
        }

        if (MusicSlider.value != musicVolume)
        {
            musicVolume = (int)MusicSlider.value;
            TxtMusic.text = musicVolume.ToString();
            MixerAudio.SetFloat("Music", LinearToDecibel(musicVolume / 100));
           
        }

        if (SoundSlider.value != soundVolume)
        {
            soundVolume = (int)SoundSlider.value;
            TxtSound.text = soundVolume.ToString();
            MixerAudio.SetFloat("Sound", LinearToDecibel(soundVolume / 100));
        }

        if (AlertSlider.value != alertVolume)
        {
            alertVolume = (int)AlertSlider.value;
            TxtAlert.text = alertVolume.ToString();
            MixerAudio.SetFloat("Alert", LinearToDecibel(alertVolume / 100));
        }
    }
}
