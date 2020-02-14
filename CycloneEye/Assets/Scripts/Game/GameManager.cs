using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum GameState
{
    INITIALIZE, PLAYING, END, PAUSED
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public static int playerCount = 2;
    public static int maxRund = 1;
    static int roundCount = 1;

    [SerializeField] List<PlayerController> players;
    [SerializeField] List<GameObject> playerDamages;
    [SerializeField] BlackPanel blackPanel;
    [SerializeField] GameObject startScreen;
    [SerializeField] GameObject endScreen;
    [SerializeField] GameObject pauseScreen;

    List<PlayerController> eliminationOrder;
    int isPaused;
    GameState state;
    public static GameState State { get { return Instance.state; } }
    public static int PauseIndex { get { return Instance.isPaused; } }
    public static int Round { get { return roundCount; } }

    private float TimeRound = 60 * 4;
    private Text timerText;

    // Start is called before the first frame update
    void Start()
    {
        if (roundCount == 1)
            ScoreManager.InitScores();
        for (int i = 0; i < players.Count; i++)
        {
            players[i].gameObject.SetActive(i < playerCount);
            playerDamages[i].gameObject.SetActive(i < playerCount);
        }
        blackPanel.Hide();
        state = GameState.INITIALIZE;
        startScreen.SetActive(true);
        Instance = this;
        isPaused = -1;
        eliminationOrder = new List<PlayerController>();
        StartCoroutine(ReadyStartAnim());
        timerText = GameObject.Find("TextTimer").GetComponent<Text>();
        int min = (int)(TimeRound / 60.0f);
        int sec = (int)(TimeRound - (min * 60));
        timerText.text = "0"+ min + ":" + sec + "0";
    }

    // Update is called once per frame
    void Update()
    {
        if (state == GameState.PLAYING)
        {
            //TIMER
            TimeRound -= Time.deltaTime;
            int min = (int) (TimeRound / 60.0f);
            int sec = (int) (TimeRound - (min * 60));
            timerText.text = min + ":" + sec;
        }
    }

    IEnumerator ReadyStartAnim()
    {
        yield return new WaitForSeconds(2.0f);
        state = GameState.PLAYING;
        yield return new WaitForSeconds(0.5f);
        startScreen.SetActive(false);
    }

    public void RemovePlayer(PlayerController player)
    {
        eliminationOrder.Add(player);
        player.gameObject.SetActive(false);
        EventManager.onPlayerEliminated.Invoke();

        if (eliminationOrder.Count == playerCount - 1)
        {
            StartCoroutine(EndAnim());
        }
    }

    public static void EndTime()
    {
        Instance.StartCoroutine(Instance.EndAnim());
    }

    IEnumerator EndAnim()
    {
        state = GameState.END;
        endScreen.SetActive(true);
        yield return new WaitForSeconds(1f);
        yield return blackPanel.ShowAnim();

        for(int i = 0; i < 4; i++)
        {
            if (players[i].gameObject.activeSelf)
                ScoreManager.roundWinners[i]++;
        }

        if (roundCount == maxRund)
        {
            roundCount = 1;
            SceneManager.LoadScene("SceneScore");
        }
        else
        {
            roundCount++;
            SceneManager.LoadScene("Issa");
        }
    }

    public void Pause(int playerIdx)
    {
        if (isPaused == playerIdx && state == GameState.PAUSED)
        {
            isPaused = -1;
            state = GameState.PLAYING;
            Time.timeScale = 1;
            pauseScreen.SetActive(false);
        }
        else if(isPaused == -1 && state == GameState.PLAYING)
        {
            isPaused = playerIdx;
            state = GameState.PAUSED;

            Time.timeScale = 0;
            pauseScreen.SetActive(true);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponentInChildren<SpriteRenderer>().sortingOrder = -50;
            other.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            other.gameObject.GetComponent<PlayerController>().State = PlayerState.KO;
           endScreen.SetActive(true);
            state = GameState.END;
        }
    }
    public static void Quit()
    {
        Time.timeScale = 1;
        Instance.StartCoroutine(Instance.QuitGame());
    }
    IEnumerator QuitGame()
    {
        roundCount = 1;
        state = GameState.END;
        yield return new WaitForSeconds(1f);
        yield return blackPanel.ShowAnim();
        SceneManager.LoadScene("SceneTitle");
    }


}
