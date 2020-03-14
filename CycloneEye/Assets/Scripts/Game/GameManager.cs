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
    private CameraManager camera;
    public static GameManager Instance;
    public static int playerCount = 2;
    public static int maxRund = 3;
    public static float startTime = 180;
    public static int selectedMap = 0;
    static int roundCount = 1;

    public static int[] playerOrder = new int[4] {0, 1, 2, 3};

    [SerializeField] List<PlayerController> players;
    [SerializeField] List<GameObject> playerDamages;
    [SerializeField] BlackPanel blackPanel;
    [SerializeField] GameObject startScreen;
    [SerializeField] GameObject endScreen;
    [SerializeField] GameObject pauseScreen;

    List<PlayerController> eliminationOrder;
    int isPaused;
    GameState state;

    public static GameState State
    {
        get { return Instance.state; }
    }

    public static int PauseIndex
    {
        get { return Instance.isPaused; }
    }

    public static int Round
    {
        get { return roundCount; }
    }

    private float TimeRound = startTime;
    private Text timerText;

    private MotherFuckingAudioManager audioManager;

    // Start is called before the first frame update
    void Awake()
    {

        TimeRound = startTime;
        if (roundCount == 1)
            ScoreManager.InitScores();
        playerCount = 0;
        for (int i = 0; i < players.Count; i++)
        {
            players[i].eliminated = playerOrder[i] == -1;
            players[i].gameObject.SetActive(!players[i].eliminated);
            playerDamages[i].gameObject.SetActive(!players[i].eliminated);
            if (!players[i].eliminated)
                playerCount++;
        }

        blackPanel.Hide();
        state = GameState.INITIALIZE;
        startScreen.SetActive(true);
        Instance = this;
        isPaused = -1;
        eliminationOrder = new List<PlayerController>();
        StartCoroutine(ReadyStartAnim());
        timerText = GameObject.Find("TextTimer").GetComponent<Text>();
        int min = (int) (TimeRound / 60.0f);
        int sec = (int) (TimeRound - (min * 60));
        timerText.text = "0" + min + ":" + sec + "0";
        camera = Camera.main.GetComponent<CameraManager>();
        if(GameObject.Find("AudioManager") != null)
            audioManager = GameObject.Find("AudioManager").GetComponent<MotherFuckingAudioManager>();

    }

    void Start()
    {
        if(audioManager != null)
            audioManager.PlaySound(MotherFuckingAudioManager.SoundList.WIND, true);
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
            if (TimeRound <= 0)
            {
                TimeRound = 0;
                EndTime();
            }

            timerText.text = min.ToString("00") + ":" + sec.ToString("00");
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
        player.eliminated = true;
        camera.RemovePlayer(player);
        //player.gameObject.SetActive(false);
        //EventManager.onPlayerEliminated.Invoke();

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
        if (state != GameState.END)
        {
            state = GameState.END;
            for (int i = 0; i < 4; i++)
            {
                if (!players[i].eliminated)
                {
                    ScoreManager.roundWinners[i]++;
                    players[i].ChangePointsAnim(+1);
                }

            }

            endScreen.SetActive(true);
            yield return new WaitForSeconds(2f);
            yield return blackPanel.ShowAnim();

            if (roundCount == maxRund)
            {
                if (audioManager != null)
                    audioManager.PlayMusic(MotherFuckingAudioManager.MusicList.SCORE, true);
                roundCount = 1;
                StartCoroutine(EndGame());
            }
            else
            {
                roundCount++;
                SceneManager.LoadScene("Stage" + (GameManager.selectedMap + 1));
            }
        }
    }

    public void Pause(int playerIdx)
    {
        if (isPaused == playerIdx && state == GameState.PAUSED)
        {
            if (audioManager != null)
                audioManager.SetVolumeMusic(1.0f, true);
            isPaused = -1;
            state = GameState.PLAYING;
            Time.timeScale = 1;
            pauseScreen.SetActive(false);
        }
        else if (isPaused == -1 && state == GameState.PLAYING)
        {
            if(audioManager != null)
                audioManager.SetVolumeMusic(0.1f, true);
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
            PlayerController player = other.gameObject.GetComponent<PlayerController>();

            if (player.eliminated) return; // we avoid double count

            other.gameObject.GetComponentInChildren<SpriteRenderer>().sortingOrder = -50;
            other.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            player.State = PlayerState.KO;
            if(audioManager != null)
                audioManager.PlaySound(MotherFuckingAudioManager.SoundList.PLAYER_FALL);
            if (player.pusher == -1)
            {
                ScoreManager.eliminations[player.Index - 1]--;
                player.ChangePointsAnim(-1);
            }
            else
            {
                ScoreManager.eliminations[player.pusher - 1]++;
                players[player.pusher - 1].ChangePointsAnim(+1);
            }

            //endScreen.SetActive(true);
            // state = GameState.END;
            RemovePlayer(player);
        }
    }

    public void ShowPlayerEjectionWallWay()
    {

    }

    public static void Quit()
    {
        Instance.audioManager.PlayAlert(MotherFuckingAudioManager.AlertList.EXIT_GAME);
        Instance.audioManager.PlayMusic(MotherFuckingAudioManager.MusicList.MENU, true);
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

    IEnumerator EndGame()
    {
        yield return new WaitForSeconds(1.0f);
        SceneManager.LoadScene("SceneScore");
    }
}
