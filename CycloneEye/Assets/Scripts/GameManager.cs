using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static bool gameStarted;
    public static int playerCount = 2;
    public List<PlayerController> players;
    public List<PlayerController> eliminationOrder;

    public static GameManager Instance;
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        gameStarted = false;

        eliminationOrder = new List<PlayerController>();
        StartCoroutine(ReadyStartAnim());
    }

    // Update is called once per frame
    void Update()
    {
    }

    IEnumerator ReadyStartAnim()
    {
        print("READY ?");
        yield return new WaitForSeconds(3);
        print("GO !");
        gameStarted = true;
    }

    public void RemovePlayer(PlayerController player)
    {
        eliminationOrder.Add(player);
        player.gameObject.SetActive(false);
        EventManager.onPlayerEliminated.Invoke();

        if (eliminationOrder.Count == playerCount - 1)
        {
            print("GAME! -> \"Go to Score Scene\" animation");
        }
    }

}
