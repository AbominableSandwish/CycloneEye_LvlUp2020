using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] BlackPanel blackPanel;

    [SerializeField] Slider numberTurnSlider;
    [SerializeField] Slider timerSlider;

    bool initializing = false;

    // Start is called before the first frame update
    void Start()
    {
        blackPanel.Hide();
        initializing = true;
        numberTurnSlider.value = GameManager.maxRund;
        timerSlider.value = GameManager.startTime/30;
        initializing = false;

    }

    // Update is called once per frame
    void Update()
    {
        
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
