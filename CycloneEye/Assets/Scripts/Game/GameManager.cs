using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    INITIALIZE, PLAYING, END, PAUSED
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public static int playerCount = 2;

    [SerializeField] List<PlayerController> players;
    [SerializeField] BlackPanel blackPanel;
    [SerializeField] GameObject startScreen;
    [SerializeField] GameObject endScreen;
    [SerializeField] GameObject pauseScreen;

    List<PlayerController> eliminationOrder;
    int isPaused;
    GameState state;
    public static GameState State { get { return Instance.state; } }
    public static int PauseIndex { get { return Instance.isPaused; } }

    // Start is called before the first frame update
    void Start()
    {
        blackPanel.Hide();
        state = GameState.INITIALIZE;
        startScreen.SetActive(true);
        Instance = this;
        isPaused = -1;
        eliminationOrder = new List<PlayerController>();
        StartCoroutine(ReadyStartAnim());
    }

    // Update is called once per frame
    void Update()
    {
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
            state = GameState.END;
            StartCoroutine(EndAnim());
        }
    }

    IEnumerator EndAnim()
    {
        print("GAME! -> \"Go to Score Scene\" animation");
        endScreen.SetActive(true);
        yield return new WaitForSeconds(1f);
        yield return blackPanel.ShowAnim();
        print("CHANGE SCENE");
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

}
