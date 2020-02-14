using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrepareGame : MonoBehaviour
{
    [SerializeField] GameObject[] playerOn;
    [SerializeField] GameObject[] playerOff;

    [SerializeField] GameObject notEnoughtPlayer;
    [SerializeField] GameObject pressStartToPlay;
    [SerializeField] MenuManager menuManager;
    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < 4; i++)
            GameManager.playerOrder[i] = -1;
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i <= 4; i++)
        {
            if(Input.GetButtonDown("Attack " + i))
            {
                bool have = false;
                for (int j = 0; j < 4; j++)
                {
                    if (GameManager.playerOrder[j] == i)
                    {
                        have = true;
                        break;
                    }
                }
                if (have) break;
                for (int j = 0; j < 4; j++)
                {
                    if (GameManager.playerOrder[j] == -1)
                    {
                        GameManager.playerOrder[j] = i;
                        playerOn[j].SetActive(true);
                        playerOff[j].SetActive(false);
                        break;
                    }
                }
            }
            else if (Input.GetButtonDown("Cancel " + i))
            {
                bool ok = false;
                for(int j = 0; j < 4; j++)
                {
                    if (GameManager.playerOrder[j] == i)
                    {
                        GameManager.playerOrder[j] = -1;
                        ok = true;
                        playerOn[j].SetActive(false);
                        playerOff[j].SetActive(true);
                    }
                }
                if (!ok) gameObject.SetActive(false);
            }
        }

        int counter = 0;
        for (int j = 0; j < 4; j++)
        {
            if (GameManager.playerOrder[j] != -1)
            {
                counter++;
            }
        }
        notEnoughtPlayer.SetActive(counter < 2);
        pressStartToPlay.SetActive(counter > 1);

        if(counter > 0 && Input.GetButtonDown("Start"))
        {
            menuManager.Play();
        }
    }
}
