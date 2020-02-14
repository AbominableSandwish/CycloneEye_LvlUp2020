using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] BlackPanel blackPanel;
    [SerializeField] GameObject[] menuCursors; 
    [SerializeField] GameObject[] settingsMenuCursors;

    [SerializeField] GameObject settingsPanel;
    [SerializeField] GameObject gamePanel;

    [SerializeField] Slider numberTurnSlider;
    [SerializeField] Slider timerSlider;

    bool initializing = false;
    int index = 0;
    float timer = 0;
    bool menuActif = true;

    int settingsIndex = 0;

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
            if (Input.GetAxisRaw("Vertical") > 0 && index > 0 && timer <= 0)
            {
                timer = 0.5f;
                index--;
                UpdateCursors();
            }
            else if (Input.GetAxisRaw("Vertical") < 0 && index < menuCursors.Length - 1 && timer <= 0)
            {
                timer = 0.5f;
                index++;
                UpdateCursors();
            }
            else if (Input.GetButtonDown("Attack"))
            {
                StartCoroutine(ActivatedMenu());
            }
            else if (Input.GetAxisRaw("Vertical") == 0)
            {
                timer = 0;
            }
        } else if (settingsPanel.gameObject.activeSelf)
        {
            timer -= Time.deltaTime;
            if (Input.GetAxisRaw("Vertical") > 0 && settingsIndex > 0 && timer <= 0)
            {
                timer = 0.5f;
                settingsIndex--;
                UpdateSettingsCursors();
            }
            else if (Input.GetAxisRaw("Vertical") < 0 && settingsIndex < 1 && timer <= 0)
            {
                timer = 0.5f;
                settingsIndex++;
                UpdateSettingsCursors();
            }
            else if(Input.GetButtonDown("Cancel"))
            {
                settingsPanel.gameObject.SetActive(false);
            }
            else if (Input.GetAxisRaw("Vertical") == 0)
            {
                timer = 0;
            }
            else
            {
                switch (settingsIndex)
                {
                    case 0:
                        TryChangeSliderValue(numberTurnSlider);
                        break;
                    case 1:
                        TryChangeSliderValue(timerSlider);
                        break;
                }
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
                settingsIndex = 0;
                UpdateSettingsCursors();
                settingsPanel.SetActive(true);
                break;
            case 2:
                menuActif = false;
                QuitGame();
                break;
        }
    }
    float timerH = 0;
    void TryChangeSliderValue(Slider s)
    {
        timerH -= Time.deltaTime;
        if (Input.GetAxisRaw("Horizontal") < 0 && s.value > 0 && timerH <= 0)
        {
            timerH = 0.5f;
            s.value--;
        }
        else if (Input.GetAxisRaw("Horizontal") > 0 && s.value < s.maxValue && timerH <= 0)
        {
            timerH = 0.5f;
            s.value++;
        }
        else if (Input.GetAxisRaw("Horizontal") == 0)
        {
            timerH = 0;
        }
    }

    void UpdateCursors()
    {
        for(int i = 0; i < menuCursors.Length; i++)
        {
            menuCursors[i].GetComponent<Image>().enabled = (i == index);
        }
    }
    void UpdateSettingsCursors()
    {
        for (int i = 0; i < settingsMenuCursors.Length; i++)
        {
            settingsMenuCursors[i].GetComponent<Image>().enabled = (i == settingsIndex);
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
