using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static bool gameStarted;
    public static int playerCount = 2;
    int eliminatedPlayerCount = 0;
    public List<PlayerController> players;

    // Start is called before the first frame update
    void Start()
    {
        EventManager.onPlayerEliminated.AddListener(PlayerEliminated);
        gameStarted = false;
        StartCoroutine(ReadyStartAnim());
    }

    // Update is called once per frame
    void Update()
    {
    }

    IEnumerator ReadyStartAnim()
    {
        yield return new WaitForSeconds(3);
        gameStarted = true;

    }

    void PlayerEliminated()
    {
        eliminatedPlayerCount++;
        if (eliminatedPlayerCount == playerCount - 1)
            print("VICTORY");
    }
}
