using Assets._Scripts.Enum;
using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] CurrentScore playerScore;
    [SerializeField]CurrentScore enemyScore;

    public CurrentScore PlayerScore { get => playerScore; set => playerScore = value; }
    public CurrentScore EnemyScore { get => enemyScore; set => enemyScore = value; }

    

    public FightResult GetFinalScore()
    {
        FightResult fightResult = new FightResult();

        var playerFinalScore = playerScore.GetAllPoints();
        var enemyFinalScore = enemyScore.GetAllPoints();

        foreach(var value in playerFinalScore)
        {
            fightResult.UpdateScore(value.Key, value.Value - enemyFinalScore[value.Key]);
        }
        fightResult.Organize();
        fightResult.SetFinalSum();
        return fightResult;
    }

    public CurrentScore GetCurrentUserScore(bool playerRound)
    {
        return playerRound ? playerScore : enemyScore;
    }

}
