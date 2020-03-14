using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundSystem : MonoBehaviour
{
    private MotherFuckingAudioManager audioManager;
    // Start is called before the first frame update
    void Start()
    {
        if(GameObject.Find("AudioManager") != null)
            audioManager = GameObject.Find("AudioManager").GetComponent<MotherFuckingAudioManager>();
    }

    public void Woosh()
    {
        audioManager.PlaySound(MotherFuckingAudioManager.SoundList.WOOSH);
    }
}
