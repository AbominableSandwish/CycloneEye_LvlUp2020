using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderTimeValue : MonoBehaviour
{
    [SerializeField] Slider slider;

    private void OnGUI()
    {
        int min = (int)(30 * slider.value/60f);
        int sec = (int)(30 * slider.value) % 60;
        GetComponent<Text>().text = min.ToString("00") + ":" + sec.ToString("00");
    }
}
