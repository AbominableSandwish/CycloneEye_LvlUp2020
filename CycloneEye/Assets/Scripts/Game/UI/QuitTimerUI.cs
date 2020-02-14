using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitTimerUI : MonoBehaviour
{

    float timer = 0;
    bool quit = false;
    void Update()
    {
        if (quit) return;
        if (Input.GetButton("Cancel " + GameManager.PauseIndex))
            timer = Mathf.Min(1, timer + 0.005f);
        else
            timer = 0;

        transform.localScale = new Vector3(timer, 1, 1);
        if (timer >= 1)
        {
            GameManager.Quit();
            quit = true;
        }
    }
}
