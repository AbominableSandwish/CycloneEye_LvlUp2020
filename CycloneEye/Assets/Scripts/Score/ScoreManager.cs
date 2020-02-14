using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ScoreManager
{
    // TODO : Define score
    public static int[] eliminations = new int[4];
    public static int[] roundWinners = new int[4];

    public static void InitScores()
    {
        for(int i = 0; i < 4; i++)
        {
            eliminations[i] = 0;
            roundWinners[i] = 0;
        }
    }

    public static int FinalScore(int index)
    {
        return roundWinners[index] + eliminations[index];
    }

    public static bool IsWinner(int index)
    {
        int score = FinalScore(index);
        for(int i = 0; i < GameManager.playerCount; i++)
        {
            if (score < FinalScore(i))
                return false;
        }
        return true;
    }
    public static int MaxScore()
    {
        int score = -1;
        for (int i = 0; i < GameManager.playerCount; i++)
        {
            int playerScore = FinalScore(i);
            if (score < playerScore)
                score = playerScore;
        }
        return score;
    }
}
