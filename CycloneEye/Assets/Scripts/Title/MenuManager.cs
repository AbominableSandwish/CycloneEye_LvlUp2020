using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] BlackPanel blackPanel;
    [SerializeField] GameObject[] menuCursors;

    [SerializeField] GameObject settingsPanel;
    [SerializeField] GameObject gamePanel;

    [SerializeField] Slider numberTurnSlider;
    [SerializeField] Slider timerSlider;

    bool initializing = false;
    int index = 0;
    float timer = 0;
    bool menuActif = true;

    // Start is called before the first frame update
    void Start()
    {
        
        blackPanel.Hide();
        initializing = true;
        numberTurnSlider.value = GameManager.maxRund;
        timerSlider.value = GameManager.startTime/30;
        initializing = false;

    }

    private void Update()
    {
        if (!gamePanel.activeSelf && !settingsPanel.activeSelf && menuActif)
        {
            timer -= Time.deltaTime;
            if (Input.GetAxis("Vertical") > 0 && index > 0 && timer <= 0)
            {
                timer = 0.3f;
                index--;
                UpdateCursors();
            }
            else if (Input.GetAxis("Vertical") < 0 && index < menuCursors.Length - 1 && timer <= 0)
            {
                timer = 0.3f;
                index++;
                UpdateCursors();
            }
            else if (Input.GetButtonDown("Attack"))
            {
                StartCoroutine(ActivatedMenu());
            }
        }
    }

    IEnumerator ActivatedMenu()
    {
        menuCursors[index].GetComponent<Animator>().SetTrigger("activate");
        yield return new WaitForSeconds(0.5f);
        switch (index)
        {
            case 0:
                gamePanel.SetActive(true);
                break;
            case 1:
                settingsPanel.SetActive(true);
                break;
            case 2:
                menuActif = false;
                QuitGame();
                break;
        }
    }

    void UpdateCursors()
    {
        for(int i = 0; i < menuCursors.Length; i++)
        {
            menuCursors[i].GetComponent<Image>().enabled = (i == index);
        }   
    }

    public void Play()
    {
        StartCoroutine(ChangeSceneAnim());
    }

    IEnumerator ChangeSceneAnim()
    {
        yield return blackPanel.ShowAnim();
        SceneManager.LoadScene("Issa");
    }

    public void QuitGame()
    {
        StartCoroutine(QuitAnim());
    }
    IEnumerator QuitAnim()
    {
        yield return blackPanel.ShowAnim();
        Application.Quit();
    }

    public void Settings()
    {
        if (initializing) return;

        GameManager.maxRund = (int)numberTurnSlider.value;
        GameManager.startTime = 30 * timerSlider.value;

    }
}
