using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseIndexUI : MonoBehaviour
{
    private void OnEnable()
    {
        GetComponent<Text>().text = "- PLAYER " + GameManager.PauseIndex + " -";
    }
}
