using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundSystem : MonoBehaviour
{
    private MotherFuckingAudioManager audioManager;
    // Start is called before the first frame update
    void Start()
    {
        audioManager = GameObject.Find("AudioManager").GetComponent<MotherFuckingAudioManager>();
    }

    public void Woosh()
    {
        audioManager.PlaySound(MotherFuckingAudioManager.SoundList.WOOSH);
    }
}
