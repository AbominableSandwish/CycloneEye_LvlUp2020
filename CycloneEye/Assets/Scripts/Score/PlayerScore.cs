using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScore : MonoBehaviour
{
    [SerializeField] int index;
    [SerializeField] GameObject winnerIcon;
    [SerializeField] GameObject goldHammer;

    [SerializeField] List<GameObject> panels;

    private void Start()
    {
        StartCoroutine(ShowAnim());
    }

    IEnumerator ShowAnim()
    {
        yield return new WaitForSeconds(1f);
        for (int i = 0; i < panels.Count; i++)
        {
            if (i < ScoreManager.FinalScore(index))
                panels[i].SetActive(true);
            else
                break;
            yield return new WaitForSeconds(0.5f);
        }
        yield return new WaitForSeconds(0.5f);
        winnerIcon.gameObject.SetActive(ScoreManager.IsWinner(index));
        yield return null;
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(winnerIcon.transform.position), out hit))
            goldHammer.transform.position = hit.point;
        goldHammer.SetActive(ScoreManager.IsWinner(index));
    }
}
