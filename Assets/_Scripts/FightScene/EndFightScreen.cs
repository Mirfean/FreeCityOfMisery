using System;
using TMPro;
using UnityEngine;

public class EndFightScreen : MonoBehaviour
{
    [SerializeField] TMP_Text WinLose;

    [SerializeField] TMP_Text player_name;
    [SerializeField] TMP_Text enemy_name;

    [SerializeField] TMP_Text player_score;
    [SerializeField] TMP_Text enemy_score;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetWinLose(bool x)
    {
        if(x)
        {
            WinLose.text = "You Win!";
        }
        else
        {
            WinLose.text = "You Lose!";
        }
    }

    public void SetPlayerData(string name, int value)
    {
        player_name.text = name;
        SetPlayerScore(value);
    }

    public void SetEnemyData(string name, int value)
    {
        enemy_name.text = name;
        SetEnemyScore(value);
    }

    void SetPlayerScore(int value)
    {
        player_score.text = value.ToString();
    }

    void SetEnemyScore(int value)
    {
        enemy_score.text = value.ToString();
    }

    internal bool SetEndScreenData(CurrentScore PlayerScore, CurrentScore EnemyScore)
    {
        int playerFinalScore = 0;
        int enemyFinalScore = 0;

        foreach (var value in PlayerScore.GetAllPoints())
        {
            playerFinalScore += value.Value;
        }

        foreach (var value in EnemyScore.GetAllPoints())
        {
            enemyFinalScore += value.Value;
        }

        SetPlayerData("Player", playerFinalScore);
        SetEnemyData("Enemy", enemyFinalScore);
        return playerFinalScore > enemyFinalScore;
    }
}
