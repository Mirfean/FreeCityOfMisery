using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EndFightScreen : MonoBehaviour
{
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
}
