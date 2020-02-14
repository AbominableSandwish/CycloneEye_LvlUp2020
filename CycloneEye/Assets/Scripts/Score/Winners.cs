using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Winners : MonoBehaviour
{
    [SerializeField] Text title;
    [SerializeField] GameObject[] winnerNames;
    [SerializeField] GameObject[] winnerSplitters;
    [SerializeField] GameObject[] playerScores;
    [SerializeField] GameObject pressButtonToContinue;
    [SerializeField] BlackPanel BlackPanel;

    // Start is called before the first frame update
    void Start()
    {
        /*
        int counter = 0;
        for (int i = 0; i < 4; i++)
        {
            if(i < GameManager.playerCount && ScoreManager.IsWinner(i))
                counter++;
        }
        if (counter > 1)
            title.text = "WINNERS";
        else
            title.text = "WINNER";
            */
        for (int i = 0; i < 4; i++)
        {
            playerScores[i].SetActive(i < GameManager.playerCount);
        }
        BlackPanel.Hide();
        StartCoroutine(WaitBeforeShow());
    }

    private void Update()
    {
        if (Input.anyKeyDown && pressButtonToContinue.activeSelf)
        {
            StartCoroutine(BackHome());
        }
    }

    IEnumerator BackHome()
    {
        yield return BlackPanel.ShowAnim();
        SceneManager.LoadScene("SceneTitle");
    }

    IEnumerator WaitBeforeShow()
    {
        yield return new WaitForSeconds(ScoreManager.MaxScore()*0.5f+1.5f);
        for(int i = 0; i < 4; i++)
        {
            bool show = i < GameManager.playerCount && ScoreManager.IsWinner(i);
            winnerNames[i].SetActive(show);
            winnerSplitters[i].SetActive(show);
        }
        winnerSplitters[4].SetActive(true);
        yield return new WaitForSeconds(0.5f);
        pressButtonToContinue.gameObject.SetActive(true);
    }
}
