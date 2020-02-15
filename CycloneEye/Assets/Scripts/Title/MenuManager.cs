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
    [SerializeField] Slider[] audioSliders;


    bool initializing = false;
    int index = 0;
    float timer = 0;
    bool menuActif = true;

    int settingsIndex = 0;

    private MotherFuckingAudioManager audioManager;

    // Start is called before the first frame update
    void Start()
    {
        audioManager = GameObject.Find("AudioManager").GetComponent<MotherFuckingAudioManager>();
        blackPanel.Hide();
        initializing = true;
        numberTurnSlider.value = GameManager.maxRund;
        timerSlider.value = GameManager.startTime/30;
        initializing = false;
        audioManager.PlayMusic(MotherFuckingAudioManager.MusicList.MENU);

    }

    bool firstTimer = true;
    private void Update()
    {
        if (!gamePanel.activeSelf && !settingsPanel.activeSelf && menuActif)
        {
            timer -= Time.deltaTime;
            if (Input.GetAxisRaw("Vertical") > 0 && index > 0 && timer <= 0)
            {
                timer = (firstTimer)?0.5f:0.1f;
                firstTimer = false;
                index--;
                UpdateCursors();
            }
            else if (Input.GetAxisRaw("Vertical") < 0 && index < menuCursors.Length - 1 && timer <= 0)
            {
                timer = (firstTimer) ? 0.5f : 0.1f;
                firstTimer = false;
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
                firstTimer = true;
            }
        } else if (settingsPanel.gameObject.activeSelf)
        {
            timer -= Time.deltaTime;
            if (Input.GetAxisRaw("Vertical") > 0 && settingsIndex > 0 && timer <= 0)
            {
                timer = (firstTimer) ? 0.5f : 0.1f;
                firstTimer = false;
                settingsIndex--;
                UpdateSettingsCursors();
            }
            else if (Input.GetAxisRaw("Vertical") < 0 && settingsIndex < 4 && timer <= 0)
            {
                timer = (firstTimer) ? 0.5f : 0.1f;
                firstTimer = false;
                settingsIndex++;
                UpdateSettingsCursors();
            }
            else if(Input.GetButtonDown("Cancel"))
            {
                audioManager.PlayAlert(MotherFuckingAudioManager.AlertList.BTN_VALIDATION);
                settingsPanel.gameObject.SetActive(false);
            }
            else
            {
                if (Input.GetAxisRaw("Vertical") == 0)
                {
                    timer = 0;
                    firstTimer = true;
                }
                switch (settingsIndex)
                {
                    case 0:
                        TryChangeSliderValue(numberTurnSlider, 0.1f);
                        break;
                    case 1:
                        TryChangeSliderValue(timerSlider, 0.1f);
                        break;
                    case 2:
                        TryChangeSliderValue(audioSliders[0], 0.01f);
                        break;
                    case 3:
                        TryChangeSliderValue(audioSliders[1], 0.01f);
                        break;
                    case 4:
                        TryChangeSliderValue(audioSliders[2], 0.01f);
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
    bool firstTimerH = true;
    void TryChangeSliderValue(Slider s, float delay)
    {
        timerH -= Time.deltaTime;
        if (Input.GetAxisRaw("Horizontal") < 0 && s.value > s.minValue && timerH <= 0)
        {
            timerH = (firstTimerH) ? 0.5f : delay;
            firstTimerH = false;
            s.value--;
        }
        else if (Input.GetAxisRaw("Horizontal") > 0 && s.value < s.maxValue && timerH <= 0)
        {
            timerH = (firstTimerH) ? 0.5f : delay;
            firstTimerH = false;
            s.value++;
        }
        else if (Input.GetAxisRaw("Horizontal") == 0)
        {
            timerH = 0;
            firstTimerH = true;
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
        int counter = 0;
        for (int j = 0; j < 4; j++)
        {
            if (GameManager.playerOrder[j] != -1)
            {
                counter++;
            }
        }
        if(counter > 1)
        {
            audioManager.PlayAlert(MotherFuckingAudioManager.AlertList.BTN_VALIDATION);
            audioManager.PlayMusic(MotherFuckingAudioManager.MusicList.MAIN, true);
            StartCoroutine(ChangeSceneAnim());
        }
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
