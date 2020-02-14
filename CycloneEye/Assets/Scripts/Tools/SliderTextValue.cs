using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderTextValue : MonoBehaviour
{
    [SerializeField] Slider slider;

    private void OnGUI()
    {
        GetComponent<Text>().text = slider.value.ToString();
    }
}
