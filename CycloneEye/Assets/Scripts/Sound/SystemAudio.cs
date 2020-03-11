using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemAudio : MonoBehaviour
{
    public MotherFuckingAudioManager audioManager;
    // Start is called before the first frame update
    void Start()
    {
        audioManager = GameObject.Find("AudioManager").GetComponent<MotherFuckingAudioManager>();
    }


    public void HammerAlert()
    {
        audioManager.PlayAlert(MotherFuckingAudioManager.AlertList.BTN_VALIDATION);
    }
}
