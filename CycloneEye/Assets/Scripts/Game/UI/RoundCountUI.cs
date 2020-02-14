using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoundCountUI : MonoBehaviour
{

    private void OnEnable()
    {
        GetComponent<Text>().text = "ROUND " + GameManager.Round;
    }
}
