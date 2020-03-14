using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScore : MonoBehaviour
{
    [SerializeField] int index;
    [SerializeField] GameObject winnerIcon;
    [SerializeField] GameObject goldHammer;

    [SerializeField] List<GameObject> panels;
    private MotherFuckingAudioManager audioManager;

    private void Start()
    {
        if(audioManager != null)
            audioManager = GameObject.Find("AudioManager").GetComponent<MotherFuckingAudioManager>();
        StartCoroutine(ShowAnim());
    }

    IEnumerator ShowAnim()
    {
        if (audioManager != null)
            audioManager.SetVolumeMusic(0.3f, true);
        yield return new WaitForSeconds(1f);
        for (int i = 0; i < panels.Count; i++)
        {
            if (i < ScoreManager.FinalScore(index))
            {
                if (ScoreManager.IsWinner(index))
                {
                    if(i < 2)
                        audioManager.PlayAlert(MotherFuckingAudioManager.AlertList.POINT_LVL1);
                    if (i >= 2 && i <= 4)
                        audioManager.PlayAlert(MotherFuckingAudioManager.AlertList.POINT_LVL2);
                    if(i > 4)
                        audioManager.PlayAlert(MotherFuckingAudioManager.AlertList.POINT_LVL3);
                }
                panels[i].SetActive(true);
            }
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
        if (ScoreManager.IsWinner(index))
        {
            if (audioManager != null)
                audioManager.PlayAlert(MotherFuckingAudioManager.AlertList.GOLD_HAMMER);
            goldHammer.SetActive(true);
        }
        yield return new WaitForSeconds(2.0f);
        if (audioManager != null)
            audioManager.SetVolumeMusic(0.4f, true);
    }
}
